namespace Veterinaria.Entidades.DTOs.Auth;

// Respuesta del login
public class TokenDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expiracion { get; set; }
}
