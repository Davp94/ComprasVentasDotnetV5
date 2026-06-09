using System.ComponentModel.DataAnnotations;

namespace ComprasVentas;

public record RefreshTokenRequest
{
    [Required]
    public string RefreshToken;
}
