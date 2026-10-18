namespace RegistroEstudiantes.Api.Entities;

public class Profesor
{
    public int Id { get; set; }
    public string Documento { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string? Telefono { get; set; }

    public List<Materia> Materias { get; set; } = new();
}