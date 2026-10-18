using RegistroEstudiantes.Api.Exceptions;

namespace RegistroEstudiantes.Api.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var (status, mensaje) = ex switch
            {
                ConflictoException => (409, ex.Message),
                NoAutenticadoException => (401, ex.Message),
                ProhibidoException => (403, ex.Message),
                ReglaNegocioException => (400, ex.Message),
                NoEncontradoException => (404, ex.Message),
                _ => (500, "Ocurrió un error inesperado. Intenta de nuevo más tarde.")
            };

            if (status == 500)
                _logger.LogError(ex, "Error no controlado");

            context.Response.StatusCode = status;
            await context.Response.WriteAsJsonAsync(new { mensaje });
        }
    }
}