using System.ComponentModel.DataAnnotations;

namespace Veterinaria.Entidades.DTOs;

// DTO de entrada para actualizar un dueño (PUT)
public class DuenoActualizarDto
{
    [Required(ErrorMessage = "Los nombres son obligatorios.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Los nombres deben tener entre 3 y 100 caracteres.")]
    public string Nombres { get; set; } = string.Empty;

    [Required(ErrorMessage = "El DNI es obligatorio.")]
    [StringLength(8, MinimumLength = 8, ErrorMessage = "El DNI debe tener 8 dígitos.")]
    [RegularExpression(@"^\d{8}$", ErrorMessage = "El DNI solo debe contener 8 dígitos.")]
    public string Dni { get; set; } = string.Empty;

    [StringLength(9, MinimumLength = 9, ErrorMessage = "El teléfono debe tener 9 dígitos.")]
    [RegularExpression(@"^\d{9}$", ErrorMessage = "El teléfono solo debe contener 9 dígitos.")]
    public string? Telefono { get; set; }

    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
    [StringLength(100, ErrorMessage = "El correo admite máximo 100 caracteres.")]
    public string? Correo { get; set; }
}
