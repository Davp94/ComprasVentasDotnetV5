using System;
using System.Collections.Generic;

namespace ComprasVentas;

public interface IRolService
{
    Task<List<RolDto>> GetAllRoles();
    Task<RolDto?> GetRolById(int id);
    Task<RolDto> CreateRol(CreateRolDto rol);
    Task UpdateRol(int id, CreateRolDto rol);
    Task DeleteRol(int id);
}
