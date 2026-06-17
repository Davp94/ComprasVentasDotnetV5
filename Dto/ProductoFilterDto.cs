using System;

namespace ComprasVentas;

public class ProductoFilterDto
{
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;

    public string SortOrder { get; set; } = "asc";

    public string SortField { get; set; } = "Id";

//Global filter
    public string? filterValue { get; set; }

//Specific filter
    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public string? Marca { get; set; }    

    public string? NombreCategoria { get; set; }


}
