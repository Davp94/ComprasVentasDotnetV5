using System;

namespace ComprasVentas;

public class CreateCategoriaDto
{
    public string Nombre { get; set; } = string.Empty;

    public string? Descripcion { get; set; }
}
