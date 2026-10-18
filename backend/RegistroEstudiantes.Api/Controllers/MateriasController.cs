using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RegistroEstudiantes.Api.Dtos;
using RegistroEstudiantes.Api.Extensions;
using RegistroEstudiantes.Api.Services;

namespace RegistroEstudiantes.Api.Controllers;

[ApiController]
[Route("api/materias")]
[Authorize]
public class MateriasController : ControllerBase
{
    private readonly MateriaService _service;

    public MateriasController(MateriaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<MateriaDto>>> Listar()
        => Ok(await _service.ListarAsync());

    [HttpGet("{id:int}/companeros")]
    public async Task<ActionResult<List<CompaneroDto>>> Companeros(int id)
        => Ok(await _service.ObtenerCompanerosAsync(id, User.ObtenerId(), User.IsInRole("Administrador")));

    [HttpGet("resumen")]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<List<MateriaResumenDto>>> Resumen()
        => Ok(await _service.ListarResumenAsync());
}