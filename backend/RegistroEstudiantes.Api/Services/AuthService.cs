using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RegistroEstudiantes.Api.Data;
using RegistroEstudiantes.Api.Dtos;
using RegistroEstudiantes.Api.Entities;
using RegistroEstudiantes.Api.Exceptions;

namespace RegistroEstudiantes.Api.Services;

public class AuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public AuthService(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task RegistrarAsync(RegistroDto dto)
    {
        var correo = dto.Correo.Trim().ToLower();
        var documento = dto.Documento.Trim();

        if (await _context.Estudiantes.AnyAsync(e => e.Correo == correo))
            throw new ConflictoException("Ya existe un estudiante con ese correo.");

        if (await _context.Estudiantes.AnyAsync(e => e.Documento == documento))
            throw new ConflictoException("Ya existe un estudiante con ese documento.");

        var estudiante = new Estudiante
        {
            Documento = documento,
            Nombre = dto.Nombre.Trim(),
            Correo = correo,
            Telefono = string.IsNullOrWhiteSpace(dto.Telefono) ? null : dto.Telefono.Trim(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Rol = "Estudiante",  
            Activo = true,
            FechaRegistro = DateTime.Now
        };

        _context.Estudiantes.Add(estudiante);
        await _context.SaveChangesAsync();
    }

    public async Task<AuthRespuestaDto> LoginAsync(LoginDto dto)
    {
        var correo = dto.Correo.Trim().ToLower();
        var estudiante = await _context.Estudiantes.FirstOrDefaultAsync(e => e.Correo == correo);

        if (estudiante == null || !BCrypt.Net.BCrypt.Verify(dto.Password, estudiante.PasswordHash))
            throw new NoAutenticadoException("Correo o contraseña incorrectos.");

        if (!estudiante.Activo)
            throw new ProhibidoException("Tu cuenta está inactiva. Contacta al administrador.");

        return GenerarToken(estudiante);
    }

    private AuthRespuestaDto GenerarToken(Estudiante estudiante)
    {
        var key = _config["Jwt:Key"]!;
        var expira = DateTime.UtcNow.AddMinutes(int.Parse(_config["Jwt:ExpiresMinutes"]!));

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, estudiante.Id.ToString()),
            new Claim(ClaimTypes.Email, estudiante.Correo),
            new Claim(ClaimTypes.Role, estudiante.Rol)
        };

        var credenciales = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: expira,
            signingCredentials: credenciales);

        return new AuthRespuestaDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Nombre = estudiante.Nombre,
            Rol = estudiante.Rol,
            Expira = expira
        };
    }
}