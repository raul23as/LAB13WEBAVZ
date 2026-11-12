using Veterinaria.Entidades;

namespace Veterinaria.Datos.Repositorios;

public interface IDuenoRepositorio
{
    Task<List<Dueno>> ListarAsync();
    Task<Dueno?> ObtenerPorIdAsync(int id);
    Task<Dueno?> ObtenerParaEditarAsync(int id);
    Task<bool> ExisteAsync(int id);
    Task<bool> ExisteDniAsync(string dni, int? excluirId = null);
    Task<int> ContarMascotasAsync(int duenoId);
    Task AgregarAsync(Dueno dueno);
    Task GuardarCambiosAsync();
    Task EliminarAsync(Dueno dueno);
}
