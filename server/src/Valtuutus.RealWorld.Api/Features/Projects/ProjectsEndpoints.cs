using Microsoft.AspNetCore.Mvc;
using Valtuutus.Lang;
using Valtuutus.RealWorld.Api.Core.Entities;
using Valtuutus.RealWorld.Api.Policies;
using Valtuutus.RealWorld.Api.Results;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Valtuutus.RealWorld.Api.Features.Projects;

public static class ProjectsEndpoints
{
    private static async Task<IResult> CreateProject([FromServices] CreateProjectHandler handler,
        [FromRoute] WorkspaceId workspaceId, [FromBody] CreateProjectReqBody req, CancellationToken ct)
    {
        return (await handler.Handle(new CreateProject {Body = req, WorkspaceId = workspaceId}, ct)).ToApiResult();
    }
    
    private static async Task<IResult> GetProject([FromServices] GetProjectHandler handler,
        [FromRoute] ProjectId projectId,CancellationToken ct)
    {
        return (await handler.Handle(new GetProjectRequest(projectId), ct)).ToApiResult();
    }
    
    private static async Task<IResult> GetProjectPermissions([FromServices] GetPermissionsHandler handler,
        [FromRoute] ProjectId projectId,CancellationToken ct)
    {
        return (await handler.Handle(new GetProjectPermission(projectId), ct)).ToApiResult();
    }
    
    private static async Task<IResult> GetProjects([FromServices] GetProjectsHandler handler, CancellationToken ct)
    {
        return (await handler.Handle(Projects.GetProjects.Instance, ct)).ToApiResult();
    }
    
    public static void MapProjectsEndpoints(this IEndpointRouteBuilder app)
    {
        var workspaceEndpoints = app.MapGroup("workspaces/{workspaceId}/projects");
        
        workspaceEndpoints.MapPost("/", CreateProject)
            .RequireAuthorization(AppPolicies.Workspace.CreateProject);
        
        workspaceEndpoints.MapGet("", GetProjects)
            .RequireAuthorization(AppPolicies.Workspace.View);

        var projectEndpoints = app.MapGroup("projects");
        
        projectEndpoints.MapGet("{projectId}", GetProject)
            .RequireAuthorization(AppPolicies.Project.View);

        projectEndpoints.MapGet("{projectId}/permissions", GetProjectPermissions)
            .RequireAuthorization(AppPolicies.Project.View);
    }
}