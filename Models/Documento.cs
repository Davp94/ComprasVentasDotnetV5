using System;

namespace ComprasVentas;

public class Documento
{
    public int Id { get; set; }

    public string Nombre { get; set; }

    public string DescripcionDoc { get; set; }
    //camelCase public string descripcionDoc { get; set; }
    //snakeCase public string descripcion_doc { get; set; }

    public string Referencia { get; set; }

    public Persona Persona { get; set; }
}
