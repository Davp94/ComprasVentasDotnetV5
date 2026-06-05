using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComprasVentas;


public class Permiso
{
    
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty; // ""
 
    public string? Recurso { get; set; }

    public string? Action { get; set; }

    public List<Rol> Roles { get; set; } = [];
}
