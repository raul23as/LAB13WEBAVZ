namespace Veterinaria.Entidades;

// Roles propios del negocio. El rol lo decide el servidor, nunca el cliente.
public static class RolesVeterinaria
{
    public const string Recepcionista = "Recepcionista";
    public const string Veterinario = "Veterinario";

    // Para endpoints que aceptan cualquiera de los dos roles
    public const string Todos = Recepcionista + "," + Veterinario;
}
