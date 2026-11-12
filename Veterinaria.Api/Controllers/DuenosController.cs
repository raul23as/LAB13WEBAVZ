using Microsoft.AspNetCore.Mvc;
using Veterinaria.Entidades.DTOs;
using Veterinaria.Negocio.Excepciones;
using Veterinaria.Negocio.Servicios;

namespace Veterinaria.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DuenosController : ControllerBase
{
    private readonly IDuenoServicio _duenos;
    private readonly IMascotaServicio _mascotas;

    public DuenosController(IDuenoServicio duenos, IMascotaServicio mascotas)
    {
        _duenos = duenos;
        _mascotas = mascotas;
    }

    // GET: api/duenos
    [HttpGet]
    [ProducesResponseType(typeof(List<DuenoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<DuenoDto>>> Listar()
    {
        return Ok(await _duenos.ListarAsync());
    }

    // GET: api/duenos/5
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(DuenoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DuenoDto>> ObtenerPorId(int id)
    {
        var dueno = await _duenos.ObtenerPorIdAsync(id);
        if (dueno is null)
            return NotFound(new { mensaje = $"No existe un dueño con Id {id}." });

        return Ok(dueno);
    }

    // GET: api/duenos/5/mascotas   (ruta anidada)
    [HttpGet("{id:int}/mascotas")]
    [ProducesResponseType(typeof(List<MascotaDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<MascotaDto>>> ListarMascotas(int id)
    {
        var mascotas = await _mascotas.ListarPorDuenoAsync(id);
        if (mascotas is null)
            return NotFound(new { mensaje = $"No existe un dueño con Id {id}." });

        return Ok(mascotas);
    }

    // POST: api/duenos
    [HttpPost]
    [ProducesResponseType(typeof(DuenoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<DuenoDto>> Crear(DuenoCrearDto dto)
    {
        try
        {
            var creado = await _duenos.CrearAsync(dto);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.Id }, creado);
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    // PUT: api/duenos/5
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Actualizar(int id, DuenoActualizarDto dto)
    {
        try
        {
            var actualizado = await _duenos.ActualizarAsync(id, dto);
            if (!actualizado)
                return NotFound(new { mensaje = $"No existe un dueño con Id {id}." });

            return NoContent();
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    // DELETE: api/duenos/5
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Eliminar(int id)
    {
        try
        {
            var eliminado = await _duenos.EliminarAsync(id);
            if (!eliminado)
                return NotFound(new { mensaje = $"No existe un dueño con Id {id}." });

            return NoContent();
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }
}
