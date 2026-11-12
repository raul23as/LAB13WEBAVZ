namespace Veterinaria.Entidades.DTOs;

// DTO de lectura: lo que la API devuelve de un dueño
public class DuenoDto
{
    public int Id { get; set; }
    public string Nombres { get; set; } = string.Empty;
    public string Dni { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string? Correo { get; set; }

    // Se calcula en el MappingProfile (ForMember + MapFrom)
    public int CantidadMascotas { get; set; }
}
