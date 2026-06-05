using System;
using Microsoft.EntityFrameworkCore;

namespace ComprasVentas;

public class PermisoRepository
{
    private readonly AppDbContext _context;

    public PermisoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async  Task<List<Permiso>> GetAllPermisos()
    {
        return await _context.Permisos.ToListAsync();
    }

    public async Task<Permiso?> GetPermisoById(int id)
    {
        return await _context.Permisos.FirstOrDefaultAsync(r=>r.Id == id);
    }

    public async Task<Permiso> CreatePermiso(Permiso permiso)
    {
        _context.Permisos.Add(permiso);
        await _context.SaveChangesAsync();
        return permiso;
    }

    public async Task UpdatePermiso(Permiso permiso)
    {
        _context.Permisos.Update(permiso);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var permiso = await _context.Permisos.FindAsync(id);
        if (permiso != null)
        {
            _context.Permisos.Remove(permiso);
            await _context.SaveChangesAsync();
        }
    }
}
