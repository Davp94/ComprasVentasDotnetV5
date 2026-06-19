using System;

namespace ComprasVentas;

public interface INotaService
{
    Task<byte[]> GenerateNotaReportAsync(int notaId);
    Task<IEnumerable<NotaResponseDto>> FindAllAsync();
    Task<NotaResponseDto> FindByIdAsync(int id);
    Task<NotaResponseDto> CreateAsync(CreateNotaDto createNotaDto);
}
