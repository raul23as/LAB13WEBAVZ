using Veterinaria.Entidades.DTOs;

namespace Veterinaria.Negocio.Servicios;

public interface IDuenoServicio
{
    Task<List<DuenoDto>> ListarAsync();
    Task<DuenoDto?> ObtenerPorIdAsync(int id);
    Task<DuenoDto> CrearAsync(DuenoCrearDto dto);
    Task<bool> ActualizarAsync(int id, DuenoActualizarDto dto);
    Task<bool> EliminarAsync(int id);
}
