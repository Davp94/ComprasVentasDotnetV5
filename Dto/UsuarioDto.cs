using System;

namespace ComprasVentas;

public class UsuarioDto
{ 
    public string Id { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string? FechaNacimiento { get; set; }
    public string? Genero { get; set; }
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
    public string? Nacionalidad { get; set; }

    //TODO add documentos
}
