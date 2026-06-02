using System;
using Microsoft.EntityFrameworkCore;

namespace ComprasVentas;

public class UniqueNameChecker(AppDbContext db) : IUniqueNameChecker
{
    
    public async Task<bool> ExistUsernameAsync(string value)
    {
        var user = await db.Usuarios.AnyAsync(u => u.Nombre == value);
        return user;
    }
}
