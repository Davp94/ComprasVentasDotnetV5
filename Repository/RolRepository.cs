using System;
using Microsoft.EntityFrameworkCore;

namespace ComprasVentas;

public class RolRepository
{
    private readonly AppDbContext _context;

    public RolRepository(AppDbContext context)
    {
        _context = context;
    }

    public async  Task<List<Rol>> GetAllRoles()
    {
        return await _context.Roles.ToListAsync();
    }

    public async Task<Rol?> GetRolById(int id)
    {
        return await _context.Roles.Include(r=>r.Permisos).FirstOrDefaultAsync(r=>r.Id == id);
    }

    public async Task<Rol> CreateRol(Rol categoria)
    {
        _context.Roles.Add(categoria);
        await _context.SaveChangesAsync();
        return categoria;
    }

    public async Task UpdateRol(Rol categoria)
    {
        _context.Roles.Update(categoria);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var categoria = await _context.Roles.FindAsync(id);
        if (categoria != null)
        {
            _context.Roles.Remove(categoria);
            await _context.SaveChangesAsync();
        }
    }
}
