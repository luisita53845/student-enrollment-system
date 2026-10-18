namespace RegistroEstudiantes.Api.Entities;

public class Estudiante
{
    public int Id { get; set; }
    public string Documento { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public string Rol { get; set; } = "Estudiante";
    public bool Activo { get; set; } = true;
    public DateTime FechaRegistro { get; set; }

    public List<Inscripcion> Inscripciones { get; set; } = new();
}   