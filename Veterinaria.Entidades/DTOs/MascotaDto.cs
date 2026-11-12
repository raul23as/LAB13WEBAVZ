namespace Veterinaria.Entidades.DTOs;

// DTO de lectura: lo que la API devuelve de una mascota
public class MascotaDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Especie { get; set; } = string.Empty;
    public string? Raza { get; set; }
    public DateOnly FechaNacimiento { get; set; }
    public decimal Peso { get; set; }
    public int DuenoId { get; set; }

    // Nombre del dueño en lugar del objeto completo (ForMember + MapFrom)
    public string Dueno { get; set; } = string.Empty;
}
