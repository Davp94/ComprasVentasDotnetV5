using System;
using System.Collections.Generic;

namespace ComprasVentas;

public class RolDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public List<int> PermisoIds { get; set; } = new();
}
