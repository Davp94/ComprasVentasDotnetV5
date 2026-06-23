using System;
using System.Text.RegularExpressions;
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

    public DbSet<AlmacenProducto> AlmacenProductos { get; set; }

    public DbSet<Sucursal> Sucursales { get; set; }

    public DbSet<Almacen> Almacenes { get; set; }
    public DbSet<Nota> Notas { get; set; }
    public DbSet<ClienteProveedor> ClienteProveedor { get; set; }
    public DbSet<Movimiento> Movimientos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //config to snack_case database sintax
        foreach( var entity in modelBuilder.Model.GetEntityTypes())
        {
            entity.SetTableName(ToSnackCase(entity.GetTableName()));

            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(ToSnackCase(property.GetFieldName()));
            }

            foreach (var key in entity.GetKeys())
            {
                key.SetName(ToSnackCase(key.GetName()));
            }

            foreach (var fk in entity.GetForeignKeys())
            {
                fk.SetConstraintName(ToSnackCase(fk.GetConstraintName()));
            }

            foreach (var index in entity.GetIndexes())
            {
                index.SetDatabaseName(ToSnackCase(index.GetDatabaseName()));
            }

        }
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
        
        modelBuilder.Entity<Usuario>()
           .HasMany(u => u.Sucursales)
           .WithMany(r => r.Usuarios)
           .UsingEntity(j => j.ToTable("SucursalUsuario"));

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

        modelBuilder.Entity<Sucursal>(e=>
        {
            e.Property(c=>c.Id).ValueGeneratedOnAdd();
            e.Property(c=>c.Nombre).IsRequired().HasMaxLength(255);
            e.Property(c=>c.Direccion).IsRequired();
            e.Property(c=>c.Telefono).IsRequired().HasMaxLength(20);
            e.Property(c=>c.Ciudad).IsRequired().HasMaxLength(50);
            
        });

        modelBuilder.Entity<Almacen>(e=>
        {
            e.Property(c=>c.Id).ValueGeneratedOnAdd();
            e.Property(c=>c.Nombre).IsRequired().HasMaxLength(100);
            e.Property(c=>c.Codigo).HasMaxLength(20);
            e.Property(c=>c.Descripcion);
            e.Property(c=>c.Direccion).IsRequired();
            e.Property(c=>c.Telefono).IsRequired().HasMaxLength(20);
            e.Property(c=>c.Ciudad).IsRequired().HasMaxLength(50);
            e.HasOne(a=>a.Sucursal)
                .WithMany(s=>s.Almacenes)
                .HasForeignKey("SucursalId");
        });

         modelBuilder.Entity<AlmacenProducto>(e=>
        {
            e.Property(c=>c.Id).ValueGeneratedOnAdd();
            e.Property(c=>c.CantidadActual).IsRequired();
            e.Property(c=>c.FechaActualizacion);
            e.Property(c=>c.PrecioVentaActual).IsRequired();
            e.Property(c=>c.Estado).IsRequired();
            e.HasOne(a=>a.Producto)
                .WithMany(s=>s.AlmacenProductos)
                .HasForeignKey("ProductoId");
            e.HasOne(a=>a.Almacen)
                .WithMany(s=>s.AlmacenProductos)
                .HasForeignKey("AlmacenId");    
        });

         modelBuilder.Entity<Nota>(e=>
        {
            e.Property(c=>c.Id).ValueGeneratedOnAdd();
            e.Property(c=>c.TipoNota).IsRequired();
            e.Property(c=>c.Impuestos);
            e.Property(c=>c.Descuento);
            e.Property(c=>c.Fecha);
            e.Property(c=>c.Estado).IsRequired();
            e.Property(c=>c.Observaciones).IsRequired();
            e.HasOne(a=>a.Usuario)
                .WithMany(s=>s.Notas)
                .HasForeignKey("UsuarioId");
            e.HasOne(a=>a.ClienteProveedor)
                .WithMany(s=>s.Notas)
                .HasForeignKey("ClienteProveedorId");    
        });

        modelBuilder.Entity<Movimiento>(e=>
        {
            e.Property(c=>c.Id).ValueGeneratedOnAdd();
            e.Property(c=>c.TipoMovimiento).IsRequired();
            e.Property(c=>c.PrecioUnitarioCompra).IsRequired();
            e.Property(c=>c.PrecioUnitarioVenta).IsRequired();
            e.HasOne(a=>a.Producto)
                .WithMany(s=>s.Movimientos)
                .HasForeignKey("ProductoId");
            e.HasOne(a=>a.Almacen)
                .WithMany(s=>s.Movimientos)
                .HasForeignKey("AlmacenId"); 
            e.HasOne(a=>a.Nota)
                .WithMany(s=>s.Movimientos)
                .HasForeignKey("NotaId");        
        });
    }

    private string ToSnackCase (string input)
    {
        if(string.IsNullOrEmpty(input)) return input;

        var startUnderscore = input.StartsWith("_");

        var res = Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();

        return startUnderscore ? $"_{res}" : res;
    }

}
