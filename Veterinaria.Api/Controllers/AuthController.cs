using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Veterinaria.Entidades.DTOs.Auth;
using Veterinaria.Negocio.Excepciones;
using Veterinaria.Negocio.Servicios;

namespace Veterinaria.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthServicio _auth;

    public AuthController(IAuthServicio auth)
    {
        _auth = auth;
    }

    // POST: api/auth/registro   (público)
    [AllowAnonymous]
    [HttpPost("registro")]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<UsuarioDto>> Registro(RegistroDto dto)
    {
        try
        {
            var usuario = await _auth.RegistrarAsync(dto);
            return StatusCode(StatusCodes.Status201Created, usuario);
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    // POST: api/auth/login   (público)
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TokenDto>> Login(LoginDto dto)
    {
        var token = await _auth.LoginAsync(dto);

        // Mismo 401 si el correo no existe o si la clave es incorrecta
        if (token is null)
            return Unauthorized(new { mensaje = "Correo o clave incorrectos." });

        return Ok(token);
    }

    // GET: api/auth/perfil   (solo autenticados) - los datos salen de los claims del token
    [Authorize]
    [HttpGet("perfil")]
    [ProducesResponseType(typeof(PerfilDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<PerfilDto> Perfil()
    {
        var perfil = new PerfilDto
        {
            Id = User.FindFirst("sub")?.Value ?? string.Empty,
            Correo = User.FindFirst("email")?.Value ?? string.Empty,
            Rol = User.FindFirst("rol")?.Value ?? string.Empty
        };
        return Ok(perfil);
    }
}
