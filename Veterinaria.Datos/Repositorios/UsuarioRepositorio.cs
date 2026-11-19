using Microsoft.EntityFrameworkCore;
using Veterinaria.Datos.Models;
using Veterinaria.Entidades;

namespace Veterinaria.Datos.Repositorios;

public class UsuarioRepositorio : IUsuarioRepositorio
{
    private readonly VeterinariaContext _context;

    public UsuarioRepositorio(VeterinariaContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> BuscarPorCorreoAsync(string correo)
    {
        return await _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Correo == correo);
    }

    public async Task<bool> ExisteCorreoAsync(string correo)
    {
        return await _context.Usuarios.AnyAsync(u => u.Correo == correo);
    }

    public async Task AgregarAsync(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
    }

    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }
}
