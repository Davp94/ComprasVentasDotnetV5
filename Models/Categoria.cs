using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComprasVentas;

[Table("Categorias")]
public class Categoria
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Nombre { get; set; } = string.Empty; // ""

    [MaxLength(200)]
    public string? Descripcion { get; set; }
}
