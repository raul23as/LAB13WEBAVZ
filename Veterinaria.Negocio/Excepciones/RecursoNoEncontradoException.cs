namespace Veterinaria.Negocio.Excepciones;

// Se lanza cuando un recurso relacionado no existe. El controlador la convierte en 404 Not Found.
public class RecursoNoEncontradoException : Exception
{
    public RecursoNoEncontradoException(string mensaje) : base(mensaje)
    {
    }
}
