using System;
using System.Collections.Generic;

namespace ComprasVentas;

public class PermisoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Recurso { get; set; }
    public string? Action { get; set; }

}
