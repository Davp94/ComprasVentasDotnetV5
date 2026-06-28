using System;
using AutoMapper;
using Dapper;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ComprasVentas;

public class NotaService(AppDbContext appDbContext, IMapper mapper) : INotaService
{
    private readonly AppDbContext _appDbContext =appDbContext;

    private readonly IMapper _mapper = mapper;

    public async Task<NotaResponseDto> CreateAsync(CreateNotaDto createNotaDto)
    {
        decimal calculateTotal = createNotaDto.Movimientos.Sum(m=>m.Cantidad * (createNotaDto.Tipo == "COMPRA" ? m.PrecioUnitarioCompra : m.PrecioUnitarioVenta));

        if(Math.Abs(calculateTotal + createNotaDto.Impuestos - createNotaDto.Descuentos - createNotaDto.Total) > 0.01m)
        {
            throw new Exception("El total proporcionado no coincide con el cálculo realizado");
        }
        var connection = _appDbContext.Database.GetDbConnection();
        if(connection.State == System.Data.ConnectionState.Closed)
            await connection.OpenAsync();
        var transaction = connection.BeginTransaction();

        try
        {
            const string sqlNota = @"
                INSERT INTO notas(fecha, tipo_nota, descuento, impuestos, total, estado, observaciones, usuario_id, cliente_proveedor_id) 
                VALUES (@Fecha, @TipoNota, @Descuento, @Impuestos, @Total, @Estado, @Observaciones, @UsuarioId, @ClienteProveedorId)
                RETURNING id;
            ";
            var notaId = await connection.ExecuteScalarAsync<int>(sqlNota, new
            {
                Fecha = DateTime.UtcNow,
                TipoNota = createNotaDto.Tipo,
                Descuento = createNotaDto.Descuentos,
                Impuestos = createNotaDto.Impuestos,
                Total = createNotaDto.Total,
                Estado = true,
                Observaciones = createNotaDto.Observaciones,
                UsuarioId = createNotaDto.UsuarioId,
                ClienteProveedorId = createNotaDto.ClienteProveedorId
            }, transaction);

            //add movimientos
            foreach (var movDto in createNotaDto.Movimientos)
            {
                const string sqlMovimientos = @"
                    INSERT INTO movimientos(tipo_movimiento, cantidad, precio_unitario_compra, precio_unitario_venta, observaciones, producto_id, almacen_id, nota_id) 
                    VALUES (@TipoMovimiento, @Cantidad, @PrecioUnitarioCompra, @PrecioUnitarioVenta, @Observaciones, @ProductoId, @AlmacenId, @NotaId);
                ";
                await connection.ExecuteAsync(sqlMovimientos, new
                {   
                    TipoMovimiento = createNotaDto.Tipo,
                    Cantidad = movDto.Cantidad,
                    PrecioUnitarioCompra = movDto.PrecioUnitarioCompra,
                    PrecioUnitarioVenta = movDto.PrecioUnitarioVenta,
                    ProductoId = movDto.ProductoId,
                    AlmacenId = movDto.AlmacenId,
                    Observaciones = movDto.Observaciones,
                    NotaId = notaId
                }, transaction);

                const string sqlCheckStock = "SELECT id, cantidad_actual AS CantidadActual FROM almacen_productos WHERE producto_id = @ProductoId AND almacen_id = @AlmacenId LIMIT 1;";
                var stockItem = await connection.QueryFirstOrDefaultAsync<StockCheckResult>(sqlCheckStock, new { ProductoId = movDto.ProductoId, AlmacenId = movDto.AlmacenId}, transaction);
                var cantidadCambio = createNotaDto.Tipo == "COMPRA" ? (int)movDto.Cantidad : -(int)movDto.Cantidad; 

                if (stockItem != null)
                {
                    const string sqlUpdateStock = "UPDATE almacen_productos SET cantidad_actual = cantidad_actual + @CantidadCambio, fecha_actualizacion = @Fecha WHERE id = @Id;";
                    await connection.ExecuteAsync(sqlUpdateStock, new
                    {
                        CantidadCambio = cantidadCambio,
                        Fecha = DateTime.UtcNow,
                        Id = stockItem.Id
                    }, transaction);
                }
                else
                {
                    if (createNotaDto.Tipo == "VENTA")
                    {
                        throw new Exception($"No hay stock disponible en el almacen para el producto {movDto.ProductoId}");
                    }

                    const string sqlInsertStock = "INSERT INTO almacen_productos(cantidad_actual, fecha_actualizacion, precio_venta_actual, estado, producto_id, almacen_id) VALUES (@Cantidad, @Fecha, @PrecioVenta, @Estado, @ProductoId, @AlmacenId);";

                    await connection.ExecuteAsync(sqlInsertStock, new
                    {
                        Cantidad = (int)movDto.Cantidad,
                        Fecha = DateTime.UtcNow,
                        PrecioVenta = movDto.PrecioUnitarioVenta,
                        Estado = true,
                        ProductoId = movDto.ProductoId,
                        AlmacenId = movDto.AlmacenId
                    }, transaction);
                }
            }
            //update inventario
            
            //close transaction
            await transaction.CommitAsync();
            //return data
            return await FindByIdAsync(notaId);
        }
        catch (System.Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<IEnumerable<NotaResponseDto>> FindAllAsync()
    {
        var connection = _appDbContext.Database.GetDbConnection();
        const string sql = @"
            SELECT n.id as Id, n.fecha as Fecha, n.tipo_nota as TipoNota, n.descuento as Descuento, n.impuestos as Impuestos, n.total as Total, n.estado as Estado, n.observaciones as Observaciones,
                   n.usuario_id as UsuarioId, n.cliente_proveedor_id as ClienteProveedorId,
                   u.nombre as UsuarioNombre,
                   cp.razon_social as ClienteProveedorRazonSocial
            FROM notas n 
            LEFT JOIN usuarios u ON n.usuario_id = u.id
            LEFT JOIN cliente_proveedor cp ON cp.id = n.cliente_proveedor_id
            ORDER BY n.fecha DESC; 
        ";
        return await connection.QueryAsync<NotaResponseDto>(sql);
    }

    public async Task<NotaResponseDto> FindByIdAsync(int id)
    {
        var connection = _appDbContext.Database.GetDbConnection();
        const string sqlNota = @"
            SELECT n.id as Id, n.fecha as Fecha, n.tipo_nota as TipoNota, n.descuento as Descuento, n.impuestos as Impuestos, n.total as Total, n.estado as Estado, n.observaciones as Observaciones,
                   n.usuario_id as UsuarioId, n.cliente_proveedor_id as ClienteProveedorId,
                   u.nombre as UsuarioNombre,
                   cp.razon_social as ClienteProveedorRazonSocial
            FROM notas n
            LEFT JOIN usuarios u ON n.usuario_id = u.id
            LEFT JOIN cliente_proveedor cp ON cp.id = n.cliente_proveedor_id
            WHERE n.id = @Id;
        ";

        const string sqlMovimientos = @"
            SELECT m.id as Id, m.cantidad as Cantidad, m.precio_unitario_compra as PrecioUnitarioCompra, m.precio_unitario_venta as PrecioUnitarioVenta, m.observaciones as Observaciones,
                   m.producto_id as ProductoId, m.almacen_id as AlmacenId, m.nota_id as NotaId,
                   p.nombre as ProductoNombre,
                   a.nombre as AlmacenNombre
            FROM movimientos m
            LEFT JOIN productos p ON p.id = m.producto_id
            LEFT JOIN almacenes a ON a.id = m.almacen_id
            WHERE m.nota_id = @NotaId;
        ";
        var nota = await connection.QueryFirstOrDefaultAsync<NotaResponseDto>(sqlNota, new { Id = id });

        if (nota != null)
        {
            var movimientos = await connection.QueryAsync<MovimientoResponseDto>(sqlMovimientos, new { NotaId = id });
            nota.Movimientos = movimientos.ToList();
        }
        return nota;
    }

    private class StockCheckResult
    {
        public int Id { get; set; }
        public int CantidadActual { get; set; }
    }

    public async Task<byte[]> GenerateNotaReportAsync(int notaId)
    {
        //GetData
        var nota = await _appDbContext.Notas
            .Include(n=>n.ClienteProveedor)
            .Include(n=>n.Movimientos)
            .ThenInclude(n=>n.Producto)
            .FirstOrDefaultAsync(n=>n.Id == notaId);
        //generate report
        QuestPDF.Settings.License = LicenseType.Community;

        var document = Document.Create(container =>
        {
           container.Page(page =>
           {
              page.Size(PageSizes.Letter);
              page.Margin(2, Unit.Centimetre);
              page.PageColor(Colors.White);
              page.DefaultTextStyle(x=>x.FontSize(12));
              page.Header().Row(row=>
              {
                  row.RelativeItem().Column(column =>
                  {
                      column.Item().Text($"Nota - {nota.Id}").SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);
                      column.Item().Text($"Fecha: {nota.Fecha:dd/MM/yyyy}");
                      column.Item().Text($"Cliente/Proveedor: {nota.ClienteProveedor.RazonSocial}");
                  });
              });
              page.Content().Column(column =>
              {
                var subTotal = 0.00m;
                  column.Item().Table(table =>
                  {
                      table.ColumnsDefinition(columns =>
                      {
                          columns.RelativeColumn(3);
                          columns.RelativeColumn();
                          columns.RelativeColumn();
                          columns.RelativeColumn();
                     });
                     table.Header(header =>
                     {
                        header.Cell().Element(CellStyle).Text("Producto"); 
                        header.Cell().Element(CellStyle).Text("Cantidad"); 
                        header.Cell().Element(CellStyle).Text("Precio"); 
                        header.Cell().Element(CellStyle).Text("Total"); 
                        static IContainer CellStyle(IContainer container)
                         {
                             return container.DefaultTextStyle(x=>x.SemiBold()).Padding(5).Border(1).BorderColor(Colors.Grey.Lighten3);
                         }
                     });
                     
                     foreach (var item in nota.Movimientos)
                      {
                          var precio = nota.TipoNota == "Entrada" ? item.PrecioUnitarioCompra : item.PrecioUnitarioVenta;
                          var totalItem = item.Cantidad * precio;
                          subTotal = subTotal + totalItem;
                          table.Cell().Element(CellStyle).Text(item.Producto.Nombre);
                          table.Cell().Element(CellStyle).Text(item.Cantidad.ToString());
                          table.Cell().Element(CellStyle).Text($"{precio:F2}");
                          table.Cell().Element(CellStyle).Text($"{totalItem:F2}");
                          
                          static IContainer CellStyle(IContainer container)
                         {
                             return container.DefaultTextStyle(x=>x.SemiBold()).Padding(5).Border(1).BorderColor(Colors.Grey.Lighten3);
                         }
                      }
                  });
                  column.Item().AlignRight().Text($"Subtotal: {subTotal- nota.Impuestos + nota.Descuento:F2}").SemiBold();
              });
              page.Footer()
              .AlignCenter().Text(
                x =>
                {
                    x.Span("Nota de Compra/venta del sistema").FontSize(12);
                    x.Line("https://misistema.com").FontSize(10).FontColor(Colors.Grey.Medium);
                }
              );
           }); 
        });
        //return report
        return document.GeneratePdf();
    }
}
