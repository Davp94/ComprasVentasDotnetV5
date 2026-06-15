using System;
using Bogus;
using Microsoft.EntityFrameworkCore;

namespace ComprasVentas;

public class UsuarioSeeder(AppDbContext appDbContext)
{
    private readonly AppDbContext _context = appDbContext;

    public async Task SeedAsync()
    {
        if(_context.Usuarios.Any())
            return;

        var roles = await _context.Roles.ToListAsync();
        var rolAdmin = roles.First(r=> r.Nombre == "ADMIN");
        var rolVendedor = roles.First(r=> r.Nombre == "VENDEDOR");
        var rolRrhh = roles.First(r=> r.Nombre == "RRHH");
        var dateT = DateTime.UtcNow.AddYears(-18).ToUniversalTime();
        var fakerPersona = new Faker<Persona>("es")
            .RuleFor(p=>p.Nombres,    f => f.Name.FirstName())
            .RuleFor(p=>p.Apellidos,    f => f.Name.LastName())
            // .RuleFor(p=>p.FechaNacimiento,    f => f.Date.Past(20, DateTime.UtcNow.AddYears(-18)).ToUniversalTime())
            .RuleFor(p=>p.Genero,    f => f.PickRandom("Masculino", "Femenino", "otro"))
            .RuleFor(p=>p.Telefono,    f => f.Phone.PhoneNumber("########"))
            .RuleFor(p=>p.Nacionalidad,    f => f.Address.Country())
            .RuleFor(p=>p.Direccion,    f => f.Address.StreetAddress());
        
        var usuarios = new List<Usuario>();

        for(int i = 0; i<10; i++)
        {
            var persona = fakerPersona.Generate();

            var faker = new Faker("es");
            var nombres = faker.Internet.UserName(persona.Nombres, persona.Apellidos);
            var correo = faker.Internet.Email(persona.Nombres, persona.Apellidos);

            var usuario = new Usuario
            {
                Nombre = nombres,
                Email = correo,
                Password = BCrypt.Net.BCrypt.HashPassword("Password123*"),
                Persona = persona,
                Roles = new List<Rol> {rolAdmin}
            };
            usuarios.Add(usuario);
        }
            

        await _context.Usuarios.AddRangeAsync(usuarios);
        await _context.SaveChangesAsync();
    }
}
