using AutoMapper;
using Veterinaria.Entidades;
using Veterinaria.Entidades.DTOs;

namespace Veterinaria.Negocio;

// Único perfil con todos los mapeos de la aplicación
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // ---------- Dueño ----------
        // Entidad -> DTO de lectura (lo que devuelve la API)
        CreateMap<Dueno, DuenoDto>()
            .ForMember(destino => destino.CantidadMascotas,
                       opcion => opcion.MapFrom(origen => origen.Mascota.Count));

        // DTO de entrada -> Entidad (POST: crear una entidad nueva)
        CreateMap<DuenoCrearDto, Dueno>();

        // DTO de entrada -> Entidad (PUT: copiar los datos sobre la entidad existente)
        CreateMap<DuenoActualizarDto, Dueno>();

        // ---------- Mascota ----------
        // Entidad -> DTO de lectura: se devuelve el nombre del dueño, no el objeto
        CreateMap<Mascota, MascotaDto>()
            .ForMember(destino => destino.Dueno,
                       opcion => opcion.MapFrom(origen => origen.Dueno.Nombres));

        CreateMap<MascotaCrearDto, Mascota>();

        CreateMap<MascotaActualizarDto, Mascota>();
    }
}
