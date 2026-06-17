using System;

namespace ComprasVentas;

public class FileService(IConfiguration configuration, IWebHostEnvironment environment) : IFileService
{
    private readonly IConfiguration _configuration = configuration;

    private readonly IWebHostEnvironment _environment = environment;


    public Task<Stream> GetFileASync(string filePath)
    {
        var fullPath = Path.Combine(_environment.ContentRootPath, filePath);
        if(!File.Exists(fullPath))
        {
            throw new FileNotFoundException("Archivo no encontrado", fullPath);
        }
        return Task.FromResult<Stream>(new FileStream(fullPath, FileMode.Open, FileAccess.Read));
    }

    public async Task<string> SaveFileAsync(IFormFile file)
    {
        if(file == null || file.Length == 0)
        {
            throw new Exception("Archivo es nulo o vacío");
        }

        var storagePath = _configuration.GetValue<string>("Storage:ImageDirectory");

        var uploadPath = Path.Combine(_environment.ContentRootPath, storagePath);

        if(!Directory.Exists(uploadPath))
        {
            Directory.CreateDirectory(uploadPath);
        }

        var fileName = $"{Guid.NewGuid()}_{file.FileName}";
        var filePath = Path.Combine(uploadPath, fileName);

        using(var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        var routeResponse = Path.Combine(storagePath, fileName); 
        return routeResponse.Replace("\\", "/");
    }
}
