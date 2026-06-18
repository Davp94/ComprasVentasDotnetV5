using System;

namespace ComprasVentas;

public class ClienteProveedor
{
    public int Id { get; set; }

    public string Tipo { get; set; }

    public string RazonSocial { get; set; }

    public string NroIdentificacion {get; set;}
    public string Telefono {get; set;}

    public string Correo { get; set; }
    public bool Estado { get; set; }

    public List<Nota> Notas { get; set; }

}
