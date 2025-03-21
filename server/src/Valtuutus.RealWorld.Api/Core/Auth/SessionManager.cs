using System.Net;
using System.Security.Claims;
using Valtuutus.RealWorld.Api.Core.Entities;
using Task = System.Threading.Tasks.Task;

namespace Valtuutus.RealWorld.Api.Core.Auth;

public interface ISessionManager
{
    UserId UserId { get; }
    bool LoggedIn { get; }

    void SetUserId(UserId userId);
}

public class SessionManager : ISessionManager
{
    private UserId? _userId;

    public UserId UserId => _userId ?? throw new InvalidOperationException();

    public bool LoggedIn => _userId.HasValue;
    
    
    public void SetUserId(UserId userId)
    {
        _userId = userId;
    }
}

public class SessionManagerMiddleware(ISessionManager sessionManager) : IMiddleware
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

        if (nameIdentifier == null || !Guid.TryParse(nameIdentifier, out var userId))
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsync("Could not identify the user.");
            return;
        }

        sessionManager.SetUserId(new UserId(userId));
        await next(context);
    }
}