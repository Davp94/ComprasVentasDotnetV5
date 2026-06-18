using System;

namespace ComprasVentas;

public class AlmacenProducto
{
    public int Id {get; set; }
    public int CantidadActual { get; set; }
    public DateTime FechaActualizacion { get; set; }
    public decimal PrecioVentaActual { get; set; } 
    public bool? Estado { get; set; }

    public Producto? Producto { get; set; }

    public Almacen? Almacen { get; set; }
 

}
