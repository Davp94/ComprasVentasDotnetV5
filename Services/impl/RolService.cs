using System;
using System.Collections.Generic;
using System.Linq;

namespace ComprasVentas;

public class RolService(RolRepository repository, PermisoRepository permisoRepository) : IRolService
{
    private readonly RolRepository _repository = repository;

    private readonly PermisoRepository _Permisorepository = permisoRepository;

    public async Task<List<RolDto>> GetAllRoles()
    {
        try
        {
            var roles = await _repository.GetAllRoles();
            return roles.Select(MapToDto).ToList();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<RolDto?> GetRolById(int id)
    {
        try
        {
            var rol = await _repository.GetRolById(id);
            if (rol == null)
            {
                throw new Exception($"Rol con Id {id} no encontrado");
            }
            return MapToDto(rol);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<RolDto> CreateRol(CreateRolDto rol)
    {
        try
        {
            var permisos = new List<Permiso>();
            foreach (var permisoId in rol.PermisoIds)
            {
                var permiso = await _Permisorepository.GetPermisoById(permisoId);
                if(permiso != null)
                {
                    permisos.Add(permiso);
                }
            }
            var rolToCreate = new Rol
            {
                Nombre = rol.Nombre,
                Descripcion = rol.Descripcion,
                Permisos = permisos
            };

            var rolSaved = await _repository.CreateRol(rolToCreate);
            return MapToDto(rolSaved);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task UpdateRol(int id, CreateRolDto rol)
    {
        try
        {
            var rolRetrieved = await _repository.GetRolById(id);
            if (rolRetrieved == null)
            {
                throw new Exception($"Rol con Id {id} no encontrado");
            }
            rolRetrieved.Permisos.Clear();
             foreach (var permisoId in rol.PermisoIds)
            {
                var permiso = await _Permisorepository.GetPermisoById(permisoId);
                if(permiso != null)
                {
                    rolRetrieved.Permisos.Add(permiso);
                }
            }

            rolRetrieved.Nombre = rol.Nombre;
            rolRetrieved.Descripcion = rol.Descripcion;

            await _repository.UpdateRol(rolRetrieved);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task DeleteRol(int id)
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

    private static RolDto MapToDto(Rol rol)
    {
        return new RolDto
        {
            Id = rol.Id,
            Nombre = rol.Nombre,
            Descripcion = rol.Descripcion,
            PermisoIds = rol.Permisos?.Select(p => p.Id).ToList() ?? new List<int>()
        };
    }
}
