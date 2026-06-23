using System;
using System.Linq;
using System.Collections.Generic;
using Bogus;
using Microsoft.EntityFrameworkCore;

namespace ComprasVentas;

public class SucursalSeeder(AppDbContext appDbContext)
{
    private readonly AppDbContext _context = appDbContext;

    public async Task SeedAsync()
    {
        if (_context.Sucursales.Any())
            return;

        var faker = new Faker("es");
        var sucursales = new List<Sucursal>();
        for (int i = 0; i < 5; i++)
        {
            sucursales.Add(new Sucursal
            {
                Nombre = faker.Company.CompanyName(),
                Direccion = faker.Address.StreetAddress(),
                Telefono = faker.Phone.PhoneNumber("########"),
                Ciudad = faker.Address.City()
            });
        }

        await _context.Sucursales.AddRangeAsync(sucursales);
        await _context.SaveChangesAsync();
    }
}
