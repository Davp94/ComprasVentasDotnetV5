using System;

namespace ComprasVentas;

public class Persona
{
    public int Id { get; set; }

    public string Nombres { get; set; } = string.Empty; // ""

    public string Apellidos { get; set; } = string.Empty;

    public DateOnly FechaNacimiento { get; set; }

    public string? Genero { get; set; }

    public string? Telefono { get; set; }

    public string? Direccion { get; set; }

    public string? Nacionalidad { get; set; }

    public Usuario Usuario { get; set; }

    public List<Documento> Documentos { get; set;} = [];
}
