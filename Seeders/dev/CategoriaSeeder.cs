using System;
using System.Linq;
using System.Collections.Generic;

namespace ComprasVentas;

public class CategoriaSeeder(AppDbContext appDbContext)
{
    private readonly AppDbContext _context = appDbContext;

    public async Task SeedAsync()
    {
        if (_context.Categorias.Any())
            return;

        var nombres = new[]
        {
            "Bebidas","Abarrotes","Lácteos","Carnes","Limpieza","Higiene","Electrónica","Ropa","Ferretería"
        };

        var categorias = nombres.Select(n => new Categoria { Nombre = n }).ToList();
        await _context.Categorias.AddRangeAsync(categorias);
        await _context.SaveChangesAsync();
    }
}
