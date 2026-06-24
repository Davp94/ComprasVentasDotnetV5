using System;

namespace ComprasVentas;

public class UsuarioService(UsuarioRepository usuarioRepository, RolRepository rolRepository,ILogger<UsuarioController> logger, IFileService fileService) : IUsuarioService
{
    private readonly UsuarioRepository _usuarioRepository = usuarioRepository;

    private readonly RolRepository _rolRepository = rolRepository;

    private readonly ILogger<UsuarioController> _logger = logger;

    private readonly IFileService _fileService = fileService;

     public async Task<List<UsuarioDto>> GetAllUsuarios()
    {
        try
        {
            var usuarios = await _usuarioRepository.GetAllUsuarios();
            return usuarios.Select(u=>MapToDto(u)).ToList();
        }
        catch (System.Exception){
        
            throw;
        }
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

    public async Task<UsuarioDto> CreateUsuario(CreateUsuarioDto usuario)
    {
        try
        {
            var roles = new List<Rol>();
            if(usuario.RolIds != null && usuario.RolIds.Count>0)
            {
                foreach (var rolId in usuario.RolIds)
                {
                    var rol = await _rolRepository.GetRolById(rolId);
                    if( rol!= null) roles.Add(rol); 
                }
            }
            var documentos = new List<Documento>();
            foreach (var d in usuario.Documentos)
            {
                documentos.Add(new Documento
                {
                    Nombre = d.Nombre,
                    DescripcionDoc = d.DescripcionDoc,
                    Referencia = await _fileService.SaveFileAsync(d.Referencia)
                });
            }
            var usuarioToCreate = new Usuario
            {
                Nombre = usuario.Username,
                Email = usuario.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(usuario.Password),
                Persona = new Persona
                {
                    Nombres = usuario.Nombres,
                    Apellidos = usuario.Apellidos,
                    FechaNacimiento = DateOnly.ParseExact(usuario.FechaNacimiento, "dd/MM/yyyy"),
                    Genero = usuario.Genero,
                    Telefono = usuario.Telefono,
                    Direccion = usuario.Direccion,
                    Nacionalidad = usuario.Nacionalidad,
                    Documentos = documentos
                },
                Roles = roles,
            
            };
            var usuarioCreated = await _usuarioRepository.CreateUsuario(usuarioToCreate);
            return MapToDto(usuarioCreated);
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }

    public Task UpdateUsuario(int id, CreateUsuarioDto usuario)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteUsuario(int id)
    {
        try
        {
            await _usuarioRepository.DeleteAsync(id);
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }


    private UsuarioDto MapToDto(Usuario usuario)
    {
        return new UsuarioDto
        {
            Id = usuario.Id,
            Username = usuario.Nombre,
            Email = usuario.Email,
            Nombres = usuario.Persona?.Nombres ?? string.Empty,
            Apellidos = usuario.Persona?.Apellidos ?? string.Empty,
            FechaNacimiento = usuario.Persona?.FechaNacimiento.ToString("yyyy-MM-dd"),
            Genero = usuario.Persona?.Genero,
            Telefono = usuario.Persona?.Telefono,
            Direccion = usuario.Persona?.Direccion,
            Nacionalidad = usuario.Persona?.Nacionalidad,
            RolIds = usuario.Roles?.Select(r => r.Id).ToList() ?? []
        };
    }
}
