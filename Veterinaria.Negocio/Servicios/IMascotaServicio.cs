using Veterinaria.Entidades.DTOs;

namespace Veterinaria.Negocio.Servicios;

public interface IMascotaServicio
{
    Task<List<MascotaDto>> ListarAsync(string? especie, string? nombre, int? duenoId);
    Task<List<MascotaDto>?> ListarPorDuenoAsync(int duenoId);
    Task<MascotaDto?> ObtenerPorIdAsync(int id);
    Task<MascotaDto> CrearAsync(MascotaCrearDto dto);
    Task<bool> ActualizarAsync(int id, MascotaActualizarDto dto);
    Task<bool> EliminarAsync(int id);
}
