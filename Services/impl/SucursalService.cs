using System;
using System.Data;
using ComprasVentas.Dto;
using ComprasVentas.Services.Spec;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace ComprasVentas.Services.impl;

public class SucursalService(AppDbContext appDbContext) : ISucursalService
{
    private readonly AppDbContext _appDbContext = appDbContext;

    public async Task<int> CreateSucursal(CreateSucursalDto createSucursalDto)
    {
        var connection = _appDbContext.Database.GetDbConnection();
        var parameters = new DynamicParameters();
        parameters.Add("p_nombre", createSucursalDto.Nombre); 
        parameters.Add("p_direccion", createSucursalDto.Direccion); 
        parameters.Add("p_telefono", createSucursalDto.Telefono); 
        parameters.Add("p_ciudad", createSucursalDto.Ciudad); 
        parameters.Add("p_id", value: 0, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

         await connection.ExecuteAsync("CALL create_sucursal_proc(@p_nombre, @p_direccion, @p_telefono, @p_ciudad, @p_id)", parameters);

         return parameters.Get<int>("p_id");
    }

    public async Task<IEnumerable<SucursalDto>> GetAllSucursales()
    {
        var connection = _appDbContext.Database.GetDbConnection();

        const string sql = "SELECT * from get_all_sucursales();";

        var sucursales = await connection.QueryAsync<SucursalDto>(sql);

        return sucursales;
    }
}
