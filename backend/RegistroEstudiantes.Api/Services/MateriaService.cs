using Microsoft.EntityFrameworkCore;
using RegistroEstudiantes.Api.Data;
using RegistroEstudiantes.Api.Dtos;
using RegistroEstudiantes.Api.Exceptions;

namespace RegistroEstudiantes.Api.Services;

public class MateriaService
{
    private readonly AppDbContext _context;

    public MateriaService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<MateriaDto>> ListarAsync()
    {
        return await _context.Materias
            .AsNoTracking()
            .OrderBy(m => m.Id)
            .Select(m => new MateriaDto
            {
                Id = m.Id,
                Nombre = m.Nombre,
                Creditos = m.Creditos,
                ProfesorId = m.ProfesorId,
                NombreProfesor = m.Profesor.Nombre
            })
            .ToListAsync();
    }

    public async Task<List<CompaneroDto>> ObtenerCompanerosAsync(int materiaId, int usuarioId, bool esAdmin)
    {
        var existe = await _context.Materias.AnyAsync(m => m.Id == materiaId);
        if (!existe)
            throw new NoEncontradoException("La materia no existe.");

        if (!esAdmin)
        {
            var estaInscrito = await _context.Inscripciones
                .AnyAsync(i => i.MateriaId == materiaId && i.EstudianteId == usuarioId);

            if (!estaInscrito)
                throw new ProhibidoException("Solo puedes ver los compañeros de materias en las que estás inscrito.");
        }

        return await _context.Inscripciones
            .AsNoTracking()
            .Where(i => i.MateriaId == materiaId
                        && i.EstudianteId != usuarioId
                        && i.Estudiante.Activo)
            .OrderBy(i => i.Estudiante.Nombre)
            .Select(i => new CompaneroDto { Nombre = i.Estudiante.Nombre })
            .ToListAsync();
    }
}