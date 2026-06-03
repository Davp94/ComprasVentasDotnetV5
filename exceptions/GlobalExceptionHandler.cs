using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.Marshalling;
using Microsoft.AspNetCore.Diagnostics;

namespace ComprasVentas;

public class GlobalExceptionHandler : IExceptionHandler
{
    //TODO add logs
    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception ex, CancellationToken cancellationToken)
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
        problem.Extension["traceId"] = httpContext.TraceIdentifier;
        httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
        return true;
    }
}
