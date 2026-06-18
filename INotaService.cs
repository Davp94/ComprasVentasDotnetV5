using System;

namespace ComprasVentas;

public interface INotaService
{
    Task<byte[]> GenerateNotaReportAsync(int notaId);
}
