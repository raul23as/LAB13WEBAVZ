using System.ComponentModel.DataAnnotations;

namespace Veterinaria.Entidades.DTOs;

// DTO de entrada para actualizar una mascota (PUT)
public class MascotaActualizarDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 50 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La especie es obligatoria.")]
    [StringLength(30, ErrorMessage = "La especie admite máximo 30 caracteres.")]
    public string Especie { get; set; } = string.Empty;

    [StringLength(50, ErrorMessage = "La raza admite máximo 50 caracteres.")]
    public string? Raza { get; set; }

    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
    public DateOnly FechaNacimiento { get; set; }

    [Range(0.01, 999.99, ErrorMessage = "El peso debe estar entre 0.01 y 999.99 kg.")]
    public decimal Peso { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe indicar un DuenoId válido.")]
    public int DuenoId { get; set; }
}
