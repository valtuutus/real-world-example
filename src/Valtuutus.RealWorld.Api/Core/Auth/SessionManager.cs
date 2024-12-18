using System.Net;
using System.Security.Claims;
using Valtuutus.RealWorld.Api.Core.Entities;
using Task = System.Threading.Tasks.Task;

namespace Valtuutus.RealWorld.Api.Core.Auth;

public interface ISessaoManager
{
    UserId UsuarioId { get; }
    bool LoggedIn { get; }

    void SetUsuarioId(UserId usuarioId);
}

public class SessaoManager : ISessaoManager
{
    private UserId? _usuarioId;

    public UserId UsuarioId => _usuarioId ?? throw new InvalidOperationException();

    public bool LoggedIn => _usuarioId.HasValue;
    
    
    public void SetUsuarioId(UserId usuarioId)
    {
        _usuarioId = usuarioId;
    }
}

public class SessionManagerMiddleware(ISessaoManager sessaoManager) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var user = context.User;
        
        if (!context.User.Identity?.IsAuthenticated ?? false)
        {
            // Is up to the authorization to determine if the route requires an authenticated user.
            // So we just next it.
            await next(context);
            return;
        }
        
        var nameIdentifier = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (nameIdentifier == null || !Guid.TryParse(nameIdentifier, out var usuarioId))
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsync("Could not identify the user.");
            return;
        }

        sessaoManager.SetUsuarioId(new UserId(usuarioId));
    }
}