using System;
using System.Linq;
using System.Collections.Generic;
using Bogus;
using Microsoft.EntityFrameworkCore;

namespace ComprasVentas;

public class ClienteProveedorSeeder(AppDbContext appDbContext)
{
    private readonly AppDbContext _context = appDbContext;

    public async Task SeedAsync()
    {
        if (_context.ClienteProveedor.Any())
            return;

        var faker = new Faker("es");
        var list = new List<ClienteProveedor>();
        for (int i = 0; i < 50; i++)
        {
            var tipo = faker.PickRandom(new[] { "CLIENTE", "PROVEEDOR" });
            list.Add(new ClienteProveedor
            {
                Tipo = tipo,
                RazonSocial = faker.Company.CompanyName(),
                NroIdentificacion = faker.Random.Replace("##########"),
                Telefono = faker.Phone.PhoneNumber("########"),
                Correo = faker.Internet.Email(),
                Estado = true
            });
        }

        await _context.ClienteProveedor.AddRangeAsync(list);
        await _context.SaveChangesAsync();
    }
}
