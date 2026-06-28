using System;

namespace ComprasVentas;

public class CreateNotaDto
{
    public decimal Impuestos { get; set; }

    public decimal Total { get; set; }

    public decimal Descuentos { get; set; }

    public string Observaciones { get; set; }

    public string Tipo { get; set; }

    public int ClienteProveedorId { get; set; }

    public int UsuarioId { get; set; }

    public List<CreateMovimientoDto> Movimientos { get; set; }
}
