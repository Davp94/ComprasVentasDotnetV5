using Microsoft.AspNetCore.Mvc;

namespace ComprasVentas;

[Route("api/[controller]")]
[ApiController]
public class RolController(IRolService service) : ControllerBase
{
    private readonly IRolService _service = service;

    [HttpGet]
    public async Task<ActionResult<List<RolDto>>> GetAll()
    {
        try
        {
            var roles = await _service.GetAllRoles();
            return Ok(roles);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al obtener los roles: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RolDto>> GetById(int id)
    {
        try
        {
            var rol = await _service.GetRolById(id);
            return Ok(rol);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al obtener el rol: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<ActionResult<RolDto>> CreateRol([FromBody] CreateRolDto rolDto)
    {
        try
        {
            var rol = await _service.CreateRol(rolDto);
            return Ok(rol);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al crear el rol: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateRol(int id, [FromBody] CreateRolDto rolDto)
    {
        try
        {
            await _service.UpdateRol(id, rolDto);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al actualizar el rol: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteRol(int id)
    {
        try
        {
            await _service.DeleteRol(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al eliminar el rol: {ex.Message}");
        }
    }
}
