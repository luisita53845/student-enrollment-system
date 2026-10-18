using System.Security.Claims;

namespace RegistroEstudiantes.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int ObtenerId(this ClaimsPrincipal user)
    {
        var valor = user.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(valor, out var id)
            ? id
            : throw new UnauthorizedAccessException("Token sin identificador válido.");
    }
}