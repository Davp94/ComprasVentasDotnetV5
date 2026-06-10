using System;

namespace ComprasVentas;

public class RefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; } = string.Empty;

    public DateTime Created { get; set; } = DateTime.Now;

    public DateTime Expires { get; set; }

    public bool IsActive { get; set; }

    public Usuario usuario { get; set; } = null;

    public int UsuarioId { get; set; }
}
