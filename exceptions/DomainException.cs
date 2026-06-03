using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Diagnostics;

namespace ComprasVentas;

public abstract class DomainException : Exception
{
    public int StatusCode { get; } // 400
    public string ErrorCode { get; } // Bad Request 
    //TODO add logs

    public DomainException(string message, int statusCode, string erroCode) : base(message)
    {
        StatusCode = statusCode;
        ErrorCode = erroCode;
    }
}
