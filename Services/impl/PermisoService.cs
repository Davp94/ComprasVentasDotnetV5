using System;
using System.Collections.Generic;
using System.Linq;

namespace ComprasVentas;

public class PermisoService(PermisoRepository repository) : IPermisoService
{
    private readonly PermisoRepository _repository = repository;

    public async Task<List<PermisoDto>> GetAllPermisos()
    {
        try
        {
            var permisos = await _repository.GetAllPermisos();
            return permisos.Select(MapToDto).ToList();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<PermisoDto?> GetPermisoById(int id)
    {
        try
        {
            var permiso = await _repository.GetPermisoById(id);
            if (permiso == null)
            {
                throw new Exception($"Permiso con Id {id} no encontrado");
            }
            return MapToDto(permiso);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<PermisoDto> CreatePermiso(CreatePermisoDto permiso)
    {
        try
        {
            var permisoToCreate = new Permiso
            {
                Nombre = permiso.Nombre,
                Recurso = permiso.Recurso,
                Action = permiso.Action
            };

            var permisoSaved = await _repository.CreatePermiso(permisoToCreate);
            return MapToDto(permisoSaved);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task UpdatePermiso(int id, CreatePermisoDto permiso)
    {
        try
        {
            var permisoRetrieved = await _repository.GetPermisoById(id);
            if (permisoRetrieved == null)
            {
                throw new Exception($"Permiso con Id {id} no encontrado");
            }

            permisoRetrieved.Nombre = permiso.Nombre;
            permisoRetrieved.Recurso = permiso.Recurso;
            permisoRetrieved.Action = permiso.Action;

            await _repository.UpdatePermiso(permisoRetrieved);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task DeletePermiso(int id)
    {
        try
        {
            await _repository.DeleteAsync(id);
        }
        catch (Exception)
        {
            throw;
        }
    }

    private static PermisoDto MapToDto(Permiso permiso)
    {
        return new PermisoDto
        {
            Id = permiso.Id,
            Nombre = permiso.Nombre,
            Recurso = permiso.Recurso,
            Action = permiso.Action,
    
        };
    }
}
