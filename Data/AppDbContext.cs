using System;
using Microsoft.EntityFrameworkCore;

namespace ComprasVentas;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Categoria> Categorias { get; set; }

    public DbSet<Producto> Productos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuración adicional de las (entidades == modelos) si es necesario
        modelBuilder.Entity<Categoria>(e=>
        {
            e.Property(c=>c.Id).ValueGeneratedOnAdd();
            e.Property(c=>c.Nombre).IsRequired().HasMaxLength(50);
            e.Property(c=>c.Descripcion).HasMaxLength(200);
            
            e.HasIndex(c => c.Nombre).IsUnique();
        });
        
        modelBuilder.Entity<Producto>(e=>
        {
            e.Property(p=>p.Nombre).IsRequired();
            e.Property(p=>p.Descripcion).HasMaxLength(500);
            e.Property(p=>p.UnidadMedida).HasMaxLength(20);
            e.Property(p=>p.Marca).HasMaxLength(50);
            e.Property(p=>p.PrecioVentaActual).IsRequired().HasPrecision(18,2);
            e.Property(p=>p.Estado).HasDefaultValue(true);
            e.HasOne(p=>p.Categoria);
        }); 
    }

}
