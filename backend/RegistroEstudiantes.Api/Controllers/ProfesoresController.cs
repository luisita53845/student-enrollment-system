using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RegistroEstudiantes.Api.Dtos;
using RegistroEstudiantes.Api.Services;

namespace RegistroEstudiantes.Api.Controllers;

[ApiController]
[Route("api/profesores")]
[Authorize]
public class ProfesoresController : ControllerBase
{
    private readonly ProfesorService _service;

    public ProfesoresController(ProfesorService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProfesorDto>>> Listar()
        => Ok(await _service.ListarAsync());
}