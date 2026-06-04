using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ComprasVentas;

public class GlobalExceptionHandler(ILogger<UsuarioController> logger) : IExceptionHandler
{
    private readonly ILogger<UsuarioController> _logger = logger;
    //TODO add logs
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception ex, CancellationToken cancellationToken)
    {
        var (status, code) = ex switch
        {
            DomainException d => (d.StatusCode, d.ErrorCode),
            ValidationException => (400, "Error en validacion"),
            _=> (500, "Internal Server Error")
        };

        var problem = new ProblemDetails
        {
          Status = status,
          Title = code,
          Detail = ex.Message,
          Instance = httpContext.Request.Path
        };
        _logger.LogError("Logging Exception {ex}", ex);
        problem.Extensions["traceId"] = httpContext.TraceIdentifier;
        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
        return true;
    }
}
