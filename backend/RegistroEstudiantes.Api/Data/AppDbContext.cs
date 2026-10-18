using Microsoft.EntityFrameworkCore;
using RegistroEstudiantes.Api.Entities;

namespace RegistroEstudiantes.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Profesor> Profesores => Set<Profesor>();
    public DbSet<Materia> Materias => Set<Materia>();
    public DbSet<Estudiante> Estudiantes => Set<Estudiante>();
    public DbSet<Inscripcion> Inscripciones => Set<Inscripcion>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Profesor>().ToTable("Profesor");
        modelBuilder.Entity<Materia>().ToTable("Materia");
        modelBuilder.Entity<Estudiante>().ToTable("Estudiante");
        modelBuilder.Entity<Inscripcion>().ToTable("Inscripcion");

        modelBuilder.Entity<Materia>()
            .HasOne(m => m.Profesor)
            .WithMany(p => p.Materias)
            .HasForeignKey(m => m.ProfesorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Inscripcion>()
            .HasOne(i => i.Estudiante)
            .WithMany(e => e.Inscripciones)
            .HasForeignKey(i => i.EstudianteId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Inscripcion>()
            .HasOne(i => i.Materia)
            .WithMany(m => m.Inscripciones)
            .HasForeignKey(i => i.MateriaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}