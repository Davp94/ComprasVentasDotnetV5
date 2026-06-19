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
                INSERT INTO notas(fecha, tipoNota, descuento, impuestos, total, estado, observaciones, usuarioId, clienteProveedorId) VALUES (@Fecha, @TipoNota, @Descuento, @Impuestos, @Total, @Estado, @Observaciones, @Usuarioid, @ClienteProveedorId)
                RETURNING id;
            ";
            var notaId = await connection.ExecuteScalarAsync<int>(sqlNota, new
            {
                Fecha = DateTime.UtcNow,
                TipoNota = createNotaDto.Tipo,
                Descuento = createNotaDto.Descuentos,
                Impuesto = createNotaDto.Impuestos,
                Total = createNotaDto.Total,
                Estado = "",
                Observaciones = createNotaDto.Observaciones,
                UsuarioId = createNotaDto.UsuarioId,
                ClienteProveedorId = createNotaDto.ClienteProveedorId
            }, transaction);

            //add movimientos
            //update inventario
            //close transaction
            //return data
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }

    public async Task<IEnumerable<NotaResponseDto>> FindAllAsync()
    {
        var connection = _appDbContext.Database.GetDbConnection();
        const string sql = @"
            select n.*, u.Nombre as UsuarioNombre,
            cp.razonSocial as ClienteProveedorRazonSocial
            from notas n 
            left join usuarios u on n.usuarioId = u.id
            left join cliente_proveedor cp on cp.id = n.clienteProveedorId
            order by n.Fecha DESC; 
        ";
        return await connection.QueryAsync<NotaResponseDto>(sql);
    }

    public async Task<NotaResponseDto> FindByIdAsync(int id)
    {
        var connection = _appDbContext.Database.GetDbConnection();
        const string sqlNota=@"
            select n.*, u.Nombre as UsuarioNombre,
            cp.razonSocial as ClienteProveedorRazonSocial
            from notas n
            left join usuarios u on n.usuarioId = u.id
            left join cliente_proveedor cp on cp.id = n.clienteProveedorId
            where n.Id = @Id;
        ";

        const string sqlMovimientos = @"
            select m.*,
            p.Nombre as ProductoNombre,
            a.Nombre as AlmacenNombre
            from movimientos m
            left join productos p ON p.productoId = p.Id
            left join almacenes a ON a.almacenId = a.Id
            where m.notaId = @NotaId;
        ";
        var nota = await connection.QueryFirstOrDefaultAsync<NotaResponseDto>(sqlNota, new { Id = id });

        if(nota != null)
        {
            var movimientos = await connection.QueryAsync<MovimientoResponseDto>(sqlMovimientos, new { NotaId = id });
            nota.Movimientos = movimientos.ToList();
        }
        return nota;
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
