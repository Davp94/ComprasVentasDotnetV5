using System;
using Microsoft.EntityFrameworkCore;

namespace ComprasVentas;

public class CategoriaRepository
{
    private readonly AppDbContext _context;

    public CategoriaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async  Task<List<Categoria>> GetAllCategorias()
    {
        return await _context.Categorias.ToListAsync();
    }

    public async Task<Categoria?> GetCategoriaById(int id)
    {
        return await _context.Categorias.FindAsync(id);
    }

    public async Task<Categoria> CreateCategoria(Categoria categoria)
    {
        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();
        return categoria;
    }

    public async Task UpdateCategoria(Categoria categoria)
    {
        _context.Categorias.Update(categoria);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var categoria = await _context.Categorias.FindAsync(id);
        if (categoria != null)
        {
            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();
        }
    }
}
