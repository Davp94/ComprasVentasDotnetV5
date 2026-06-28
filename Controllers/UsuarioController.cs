using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComprasVentas
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController(IUsuarioService usuarioService, ILogger<UsuarioController> logger) : ControllerBase
    {
        private readonly IUsuarioService _service = usuarioService;
        private readonly ILogger<UsuarioController> _logger = logger;

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDto>> GetById(int id) // /api/usuario/5   
        {
            try
            {
                var usuario = await _service.GetUsuarioById(id);
                return Ok(usuario);
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<UsuarioDto>> CreateUsuario([FromForm] CreateUsuarioDto createUsuarioDto) 
        {
            try
            {
                _logger.LogInformation("Data received to create product {@createUsuarioDto}", createUsuarioDto);
                var usuario = await _service.CreateUsuario(createUsuarioDto);
                return Ok(usuario);
            }
            catch (System.Exception ex)
            {
                _logger.LogError("Error al crear el producto {ex}", ex);
                throw;
            }
        }
    }
}
