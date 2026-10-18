namespace RegistroEstudiantes.Api.Exceptions;

public class ConflictoException : Exception
{
    public ConflictoException(string mensaje) : base(mensaje) { }
}

public class NoAutenticadoException : Exception
{
    public NoAutenticadoException(string mensaje) : base(mensaje) { }
}

public class ProhibidoException : Exception
{
    public ProhibidoException(string mensaje) : base(mensaje) { }
}

public class ReglaNegocioException : Exception
{
    public ReglaNegocioException(string mensaje) : base(mensaje) { }
}

public class NoEncontradoException : Exception
{
    public NoEncontradoException(string mensaje) : base(mensaje) { }
}