using System;
using System.ComponentModel.DataAnnotations;

namespace ComprasVentas;

public class CreateUsuarioDto
{
    [Required(ErrorMessage = "El username es obligatorio")]
    [UniqueName("Username")]
    [StringLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo es obligatorio")]
    [EmailAddress]
    [StringLength(50)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo es obligatorio")]
    [StringLength(16, MinimumLength = 8)]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).{8,}$",
    ErrorMessage = "La contraseña debe tener mínimo 8 caracteres, 1 mayúscula, 1 número y 1 carácter especial.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100)]
    public string Nombres { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100)]
    public string Apellidos { get; set; } = string.Empty;

    [RegularExpression(@"^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[0-2])/(19[0-9][0-9]|20[0-9][0-9]|2100)$",
    ErrorMessage = "Fecha inválida. Use formato dd/mm/yyyy (1900-2100)")]
    public string? FechaNacimiento { get; set; }

    public string? Genero { get; set; }

    public string? Telefono { get; set; }

    public string? Direccion { get; set; }

    public string? Nacionalidad { get; set; }

    public List<int> RolIds { get; set; } = [];

    //TODO add documentos
}
