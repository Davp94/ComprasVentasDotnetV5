using System;

namespace ComprasVentas;

public class DataSeeder
{
    private readonly PermisoSeeder _permisoSeeder;
    private readonly RolSeeder _rolSeeder;

    private readonly UsuarioSeeder _usuarioSeeder;

    public DataSeeder(PermisoSeeder permisoSeeder, RolSeeder rolSeeder, UsuarioSeeder usuarioSeeder)
    {
        _permisoSeeder = permisoSeeder;
        _rolSeeder = rolSeeder;
        _usuarioSeeder = usuarioSeeder;
    }

    public async Task SeedAsync()
    {
        await _permisoSeeder.SeedAsync();
        await _rolSeeder.SeedAsync();
        await _usuarioSeeder.SeedAsync();
    }
}
