using AutoMapper;
using Veterinaria.Datos.Repositorios;
using Veterinaria.Entidades;
using Veterinaria.Entidades.DTOs;
using Veterinaria.Negocio.Excepciones;

namespace Veterinaria.Negocio.Servicios;

public class MascotaServicio : IMascotaServicio
{
    public const int MaximoMascotasPorDueno = 5;

    private readonly IMascotaRepositorio _repositorio;
    private readonly IDuenoRepositorio _duenos;
    private readonly IMapper _mapper;

    public MascotaServicio(IMascotaRepositorio repositorio, IDuenoRepositorio duenos, IMapper mapper)
    {
        _repositorio = repositorio;
        _duenos = duenos;
        _mapper = mapper;
    }

    public async Task<List<MascotaDto>> ListarAsync(string? especie, string? nombre, int? duenoId)
    {
        var mascotas = await _repositorio.ListarAsync(especie, nombre, duenoId);
        return _mapper.Map<List<MascotaDto>>(mascotas);
    }

    // Ruta anidada: devuelve null si el dueño no existe (el controlador responde 404)
    public async Task<List<MascotaDto>?> ListarPorDuenoAsync(int duenoId)
    {
        if (!await _duenos.ExisteAsync(duenoId)) return null;

        var mascotas = await _repositorio.ListarPorDuenoAsync(duenoId);
        return _mapper.Map<List<MascotaDto>>(mascotas);
    }

    public async Task<MascotaDto?> ObtenerPorIdAsync(int id)
    {
        var mascota = await _repositorio.ObtenerPorIdAsync(id);
        return mascota is null ? null : _mapper.Map<MascotaDto>(mascota);
    }

    public async Task<MascotaDto> CrearAsync(MascotaCrearDto dto)
    {
        await ValidarDuenoAsync(dto.DuenoId);

        var mascota = _mapper.Map<Mascota>(dto);      // crear: Map<T>
        await _repositorio.AgregarAsync(mascota);

        // Se vuelve a leer con el dueño incluido para devolver su nombre en el DTO
        var creada = await _repositorio.ObtenerPorIdAsync(mascota.Id);
        return _mapper.Map<MascotaDto>(creada!);
    }

    public async Task<bool> ActualizarAsync(int id, MascotaActualizarDto dto)
    {
        var mascota = await _repositorio.ObtenerParaEditarAsync(id);
        if (mascota is null) return false;

        // Solo se valida el límite si la mascota cambia de dueño
        if (mascota.DuenoId != dto.DuenoId)
            await ValidarDuenoAsync(dto.DuenoId);

        _mapper.Map(dto, mascota);                    // actualizar: Map(origen, destino)
        await _repositorio.GuardarCambiosAsync();
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var mascota = await _repositorio.ObtenerParaEditarAsync(id);
        if (mascota is null) return false;

        await _repositorio.EliminarAsync(mascota);
        return true;
    }

    private async Task ValidarDuenoAsync(int duenoId)
    {
        if (!await _duenos.ExisteAsync(duenoId))
            throw new RecursoNoEncontradoException($"No existe un dueño con Id {duenoId}.");

        // Regla de negocio 3: un dueño puede tener como máximo 5 mascotas
        var cantidad = await _duenos.ContarMascotasAsync(duenoId);
        if (cantidad >= MaximoMascotasPorDueno)
            throw new ReglaNegocioException(
                $"El dueño ya tiene {cantidad} mascotas; el máximo permitido es {MaximoMascotasPorDueno}.");
    }
}
