using System;

namespace ComprasVentas;

public class UsuarioService(UsuarioRepository usuarioRepository) : IUsuarioService
{
    private readonly UsuarioRepository _usuarioRepository = usuarioRepository;

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

    public async Task<UsuarioDto?> GetUsuarioById(int id)
    {
        try
        {
            var usuario = await _usuarioRepository.GetUsuarioById(id);
            if(usuario == null)
            {
                throw new ResourceNotFoundException($"Usuario con ID {id} no encontrado");
            }
            return MapToDto(usuario);
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }

    public Task UpdateUsuario(int id, CreateUsuarioDto categoria)
    {
        throw new NotImplementedException();
    }

    private UsuarioDto MapToDto(Usuario usuario)
    {
        return new UsuarioDto
        {
            Id = usuario.Id
        };
    }
}
