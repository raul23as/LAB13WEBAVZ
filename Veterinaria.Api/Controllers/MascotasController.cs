using Microsoft.AspNetCore.Mvc;
using Veterinaria.Entidades.DTOs;
using Veterinaria.Negocio.Excepciones;
using Veterinaria.Negocio.Servicios;

namespace Veterinaria.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MascotasController : ControllerBase
{
    private readonly IMascotaServicio _mascotas;

    public MascotasController(IMascotaServicio mascotas)
    {
        _mascotas = mascotas;
    }

    // GET: api/mascotas?especie=Perro&nombre=ro&duenoId=1   (filtros por query string)
    [HttpGet]
    [ProducesResponseType(typeof(List<MascotaDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<MascotaDto>>> Listar(
        [FromQuery] string? especie, [FromQuery] string? nombre, [FromQuery] int? duenoId)
    {
        return Ok(await _mascotas.ListarAsync(especie, nombre, duenoId));
    }

    // GET: api/mascotas/5
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(MascotaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MascotaDto>> ObtenerPorId(int id)
    {
        var mascota = await _mascotas.ObtenerPorIdAsync(id);
        if (mascota is null)
            return NotFound(new { mensaje = $"No existe una mascota con Id {id}." });

        return Ok(mascota);
    }

    // POST: api/mascotas
    [HttpPost]
    [ProducesResponseType(typeof(MascotaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<MascotaDto>> Crear(MascotaCrearDto dto)
    {
        try
        {
            var creada = await _mascotas.CrearAsync(dto);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = creada.Id }, creada);
        }
        catch (RecursoNoEncontradoException ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    // PUT: api/mascotas/5
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Actualizar(int id, MascotaActualizarDto dto)
    {
        try
        {
            var actualizada = await _mascotas.ActualizarAsync(id, dto);
            if (!actualizada)
                return NotFound(new { mensaje = $"No existe una mascota con Id {id}." });

            return NoContent();
        }
        catch (RecursoNoEncontradoException ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    // DELETE: api/mascotas/5
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Eliminar(int id)
    {
        var eliminada = await _mascotas.EliminarAsync(id);
        if (!eliminada)
            return NotFound(new { mensaje = $"No existe una mascota con Id {id}." });

        return NoContent();
    }
}
