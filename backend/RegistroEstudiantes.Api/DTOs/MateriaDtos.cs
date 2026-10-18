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