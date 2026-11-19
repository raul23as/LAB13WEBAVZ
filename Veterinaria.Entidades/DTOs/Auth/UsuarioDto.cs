namespace Veterinaria.Entidades.DTOs.Auth;

// Respuesta del registro (nunca incluye el hash de la clave)
public class UsuarioDto
{
    public int Id { get; set; }
    public string Correo { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
}
