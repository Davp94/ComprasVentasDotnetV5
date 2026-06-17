using System;

namespace ComprasVentas;

public interface IFileService
{
    Task<string> SaveFileAsync(IFormFile file);

    Task<Stream> GetFileASync(string filePath);
}
