using Microsoft.EntityFrameworkCore;
using RegistroEstudiantes.Api.Data;
using RegistroEstudiantes.Api.Dtos;
using RegistroEstudiantes.Api.Entities;
using RegistroEstudiantes.Api.Exceptions;

namespace RegistroEstudiantes.Api.Services;

public class InscripcionService
{
    private const int MateriasRequeridas = 3;
    private readonly AppDbContext _context;

    public InscripcionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<InscripcionDto> ObtenerAsync(int estudianteId)
    {
        var materias = await _context.Inscripciones
            .AsNoTracking()
            .Where(i => i.EstudianteId == estudianteId)
            .OrderBy(i => i.MateriaId)
            .Select(i => new MateriaDto
            {
                Id = i.Materia.Id,
                Nombre = i.Materia.Nombre,
                Creditos = i.Materia.Creditos,
                ProfesorId = i.Materia.ProfesorId,
                NombreProfesor = i.Materia.Profesor.Nombre
            })
            .ToListAsync();

        return new InscripcionDto
        {
            Materias = materias,
            TotalCreditos = materias.Sum(m => m.Creditos),
            Estado = materias.Count == MateriasRequeridas ? "Completa" : "Pendiente"
        };
    }

    public async Task<InscripcionDto> DefinirAsync(int estudianteId, InscripcionActualizarDto dto)
    {
        var ids = dto.MateriaIds;

        if (ids.Count != MateriasRequeridas)
            throw new ReglaNegocioException($"Debes seleccionar exactamente {MateriasRequeridas} materias.");

        if (ids.Distinct().Count() != ids.Count)
            throw new ReglaNegocioException("No puedes repetir una materia.");

        var materias = await _context.Materias
            .Where(m => ids.Contains(m.Id))
            .ToListAsync();

        if (materias.Count != ids.Count)
            throw new NoEncontradoException("Alguna de las materias seleccionadas no existe.");

        if (materias.Select(m => m.ProfesorId).Distinct().Count() != materias.Count)
            throw new ReglaNegocioException("No se puede repetir profesor: las 3 materias deben ser de profesores diferentes.");

        // Reemplazo en una sola operación
        var anteriores = _context.Inscripciones.Where(i => i.EstudianteId == estudianteId);
        _context.Inscripciones.RemoveRange(anteriores);

        foreach (var materia in materias)
        {
            _context.Inscripciones.Add(new Inscripcion
            {
                EstudianteId = estudianteId,
                MateriaId = materia.Id,
                FechaInscripcion = DateTime.Now
            });
        }

        await _context.SaveChangesAsync();
        return await ObtenerAsync(estudianteId);
    }
}