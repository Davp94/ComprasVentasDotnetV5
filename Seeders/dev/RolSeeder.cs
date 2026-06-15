using System;
using Microsoft.EntityFrameworkCore;

namespace ComprasVentas;

public class RolSeeder(AppDbContext appDbContext)
{
    private readonly AppDbContext _context = appDbContext;

    public async Task SeedAsync()
    {
        if(_context.Roles.Any())
            return;

        var permisos = await _context.Permisos.ToListAsync();

        var admin = new Rol
        {
          Nombre = "ADMIN",
          Descripcion = "Administrador del sistema con acceso a todo",
          Permisos = permisos
        };

        var vendedorPermisos = permisos.Where(p => p.Recurso !=null && new[] {"productos", "categorias", "notas", "movimientos", "clientes"}.Contains(p.Recurso)).ToList();

        var vendedor = new Rol
        {
          Nombre = "VENDEDOR",
          Descripcion = "Vendedor que gestiona las compras ventas realizadas",
          Permisos = vendedorPermisos
        };

        var rrhhPermisos = permisos.Where(p => p.Recurso !=null && new[] {"usuarios", "roles", "permisos"}.Contains(p.Recurso)).ToList();

        var rrhh = new Rol
        {
          Nombre = "RRHH",
          Descripcion = "Usuario de recursos humanos",
          Permisos = rrhhPermisos
        };
       
        await _context.Roles.AddRangeAsync(admin, vendedor, rrhh);
        await _context.SaveChangesAsync();
    }
}
