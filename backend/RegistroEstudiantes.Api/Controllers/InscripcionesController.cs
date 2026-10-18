using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RegistroEstudiantes.Api.Dtos;
using RegistroEstudiantes.Api.Extensions;
using RegistroEstudiantes.Api.Services;

namespace RegistroEstudiantes.Api.Controllers;

[ApiController]
[Route("api/inscripciones")]
[Authorize(Roles = "Estudiante")]
public class InscripcionesController : ControllerBase
{
    private readonly InscripcionService _service;

    public InscripcionesController(InscripcionService service)
    {
        _service = service;
    }

    [HttpGet("me")]
    public async Task<ActionResult<InscripcionDto>> ObtenerMia()
        => Ok(await _service.ObtenerAsync(User.ObtenerId()));

    [HttpPut("me")]
    public async Task<ActionResult<InscripcionDto>> DefinirMia(InscripcionActualizarDto dto)
        => Ok(await _service.DefinirAsync(User.ObtenerId(), dto));
}