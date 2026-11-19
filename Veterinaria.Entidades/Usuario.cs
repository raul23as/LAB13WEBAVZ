namespace Veterinaria.Entidades;

// Semana 14: usuario del sistema (se agregó a mano, sin volver a hacer Scaffold)
public partial class Usuario
{
    public int Id { get; set; }

    public string Correo { get; set; } = null!;

    // Solo se guarda el hash BCrypt de la contraseña, nunca la contraseña en texto
    public string ClaveHash { get; set; } = null!;

    public string Rol { get; set; } = null!;
}
