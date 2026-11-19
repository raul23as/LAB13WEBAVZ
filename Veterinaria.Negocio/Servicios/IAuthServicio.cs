using Veterinaria.Entidades.DTOs.Auth;

namespace Veterinaria.Negocio.Servicios;

public interface IAuthServicio
{
    Task<UsuarioDto> RegistrarAsync(RegistroDto dto);

    // Devuelve null si el correo no existe o la clave es incorrecta
    Task<TokenDto?> LoginAsync(LoginDto dto);
}
