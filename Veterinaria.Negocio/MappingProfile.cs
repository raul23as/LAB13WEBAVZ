using AutoMapper;
using Veterinaria.Entidades;
using Veterinaria.Entidades.DTOs;
using Veterinaria.Entidades.DTOs.Auth;

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

        // ---------- Usuario (semana 14) ----------
        // DTO de registro -> Entidad: la clave se convierte en hash y el rol lo pone el servidor
        CreateMap<RegistroDto, Usuario>()
            .ForMember(destino => destino.ClaveHash, opcion => opcion.Ignore())
            .ForMember(destino => destino.Rol, opcion => opcion.Ignore());

        // Entidad -> DTO de respuesta del registro (sin el hash de la clave)
        CreateMap<Usuario, UsuarioDto>();
    }
}
