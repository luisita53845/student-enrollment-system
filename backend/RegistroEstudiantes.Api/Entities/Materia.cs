namespace RegistroEstudiantes.Api.Entities;

public class Materia
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int Creditos { get; set; }
    public int ProfesorId { get; set; }

    public Profesor Profesor { get; set; } = null!;
    public List<Inscripcion> Inscripciones { get; set; } = new();
}