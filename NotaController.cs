using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComprasVentas
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotaController(INotaService notaService) : ControllerBase
    {
        private readonly INotaService _notaService = notaService;

        [HttpGet("{id}/report")]
        public async Task<ActionResult> GenerateNotaReport(int id)
        {
            var pdf = await _notaService.GenerateNotaReportAsync(id);
            return File(pdf, "application/pdf", $"nota_{id}_report.pdf");
        }
    }
}
