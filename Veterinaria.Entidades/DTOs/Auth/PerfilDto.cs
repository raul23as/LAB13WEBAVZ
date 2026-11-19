namespace Veterinaria.Entidades.DTOs.Auth;

// Datos tomados de los claims del token (no de la base de datos)
public class PerfilDto
{
    public string Id { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
}
