using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComprasVentas;

public class Rol
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty; // ""

    public string? Descripcion { get; set; }

    public List<Permiso> Permisos { get; set; } = [];

    public List<Usuario> Usuarios { get; set; } = [];
}
