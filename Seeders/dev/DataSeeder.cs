using System;

namespace ComprasVentas;

public class DataSeeder
{
    private readonly PermisoSeeder _permisoSeeder;
    private readonly RolSeeder _rolSeeder;

    public DataSeeder(PermisoSeeder permisoSeeder, RolSeeder rolSeeder)
    {
        _permisoSeeder = permisoSeeder;
        _rolSeeder = rolSeeder;
    }

    public async Task SeedAsync()
    {
        await _permisoSeeder.SeedAsync();
    }
}
