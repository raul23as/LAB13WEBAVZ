namespace Veterinaria.Negocio.Seguridad;

// Se llena con la sección "Jwt" de la configuración.
// La Clave y los minutos de expiración están en appsettings.Development.json (no se sube al repositorio).
public class JwtOpciones
{
    public string Clave { get; set; } = string.Empty;
    public string Emisor { get; set; } = string.Empty;
    public string Audiencia { get; set; } = string.Empty;
    public int MinutosExpiracion { get; set; } = 60;
}
