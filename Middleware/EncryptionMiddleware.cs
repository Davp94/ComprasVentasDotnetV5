using System;
using System.Text;
using ComprasVentas.Services.impl;

namespace ComprasVentas.Middleware;

public class EncryptionMiddleware(RequestDelegate next, EncryptionService encryptionService)
{
    private readonly RequestDelegate _next = next;

    private readonly EncryptionService _encryptionService = encryptionService;

    public async Task InvokeAsync(HttpContext context)
    {
        //dcrypt request
        if(context.Request.ContentLength > 0)
        {
            var reader = new StreamReader(context.Request.Body);
            string encryptedBody = await reader.ReadToEndAsync();
            try
            {
                var decryptedBody = _encryptionService.Decrypt(encryptedBody);
                var byteArray = Encoding.UTF8.GetBytes(decryptedBody);
                var stream = new MemoryStream(byteArray);
                context.Request.Body = stream;
                context.Request.ContentLength = stream.Length;
            }
            catch (System.Exception)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Texto encriptado inválido");
                return;   
            }
        }
    //Encrypt response
        var originalBodyStream = context.Response.Body;

        var responseBody = new MemoryStream();
        context.Response.Body = responseBody;
        await _next(context);
        context.Response.Body = originalBodyStream;
        responseBody.Seek(0, SeekOrigin.Begin);

        var reader = new StreamReader(responseBody);
        string plainResponse = await reader.ReadToEndAsync();

        var encryptedResponse = _encryptionService.Encrypt(plainResponse);
        await context.Response.WriteAsync(encryptedResponse);

    }
}
