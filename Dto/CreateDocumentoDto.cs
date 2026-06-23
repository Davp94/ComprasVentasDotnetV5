using System;

namespace ComprasVentas.Dto;

public class CreateDocumentoDto
{
    public int Id {get; set;}
    public string Nombre {get; set; }
    public string DescripcionDoc { get; set; }
    public IFormFile? Referencia { get; set; }
}
