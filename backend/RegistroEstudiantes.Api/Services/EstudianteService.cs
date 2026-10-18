using Microsoft.EntityFrameworkCore;
using RegistroEstudiantes.Api.Data;
using RegistroEstudiantes.Api.Dtos;
using RegistroEstudiantes.Api.Entities;
using RegistroEstudiantes.Api.Exceptions;

namespace RegistroEstudiantes.Api.Services;

public class EstudianteService
{
    private readonly AppDbContext _context;

    public EstudianteService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<EstudiantePerfilDto> ObtenerMiPerfilAsync(int id)
    {
        var estudiante = await BuscarAsync(id);
        return ADto(estudiante);
    }

    public async Task<EstudiantePerfilDto> ActualizarMiPerfilAsync(int id, EstudianteActualizarDto dto)
    {
        var estudiante = await BuscarAsync(id);

        estudiante.Nombre = dto.Nombre.Trim();
        estudiante.Telefono = string.IsNullOrWhiteSpace(dto.Telefono) ? null : dto.Telefono.Trim();

        await _context.SaveChangesAsync();
        return ADto(estudiante);
    }

    public async Task<List<EstudiantePerfilDto>> ListarAsync()
    {
        var estudiantes = await _context.Estudiantes
            .AsNoTracking()
            .OrderBy(e => e.Nombre)
            .ToListAsync();

        return estudiantes.Select(ADto).ToList();
    }

    public async Task<EstudiantePerfilDto> ObtenerPorIdAsync(int id)
    {
        var estudiante = await BuscarAsync(id);
        return ADto(estudiante);
    }

    public async Task InactivarAsync(int id, int adminId)
    {
        if (id == adminId)
            throw new ReglaNegocioException("Un administrador no puede inactivarse a sí mismo.");

        var estudiante = await BuscarAsync(id);

        var inscripciones = _context.Inscripciones.Where(i => i.EstudianteId == id);
        _context.Inscripciones.RemoveRange(inscripciones);

        estudiante.Activo = false;
        await _context.SaveChangesAsync();
    }

    public async Task ReactivarAsync(int id)
    {
        var estudiante = await BuscarAsync(id);
        estudiante.Activo = true;
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(int id, int adminId)
    {
        if (id == adminId)
            throw new ReglaNegocioException("Un administrador no puede eliminarse a sí mismo.");

        var estudiante = await BuscarAsync(id);

        _context.Estudiantes.Remove(estudiante);
        await _context.SaveChangesAsync(); 
    }


    private async Task<Estudiante> BuscarAsync(int id)
    {
        return await _context.Estudiantes.FirstOrDefaultAsync(e => e.Id == id)
            ?? throw new NoEncontradoException("El estudiante no existe.");
    }

    private static EstudiantePerfilDto ADto(Estudiante e) => new()
    {
        Id = e.Id,
        Documento = e.Documento,
        Nombre = e.Nombre,
        Correo = e.Correo,
        Telefono = e.Telefono,
        Rol = e.Rol,
        Activo = e.Activo,
        FechaRegistro = e.FechaRegistro
    };
}