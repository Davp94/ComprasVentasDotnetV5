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

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Persona> Personas { get; set; }
    public DbSet<Documento> Documentos { get; set; }
    public DbSet<Rol> Roles { get; set; }
    public DbSet<Permiso> Permisos { get; set; }

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

        modelBuilder.Entity<Usuario>(e=>
        {
            e.Property(c=>c.Id).ValueGeneratedOnAdd();
            e.Property(c=>c.Nombre).IsRequired().HasMaxLength(50);
            e.Property(c=>c.Email).IsRequired().HasMaxLength(200);
            e.Property(c=>c.Password).IsRequired().HasMaxLength(200);
            
            e.HasIndex(c => c.Nombre).IsUnique();
            e.HasIndex(c => c.Email).IsUnique();
        }); 

        modelBuilder.Entity<Usuario>()
           .HasMany(u => u.Roles)
           .WithMany(r => r.Usuarios)
           .UsingEntity(j => j.ToTable("UsuariosRoles"));

        modelBuilder.Entity<Rol>()
           .HasMany(p => p.Permisos)
           .WithMany(r => r.Roles)
           .UsingEntity(j => j.ToTable("RolesPermisos"));   

        modelBuilder.Entity<Persona>(e=>
        {
            e.Property(p=>p.Nombres).IsRequired().HasMaxLength(100);
            e.Property(p=>p.Apellidos).IsRequired().HasMaxLength(100);
            e.Property(p=>p.FechaNacimiento);
            e.Property(p=>p.Genero).HasMaxLength(20);
            e.Property(p=>p.Telefono).HasMaxLength(20);
            e.Property(p=>p.Direccion).HasMaxLength(300);
            e.HasOne(u=>u.Usuario).WithOne(p=>p.Persona)
            .HasForeignKey<Usuario>(p=>p.PersonaId);
        });

          modelBuilder.Entity<Documento>(e=>
        {
            e.Property(c=>c.Id).ValueGeneratedOnAdd();
            e.Property(c=>c.Nombre).IsRequired().HasMaxLength(50);
            e.Property(c=>c.DescripcionDoc).HasMaxLength(200);
             e.HasOne(p=>p.Persona);
        });

        modelBuilder.Entity<Rol>(e=>
        {
            e.Property(c=>c.Id).ValueGeneratedOnAdd();
            e.Property(c=>c.Nombre).IsRequired().HasMaxLength(100);
            e.Property(c=>c.Descripcion).HasMaxLength(300);
            
            e.HasIndex(c => c.Nombre).IsUnique();
        });

        modelBuilder.Entity<Permiso>(e=>
        {
            e.Property(c=>c.Id).ValueGeneratedOnAdd();
            e.Property(c=>c.Nombre).IsRequired().HasMaxLength(100);
            e.Property(c=>c.Recurso).HasMaxLength(100);
            e.Property(c=>c.Action).IsRequired().HasMaxLength(100);
            
            e.HasIndex(c => c.Nombre).IsUnique();
        });
    }

}
