using System;

namespace ComprasVentas;

public class UsuarioService : IUsuarioService
{
    public Task<UsuarioDto> CreateUsuario(CreateUsuarioDto categoria)
    {
        throw new NotImplementedException();
    }

    public Task DeleteUsuario(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<UsuarioDto>> GetAllUsuarios()
    {
        throw new NotImplementedException();
    }

    public Task<UsuarioDto?> GetUsuarioById(int id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateUsuario(int id, CreateUsuarioDto categoria)
    {
        throw new NotImplementedException();
    }
}
