using System;
using Microsoft.EntityFrameworkCore;

namespace ComprasVentas;

public class UsuarioRepository
{
    private readonly AppDbContext _context;

    public UsuarioRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Usuario>> GetAllUsuarios()
    {
        return await _context.Usuarios
        .Include(u => u.Persona)
        .Include(u => u.Roles)
        .ToListAsync();
    }

    public async Task<Usuario?> GetUsuarioById(int id)
    {
        return await _context.Usuarios
        .Include(u => u.Persona)
        .Include(u => u.Roles)
        .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<Usuario> CreateUsuario(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }

    public async Task UpdateUsuario(Usuario usuario)
    {
        _context.Usuarios.Update(usuario);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario != null)
        {
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
        }
    }
}
