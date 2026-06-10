using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComprasVentas
{
    [Authorize]
    [Route("api/[controller]")] //  -- route -> api/categoria
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _service;

        public CategoriaController(ICategoriaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoriaDto>>> GetAll()   // /api/categoria
        {
            try
            {
                var categorias = await _service.GetAllCategorias();
                return Ok(categorias);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Error al obtener las categorías: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaDto>> GetById(int id) // /api/categoria/5
        {
            try
            {
                var categoria = await _service.GetCategoriaById(id);
                return Ok(categoria);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Error al obtener las categorías: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<CategoriaDto>> CreateCategoria([FromBody] CreateCategoriaDto categoriaDto) // /api/categoria
        {
            try
            {
                var categoria = await _service.CreateCategoria(categoriaDto);
                return Ok(categoria);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Error al crear la categoría: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCategoria(int id, [FromBody] CreateCategoriaDto categoriaDto) // /api/categoria
        {
            try
            {
                await _service.UpdateCategoria(id, categoriaDto);
                return NoContent();
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Error al actualizar las categorías: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategoria(int id) // /api/categoria
        {
            try
            {
                await _service.DeleteCategoria(id);
                return NoContent();
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Error al eliminar la categoria: {ex.Message}");
            }
        }
    }
}
