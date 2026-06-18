using System;

namespace ComprasVentas;

public class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty; // ""

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public int PersonaId { get; set; }
    public Persona Persona { get; set; }

    public List<Rol> Roles { get; set; } = [];

    public List<RefreshToken> RefreshTokens { get; set; } = [];

    public List<Nota> Notas { get; set; }

}
