using System;

namespace ComprasVentas;

public class NotaResponseDto
{
    public int Id { get; set; }

    public string Fecha { get; set; }

    public string TipoNota { get; set; }

    public decimal Descuento { get; set; }

    public decimal Impuestos { get; set; }

    public decimal Total { get; set; }

    public string Observaciones { get; set; }

    public int UsuarioId { get; set; }

    public string UsuarioNombre { get; set; }

    public int ClienteProveedorId { get; set; }

    public string ClienteProveedorRazonSocial { get; set; }

    public IEnumerable<MovimientoResponseDto> Movimientos { get; set; }

}
