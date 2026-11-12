using AutoMapper;
using Veterinaria.Datos.Repositorios;
using Veterinaria.Entidades;
using Veterinaria.Entidades.DTOs;
using Veterinaria.Negocio.Excepciones;

namespace Veterinaria.Negocio.Servicios;

public class DuenoServicio : IDuenoServicio
{
    private readonly IDuenoRepositorio _repositorio;
    private readonly IMapper _mapper;

    public DuenoServicio(IDuenoRepositorio repositorio, IMapper mapper)
    {
        _repositorio = repositorio;
        _mapper = mapper;
    }

    public async Task<List<DuenoDto>> ListarAsync()
    {
        var duenos = await _repositorio.ListarAsync();
        return _mapper.Map<List<DuenoDto>>(duenos);
    }

    public async Task<DuenoDto?> ObtenerPorIdAsync(int id)
    {
        var dueno = await _repositorio.ObtenerPorIdAsync(id);
        return dueno is null ? null : _mapper.Map<DuenoDto>(dueno);
    }

    public async Task<DuenoDto> CrearAsync(DuenoCrearDto dto)
    {
        // Regla de negocio 1: el DNI no se puede repetir
        if (await _repositorio.ExisteDniAsync(dto.Dni))
            throw new ReglaNegocioException($"Ya existe un dueño registrado con el DNI {dto.Dni}.");

        var dueno = _mapper.Map<Dueno>(dto);          // crear: Map<T>
        await _repositorio.AgregarAsync(dueno);
        return _mapper.Map<DuenoDto>(dueno);
    }

    public async Task<bool> ActualizarAsync(int id, DuenoActualizarDto dto)
    {
        var dueno = await _repositorio.ObtenerParaEditarAsync(id);
        if (dueno is null) return false;

        // Regla de negocio 1 (también al actualizar): el DNI no puede ser de otro dueño
        if (await _repositorio.ExisteDniAsync(dto.Dni, excluirId: id))
            throw new ReglaNegocioException($"El DNI {dto.Dni} ya pertenece a otro dueño.");

        _mapper.Map(dto, dueno);                      // actualizar: Map(origen, destino)
        await _repositorio.GuardarCambiosAsync();
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var dueno = await _repositorio.ObtenerParaEditarAsync(id);
        if (dueno is null) return false;

        // Regla de negocio 2: no se puede eliminar un dueño que todavía tiene mascotas
        var cantidad = await _repositorio.ContarMascotasAsync(id);
        if (cantidad > 0)
            throw new ReglaNegocioException(
                $"No se puede eliminar al dueño porque tiene {cantidad} mascota(s) registrada(s).");

        await _repositorio.EliminarAsync(dueno);
        return true;
    }
}
