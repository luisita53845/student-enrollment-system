using Microsoft.EntityFrameworkCore;
using RegistroEstudiantes.Api.Data;
using RegistroEstudiantes.Api.Dtos;

namespace RegistroEstudiantes.Api.Services;

public class ProfesorService
{
    private readonly AppDbContext _context;

    public ProfesorService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProfesorDto>> ListarAsync()
    {
        return await _context.Profesores
            .AsNoTracking()
            .OrderBy(p => p.Id)
            .Select(p => new ProfesorDto
            {
                Id = p.Id,
                Documento = p.Documento,
                Nombre = p.Nombre,
                Correo = p.Correo,
                Telefono = p.Telefono,
                Materias = p.Materias.OrderBy(m => m.Id).Select(m => m.Nombre).ToList()
            })
            .ToListAsync();
    }
}