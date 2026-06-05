using System;
using System.Collections.Generic;

namespace ComprasVentas;

public interface IPermisoService
{
    Task<List<PermisoDto>> GetAllPermisos();
    Task<PermisoDto?> GetPermisoById(int id);
    Task<PermisoDto> CreatePermiso(CreatePermisoDto permiso);
    Task UpdatePermiso(int id, CreatePermisoDto permiso);
    Task DeletePermiso(int id);
}
