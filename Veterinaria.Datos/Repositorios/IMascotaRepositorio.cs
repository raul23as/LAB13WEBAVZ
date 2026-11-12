using Veterinaria.Entidades;

namespace Veterinaria.Datos.Repositorios;

public interface IMascotaRepositorio
{
    Task<List<Mascota>> ListarAsync(string? especie, string? nombre, int? duenoId);
    Task<List<Mascota>> ListarPorDuenoAsync(int duenoId);
    Task<Mascota?> ObtenerPorIdAsync(int id);
    Task<Mascota?> ObtenerParaEditarAsync(int id);
    Task AgregarAsync(Mascota mascota);
    Task GuardarCambiosAsync();
    Task EliminarAsync(Mascota mascota);
}
