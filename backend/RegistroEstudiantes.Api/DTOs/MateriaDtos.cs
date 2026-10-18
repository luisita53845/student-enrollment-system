namespace RegistroEstudiantes.Api.Dtos;

public class MateriaDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int Creditos { get; set; }
    public int ProfesorId { get; set; }
    public string NombreProfesor { get; set; } = string.Empty;
}

public class CompaneroDto
{
    public string Nombre { get; set; } = string.Empty;
}

public class MateriaResumenDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int Creditos { get; set; }
    public string NombreProfesor { get; set; } = string.Empty;
    public int InscritosActivos { get; set; }
}

public class ProfesorDto
{
    public int Id { get; set; }
    public string Documento { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public List<string> Materias { get; set; } = new();
}