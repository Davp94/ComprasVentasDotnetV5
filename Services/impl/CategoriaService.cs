using System;

namespace ComprasVentas;

public class CategoriaService(CategoriaRepository repository) : ICategoriaService
{
    private readonly CategoriaRepository _repository = repository;

    public async Task<List<CategoriaDto>> GetAllCategorias()
    {
        try
        {
            var categorias = await _repository.GetAllCategorias();
            return categorias.Select(MapToDto).ToList();
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    public async Task<CategoriaDto?> GetCategoriaById(int id)
    {
        try
        {
            var categoria = await _repository.GetCategoriaById(id);
            if(categoria == null)
            {
                throw new Exception($"Categoria con Id {id} no encontrada");
            }
            return MapToDto(categoria);
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    public async Task<CategoriaDto> CreateCategoria(CreateCategoriaDto categoria)
    {
        try
        {
            //valid unique email
            if (true)
            {
                throw new BusinessRuleException("");
            }
            var categoriaToCreate = new Categoria
            {
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion
            };
            var categoriaSaved = await _repository.CreateCategoria(categoriaToCreate); 
            return MapToDto(categoriaSaved);
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    public async Task UpdateCategoria(int id, CreateCategoriaDto categoria)
    {
        try
        {
            var categoriaRetrieved = await _repository.GetCategoriaById(id);
            if(categoriaRetrieved == null)
            {
                throw new Exception($"Categoria con Id {id} no encontrada");
            } 
            categoriaRetrieved.Nombre = categoria.Nombre;
            categoriaRetrieved.Descripcion = categoria.Descripcion;

            await _repository.UpdateCategoria(categoriaRetrieved);
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }

    public async Task DeleteCategoria(int id)
    {
        try
        {
            await _repository.DeleteAsync(id);
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }

    private static CategoriaDto MapToDto(Categoria categoria)
    {
        return new CategoriaDto
        {
            Id = categoria.Id,
            Nombre = categoria.Nombre,
            Descripcion = categoria.Descripcion
        };
    }

}
