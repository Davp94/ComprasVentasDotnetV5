using Microsoft.AspNetCore.Mvc;

namespace ComprasVentas;

[Route("api/[controller]")]
[ApiController]
public class PermisoController(IPermisoService service) : ControllerBase
{
    private readonly IPermisoService _service = service;

    [HttpGet]
    public async Task<ActionResult<List<PermisoDto>>> GetAll()
    {
        try
        {
            var permisos = await _service.GetAllPermisos();
            return Ok(permisos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al obtener los permisos: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PermisoDto>> GetById(int id)
    {
        try
        {
            var permiso = await _service.GetPermisoById(id);
            return Ok(permiso);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al obtener el permiso: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<ActionResult<PermisoDto>> CreatePermiso([FromBody] CreatePermisoDto permisoDto)
    {
        try
        {
            var permiso = await _service.CreatePermiso(permisoDto);
            return Ok(permiso);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al crear el permiso: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdatePermiso(int id, [FromBody] CreatePermisoDto permisoDto)
    {
        try
        {
            await _service.UpdatePermiso(id, permisoDto);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al actualizar el permiso: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePermiso(int id)
    {
        try
        {
            await _service.DeletePermiso(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al eliminar el permiso: {ex.Message}");
        }
    }
}
