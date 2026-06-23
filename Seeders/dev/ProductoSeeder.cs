using System;
using System.Linq;
using System.Collections.Generic;
using Bogus;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace ComprasVentas;

public class ProductoSeeder(AppDbContext appDbContext)
{
    private readonly AppDbContext _context = appDbContext;

    public async Task SeedAsync()
    {
        if (_context.Productos.Any())
            return;

        var categorias = await _context.Categorias.ToListAsync();
        if (!categorias.Any())
        {
            var catSeeder = new CategoriaSeeder(_context);
            await catSeeder.SeedAsync();
            categorias = await _context.Categorias.ToListAsync();
        }

        var faker = new Faker("es");
        var productos = new List<Producto>();
        for (int i = 0; i < 50; i++)
        {
            var categoria = faker.PickRandom(categorias);
            productos.Add(new Producto
            {
                Nombre = faker.Commerce.ProductName(),
                Descripcion = faker.Commerce.ProductDescription(),
                Marca = faker.Company.CompanyName(),
                UnidadMedida = faker.Random.ArrayElement(new[] { "unidad", "kg", "lt", "pack" }),
                PrecioVentaActual = decimal.Parse(faker.Commerce.Price(1, 100, 2), CultureInfo.InvariantCulture),
                Estado = true,
                Categoria = categoria
            });
        }

        await _context.Productos.AddRangeAsync(productos);
        await _context.SaveChangesAsync();
    }
}
