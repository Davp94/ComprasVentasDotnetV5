using System;
using System.Linq;
using System.Collections.Generic;
using Bogus;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace ComprasVentas;

public class AlmacenProductoSeeder(AppDbContext appDbContext)
{
    private readonly AppDbContext _context = appDbContext;
    private const int RecordsPerAlmacen = 50;

    public async Task SeedAsync()
    {
        if (_context.AlmacenProductos.Any())
            return;

        var almacenes = await _context.Almacenes.ToListAsync();
        if (!almacenes.Any())
        {
            var almacenSeeder = new AlmacenSeeder(_context);
            await almacenSeeder.SeedAsync();
            almacenes = await _context.Almacenes.ToListAsync();
        }

        var productos = await _context.Productos.ToListAsync();
        if (productos.Count < RecordsPerAlmacen)
        {
            var productoSeeder = new ProductoSeeder(_context);
            await productoSeeder.SeedAsync();
            productos = await _context.Productos.ToListAsync();
        }

        var faker = new Faker("es");
        var almacenProductos = new List<AlmacenProducto>();

        foreach (var almacen in almacenes)
        {
            var selectedProductos = faker.Random.ListItems(productos, RecordsPerAlmacen);

            foreach (var producto in selectedProductos)
            {
                almacenProductos.Add(new AlmacenProducto
                {
                    CantidadActual = faker.Random.Int(0, 200),
                    FechaActualizacion = faker.Date.Recent(30),
                    PrecioVentaActual = decimal.Parse(
                        faker.Commerce.Price(1, 500, 2), CultureInfo.InvariantCulture),
                    Estado = true,
                    Producto = producto,
                    Almacen = almacen
                });
            }
        }

        await _context.AlmacenProductos.AddRangeAsync(almacenProductos);
        await _context.SaveChangesAsync();
    }
}
