using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RegistroEstudiantes.Api.Dtos;
using RegistroEstudiantes.Api.Services;

namespace RegistroEstudiantes.Api.Controllers;

[ApiController]
[Route("api/auth")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("registro")]
    public async Task<IActionResult> Registro(RegistroDto dto)
    {
        await _authService.RegistrarAsync(dto);
        return StatusCode(201, new { mensaje = "Registro exitoso. Ya puedes iniciar sesión." });
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthRespuestaDto>> Login(LoginDto dto)
    {
        var respuesta = await _authService.LoginAsync(dto);
        return Ok(respuesta);
    }
}