using System.ComponentModel.DataAnnotations;

namespace Veterinaria.Entidades.DTOs.Auth;

public class LoginDto
{
    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
    public string Correo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La clave es obligatoria.")]
    public string Clave { get; set; } = string.Empty;
}
