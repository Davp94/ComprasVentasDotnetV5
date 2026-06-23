using System;
using System.Linq;
using System.Collections.Generic;
using Bogus;
using Microsoft.EntityFrameworkCore;

namespace ComprasVentas;

public class AlmacenSeeder(AppDbContext appDbContext)
{
    private readonly AppDbContext _context = appDbContext;

    public async Task SeedAsync()
    {
        if (_context.Almacenes.Any())
            return;

        var sucursales = await _context.Sucursales.ToListAsync();
        if (!sucursales.Any())
        {
            var sucSeeder = new SucursalSeeder(_context);
            await sucSeeder.SeedAsync();
            sucursales = await _context.Sucursales.ToListAsync();
        }

        var faker = new Faker("es");
        var almacenes = new List<Almacen>();
        foreach (var suc in sucursales)
        {
            var count = faker.Random.Int(1, 2);
            for (int i = 0; i < count; i++)
            {
                almacenes.Add(new Almacen
                {
                    Nombre = $"{suc.Nombre} Almacén {i + 1}",
                    Codigo = faker.Random.AlphaNumeric(6).ToUpper(),
                    Descripcion = faker.Commerce.Product(),
                    Direccion = faker.Address.StreetAddress(),
                    Telefono = faker.Phone.PhoneNumber("########"),
                    Ciudad = suc.Ciudad,
                    Sucursal = suc
                });
            }
        }

        await _context.Almacenes.AddRangeAsync(almacenes);
        await _context.SaveChangesAsync();
    }
}
