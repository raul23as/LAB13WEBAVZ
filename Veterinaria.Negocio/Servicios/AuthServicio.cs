using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Veterinaria.Datos.Repositorios;
using Veterinaria.Entidades;
using Veterinaria.Entidades.DTOs.Auth;
using Veterinaria.Negocio.Excepciones;
using Veterinaria.Negocio.Seguridad;

namespace Veterinaria.Negocio.Servicios;

public class AuthServicio : IAuthServicio
{
    private readonly IUsuarioRepositorio _usuarios;
    private readonly IMapper _mapper;
    private readonly JwtOpciones _jwt;

    public AuthServicio(IUsuarioRepositorio usuarios, IMapper mapper, IOptions<JwtOpciones> jwt)
    {
        _usuarios = usuarios;
        _mapper = mapper;
        _jwt = jwt.Value;
    }

    public async Task<UsuarioDto> RegistrarAsync(RegistroDto dto)
    {
        var correo = dto.Correo.Trim().ToLowerInvariant();

        // Regla de negocio: el correo no se puede repetir
        if (await _usuarios.ExisteCorreoAsync(correo))
            throw new ReglaNegocioException($"Ya existe un usuario registrado con el correo {correo}.");

        var usuario = _mapper.Map<Usuario>(dto);
        usuario.Correo = correo;
        usuario.ClaveHash = BCrypt.Net.BCrypt.HashPassword(dto.Clave);   // se guarda el hash, no la clave
        usuario.Rol = RolesVeterinaria.Recepcionista;                    // el rol lo asigna el servidor

        await _usuarios.AgregarAsync(usuario);
        return _mapper.Map<UsuarioDto>(usuario);
    }

    public async Task<TokenDto?> LoginAsync(LoginDto dto)
    {
        var usuario = await _usuarios.BuscarPorCorreoAsync(dto.Correo.Trim().ToLowerInvariant());

        // Mismo resultado si el correo no existe o si la clave es incorrecta
        if (usuario is null || !BCrypt.Net.BCrypt.Verify(dto.Clave, usuario.ClaveHash))
            return null;

        return GenerarToken(usuario);
    }

    private TokenDto GenerarToken(Usuario usuario)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, usuario.Correo),
            new("rol", usuario.Rol),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Clave));
        var credenciales = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);
        var expiracion = DateTime.UtcNow.AddMinutes(_jwt.MinutosExpiracion);

        var token = new JwtSecurityToken(
            issuer: _jwt.Emisor,
            audience: _jwt.Audiencia,
            claims: claims,
            expires: expiracion,
            signingCredentials: credenciales);

        return new TokenDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiracion = expiracion
        };
    }
}
