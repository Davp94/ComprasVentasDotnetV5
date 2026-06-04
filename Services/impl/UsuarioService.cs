using System;

namespace ComprasVentas;

public class UsuarioService(UsuarioRepository usuarioRepository, ILogger<UsuarioController> logger) : IUsuarioService
{
    private readonly UsuarioRepository _usuarioRepository = usuarioRepository;

    private readonly ILogger<UsuarioController> _logger = logger;

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
            _logger.LogInformation("Data from user retrieved on get user by Id {usuario}", usuario);
            if(usuario == null)
            {
                throw new ResourceNotFoundException("Usuario", id);
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
