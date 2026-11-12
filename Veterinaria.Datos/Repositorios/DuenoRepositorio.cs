using Microsoft.EntityFrameworkCore;
using Veterinaria.Datos.Models;
using Veterinaria.Entidades;

namespace Veterinaria.Datos.Repositorios;

public class DuenoRepositorio : IDuenoRepositorio
{
    private readonly VeterinariaContext _context;

    public DuenoRepositorio(VeterinariaContext context)
    {
        _context = context;
    }

    public async Task<List<Dueno>> ListarAsync()
    {
        return await _context.Duenos
            .AsNoTracking()
            .Include(d => d.Mascota)
            .OrderBy(d => d.Nombres)
            .ToListAsync();
    }

    public async Task<Dueno?> ObtenerPorIdAsync(int id)
    {
        return await _context.Duenos
            .AsNoTracking()
            .Include(d => d.Mascota)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    // Sin AsNoTracking: EF Core sigue los cambios para poder actualizar o eliminar
    public async Task<Dueno?> ObtenerParaEditarAsync(int id)
    {
        return await _context.Duenos.FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<bool> ExisteAsync(int id)
    {
        return await _context.Duenos.AnyAsync(d => d.Id == id);
    }

    public async Task<bool> ExisteDniAsync(string dni, int? excluirId = null)
    {
        return await _context.Duenos
            .AnyAsync(d => d.Dni == dni && (excluirId == null || d.Id != excluirId));
    }

    public async Task<int> ContarMascotasAsync(int duenoId)
    {
        return await _context.Mascotas.CountAsync(m => m.DuenoId == duenoId);
    }

    public async Task AgregarAsync(Dueno dueno)
    {
        _context.Duenos.Add(dueno);
        await _context.SaveChangesAsync();
    }

    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(Dueno dueno)
    {
        _context.Duenos.Remove(dueno);
        await _context.SaveChangesAsync();
    }
}
