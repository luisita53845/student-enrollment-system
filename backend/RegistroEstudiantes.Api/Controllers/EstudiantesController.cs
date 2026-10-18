using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RegistroEstudiantes.Api.Dtos;
using RegistroEstudiantes.Api.Extensions;
using RegistroEstudiantes.Api.Services;

namespace RegistroEstudiantes.Api.Controllers;

[ApiController]
[Route("api/estudiantes")]
[Authorize]
public class EstudiantesController : ControllerBase
{
    private readonly EstudianteService _service;

    public EstudiantesController(EstudianteService service)
    {
        _service = service;
    }

    // Mi perfil (estudiante o admin)

    [HttpGet("me")]
    public async Task<ActionResult<EstudiantePerfilDto>> ObtenerMiPerfil()
        => Ok(await _service.ObtenerMiPerfilAsync(User.ObtenerId()));

    [HttpPut("me")]
    public async Task<ActionResult<EstudiantePerfilDto>> ActualizarMiPerfil(EstudianteActualizarDto dto)
        => Ok(await _service.ActualizarMiPerfilAsync(User.ObtenerId(), dto));

    // Administrador

    [HttpGet]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<List<EstudiantePerfilDto>>> Listar()
        => Ok(await _service.ListarAsync());

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<EstudiantePerfilDto>> ObtenerPorId(int id)
        => Ok(await _service.ObtenerPorIdAsync(id));

    [HttpPut("{id:int}/inactivar")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Inactivar(int id)
    {
        await _service.InactivarAsync(id, User.ObtenerId());
        return NoContent();
    }

    [HttpPut("{id:int}/reactivar")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Reactivar(int id)
    {
        await _service.ReactivarAsync(id);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Eliminar(int id)
    {
        await _service.EliminarAsync(id, User.ObtenerId());
        return NoContent();
    }
}