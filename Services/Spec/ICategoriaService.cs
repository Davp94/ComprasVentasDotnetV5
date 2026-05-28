using System;

namespace ComprasVentas;

public interface ICategoriaService
{
    Task<List<CategoriaDto>> GetAllCategorias();

    Task<CategoriaDto?> GetCategoriaById(int id);

    Task<CategoriaDto> CreateCategoria(CreateCategoriaDto categoria);

    Task UpdateCategoria(int id, CreateCategoriaDto categoria);

    Task DeleteCategoria(int id);
}
