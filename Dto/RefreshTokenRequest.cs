using System.ComponentModel.DataAnnotations;

namespace ComprasVentas;

public record RefreshTokenRequest
(
    [Required]
    string RefreshToken
);
