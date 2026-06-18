using System;

namespace ComprasVentas;

public class Producto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty; // ""

    public string? Descripcion { get; set; }

    public string? UnidadMedida { get; set; }

    public string? Marca { get; set; }

    public decimal PrecioVentaActual { get; set; }

    public string? Imagen { get; set; }

    public bool Estado { get; set; }

    public Categoria? Categoria { get; set; }

    public List<AlmacenProducto> AlmacenProductos { get; set; } = [];

    public List<Movimiento> Movimientos { get; set; } = [];
}
