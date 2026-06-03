using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Diagnostics;

namespace ComprasVentas;

public class ResourceNotFoundException : DomainException
{
    public ResourceNotFoundException(string resource, object id) : base($"{resource} con id {id} no encontrado", 404, "NOT_FOUND"){}
    
}
