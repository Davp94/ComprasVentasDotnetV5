using System;

namespace ComprasVentas;

public class PermisoSeeder(AppDbContext appDbContext)
{
    private readonly AppDbContext _context = appDbContext;

    public async Task SeedAsync()
    {
        if(_context.Permisos.Any())
            return;

        var recursos = new[]
        {
          "usuarios", "roles", "permisos", "productos", "categorias",
          "sucursales", "almacenes", "notas", "movimientos", "clientes",
          "documentos"
        };

        var acciones = new[] {"crear", "leer", "actualizar", "eliminar"}; 
        var permisos = new List<Permiso>();
        foreach (var recurso in recursos)
        {
            foreach (var accion in acciones)
            {
                permisos.Add(new Permiso
                {
                   Nombre = $"{accion}:{recurso}",
                   Recurso = recurso,
                   Action = accion 
                });
            }
        }   
        await _context.Permisos.AddRangeAsync(permisos);
        await _context.SaveChangesAsync();
    }

}
