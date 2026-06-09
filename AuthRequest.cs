using System;
using System.ComponentModel.DataAnnotations;

namespace ComprasVentas;

public record AuthRequest
{
    [Required]
    [EmailAddress]
    public string Email;

    [Required]
    public string Password;
}
