using System;

namespace ComprasVentas;

public class CreateProductoDto
{

    public string Nombre { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    public string? UnidadMedida { get; set; }

    public string? Marca { get; set; }

    public decimal PrecioVentaActual { get; set; }

    public IFormFile? Imagen { get; set; } //Type File (Archivo)

    public bool Estado { get; set; }

    public int CategoriaId { get; set; }
}
