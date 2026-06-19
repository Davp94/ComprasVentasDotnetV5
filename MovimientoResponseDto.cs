using System;

namespace ComprasVentas;

public class MovimientoResponseDto
{
    public int Id { get; set; }
    public decimal Cantidad { get; set; }

    public string TipoMovimiento { get; set; }
    public decimal PrecioUnitarioCompra { get; set; }
    public decimal PrecioUnitarioVenta { get; set; }

    public string Observaciones { get; set; }

    public int ProductoId { get; set; }
    public string ProductoNombre { get; set; }

    public int AlmacenId { get; set; }
    public string AlmacenNombre { get; set; }

}
