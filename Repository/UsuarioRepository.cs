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
        return await _context.Usuarios.ToListAsync();
    }

    public async Task<Usuario?> GetUsuarioById(int id)
    {
        return await _context.Usuarios.FindAsync(id);
    }

    public async Task<Usuario> CreateUsuario(Usuario categoria)
    {
        _context.Usuarios.Add(categoria);
        await _context.SaveChangesAsync();
        return categoria;
    }

    public async Task UpdateUsuario(Usuario categoria)
    {
        _context.Usuarios.Update(categoria);
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
