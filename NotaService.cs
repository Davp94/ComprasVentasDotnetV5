using System;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ComprasVentas;

public class NotaService(AppDbContext appDbContext) : INotaService
{
    private readonly AppDbContext _appDbContext =appDbContext;
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
                  });
              });
           }); 
        });
        //return report
    }
}
