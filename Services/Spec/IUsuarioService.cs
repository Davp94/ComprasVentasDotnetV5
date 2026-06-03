using System;

namespace ComprasVentas;

public interface IUsuarioService
{
    Task<List<UsuarioDto>> GetAllUsuarios();

    Task<UsuarioDto?> GetUsuarioById(int id);

    Task<UsuarioDto> CreateUsuario(CreateUsuarioDto categoria);

    Task UpdateUsuario(int id, CreateUsuarioDto categoria);

    Task DeleteUsuario(int id);
}
