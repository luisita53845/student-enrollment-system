namespace RegistroEstudiantes.Api.Dtos;

public class InscripcionActualizarDto
{
    public List<int> MateriaIds { get; set; } = new();
}

public class InscripcionDto
{
    public List<MateriaDto> Materias { get; set; } = new();
    public int TotalCreditos { get; set; }
    public string Estado { get; set; } = "Pendiente";
}