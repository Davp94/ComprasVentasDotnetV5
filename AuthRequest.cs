using System;
using System.ComponentModel.DataAnnotations;

namespace ComprasVentas;

public record AuthRequest
(
    [Required]
    [EmailAddress]
    string Email,

    [Required]
    string Password
);
