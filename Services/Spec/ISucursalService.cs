using System;
using ComprasVentas.Dto;

namespace ComprasVentas.Services.Spec;

public interface ISucursalService
{
    Task<IEnumerable<SucursalDto>> GetAllSucursales();

    Task<int> CreateSucursal(CreateSucursalDto createSucursalDto);
}
