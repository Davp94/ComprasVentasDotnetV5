using System;
using System.Collections.Generic;

namespace ComprasVentas;

public class CreateRolDto
{
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public List<int> PermisoIds { get; set; } = new();
}
