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

public abstract class BaseWorkspaceHandler<T>(ICheckEngine checkEngine, ISessionManager sessionManager) : AuthorizationHandler<T>
where T: WorkspaceRequirements.WorkspaceRequirement, IWithPermissionRequirement
{
    protected sealed override async Task HandleRequirementAsync(AuthorizationHandlerContext context, T requirement)
    {
        if (!sessionManager.LoggedIn)
        {
            // logger.LogInformation("CompanyOwnerAuthorizationHandler, user is not authenticated");
            context.Fail(new AuthorizationFailureReason(this, "user is not authenticated"));
            return;
        }
        
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
            SubjectId = sessionManager.UserId.ToString(),
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

public abstract class BaseProjectHandler<T>(ICheckEngine checkEngine, ISessionManager sessionManager) : AuthorizationHandler<T>
    where T: ProjectRequirements.ProjectRequirement, IWithPermissionRequirement
{
    protected sealed override async Task HandleRequirementAsync(AuthorizationHandlerContext context, T requirement)
    {
        if (!sessionManager.LoggedIn)
        {
            // logger.LogInformation("CompanyOwnerAuthorizationHandler, user is not authenticated");
            context.Fail(new AuthorizationFailureReason(this, "user is not authenticated"));
            return;
        }
        
        var ctx = (context.Resource as HttpContext)!;
        var ct = ctx.RequestAborted;
        if (!ctx.TryGetId<ProjectId>("projectId", out var projectId))
        {
            context.Fail();
            return;
        }
        var result = await checkEngine.Check(new CheckRequest
        {
            EntityType = SchemaConstsGen.Project.Name,
            EntityId = projectId.ToString(),
            SubjectType = SchemaConstsGen.User.Name,
            SubjectId = sessionManager.UserId.ToString(),
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


public sealed class CreateProjectHandler(ICheckEngine checkEngine, ISessionManager sessionManager)
    : BaseWorkspaceHandler<WorkspaceRequirements.CreateProject>(checkEngine, sessionManager);
    
public sealed class AssignUserHandler(ICheckEngine checkEngine, ISessionManager sessionManager)
    : BaseWorkspaceHandler<WorkspaceRequirements.AssignUser>(checkEngine, sessionManager);
    
public sealed class ViewWorkspaceHandler(ICheckEngine checkEngine, ISessionManager sessionManager)
    : BaseWorkspaceHandler<WorkspaceRequirements.View>(checkEngine, sessionManager);
    
public sealed class ViewProjectHandler(ICheckEngine checkEngine, ISessionManager sessionManager)
    : BaseProjectHandler<ProjectRequirements.View>(checkEngine, sessionManager);

public sealed class EditProjectHandler(ICheckEngine checkEngine, ISessionManager sessionManager)
    : BaseProjectHandler<ProjectRequirements.Edit>(checkEngine, sessionManager);
    
public sealed class CreateTaskHandler(ICheckEngine checkEngine, ISessionManager sessionManager)
    : BaseProjectHandler<ProjectRequirements.CreateTask>(checkEngine, sessionManager);