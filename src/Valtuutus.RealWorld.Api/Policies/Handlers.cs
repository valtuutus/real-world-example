using Microsoft.AspNetCore.Authorization;
using Valtuutus.Core.Engines.Check;
using Valtuutus.Lang;
using Valtuutus.RealWorld.Api.Core.Auth;
using Valtuutus.RealWorld.Api.Core.Entities;
using Task = System.Threading.Tasks.Task;

namespace Valtuutus.RealWorld.Api.Policies;

public static class IdExtensions
{
    public static bool TryGetId<T>(this HttpContext context, string name, out T id)
    where T : struct, ISpanParsable<T>
    {
        var values = context.Request.RouteValues;
        if (!values.TryGetValue(name, out var intermediate)
            || !(intermediate is string idString) ||
            !T.TryParse(idString, null, out id))
        {
            id = default;
            return false;
        }
        return true;
    }
}

public abstract class BaseWorkspaceHandler<T>(ICheckEngine checkEngine, ISessaoManager sessaoManager) : AuthorizationHandler<T>
where T: WorkspaceRequirements.WorkspaceRequirement, IWithPermissionRequirement
{
    protected sealed override async Task HandleRequirementAsync(AuthorizationHandlerContext context, T requirement)
    {
        var ctx = (context.Resource as HttpContext)!;
        var ct = ctx.RequestAborted;
        if (!ctx.TryGetId<WorkspaceId>("workspaceId", out var workspaceId))
        {
            context.Fail();
            return;
        }
        var result = await checkEngine.Check(new CheckRequest
        {
            EntityType = SchemaConstsGen.Workspace.Name,
            EntityId = workspaceId.ToString(),
            SubjectType = SchemaConstsGen.User.Name,
            SubjectId = sessaoManager.UsuarioId.ToString(),
            Permission = requirement.Permission
        }, ct);
        if (!result)
        {
            context.Fail();
            return;
        }
        context.Succeed(requirement);
    }
}

public sealed class CreateProjectHandler(ICheckEngine checkEngine, ISessaoManager sessaoManager)
    : BaseWorkspaceHandler<WorkspaceRequirements.CreateProject>(checkEngine, sessaoManager);
    
public sealed class AssignUserHandler(ICheckEngine checkEngine, ISessaoManager sessaoManager)
    : BaseWorkspaceHandler<WorkspaceRequirements.AssignUser>(checkEngine, sessaoManager);
    
public sealed class ViewWorkspaceHandler(ICheckEngine checkEngine, ISessaoManager sessaoManager)
    : BaseWorkspaceHandler<WorkspaceRequirements.View>(checkEngine, sessaoManager);