using Microsoft.EntityFrameworkCore;
using Veterinaria.Datos.Models;
using Veterinaria.Entidades;

namespace Veterinaria.Datos.Repositorios;

public class MascotaRepositorio : IMascotaRepositorio
{
    private readonly VeterinariaContext _context;

    public MascotaRepositorio(VeterinariaContext context)
    {
        _context = context;
    }

    // Filtros opcionales por query string: especie, nombre y duenoId
    public async Task<List<Mascota>> ListarAsync(string? especie, string? nombre, int? duenoId)
    {
        var consulta = _context.Mascotas
            .AsNoTracking()
            .Include(m => m.Dueno)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(especie))
            consulta = consulta.Where(m => m.Especie == especie);

        if (!string.IsNullOrWhiteSpace(nombre))
            consulta = consulta.Where(m => m.Nombre.Contains(nombre));

        if (duenoId.HasValue)
            consulta = consulta.Where(m => m.DuenoId == duenoId.Value);

        return await consulta.OrderBy(m => m.Nombre).ToListAsync();
    }

    public async Task<List<Mascota>> ListarPorDuenoAsync(int duenoId)
    {
        return await _context.Mascotas
            .AsNoTracking()
            .Include(m => m.Dueno)
            .Where(m => m.DuenoId == duenoId)
            .OrderBy(m => m.Nombre)
            .ToListAsync();
    }

    public async Task<Mascota?> ObtenerPorIdAsync(int id)
    {
        return await _context.Mascotas
            .AsNoTracking()
            .Include(m => m.Dueno)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<Mascota?> ObtenerParaEditarAsync(int id)
    {
        return await _context.Mascotas.FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task AgregarAsync(Mascota mascota)
    {
        _context.Mascotas.Add(mascota);
        await _context.SaveChangesAsync();
    }

    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(Mascota mascota)
    {
        _context.Mascotas.Remove(mascota);
        await _context.SaveChangesAsync();
    }
}
