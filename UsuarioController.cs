using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComprasVentas
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController(IUsuarioService usuarioService) : ControllerBase
    {
        private readonly IUsuarioService _service = usuarioService;

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
                return StatusCode(500, $"Error al obtener el usuario: {ex.Message}");
            }
        }
    }
}
