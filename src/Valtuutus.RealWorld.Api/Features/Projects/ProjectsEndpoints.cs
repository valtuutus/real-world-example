using Microsoft.AspNetCore.Mvc;
using Valtuutus.Lang;
using Valtuutus.RealWorld.Api.Core.Entities;
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
        [FromRoute] WorkspaceId workspaceId, [FromRoute] ProjectId projectId,CancellationToken ct)
    {
        return (await handler.Handle(new GetProjectRequest(workspaceId, projectId), ct)).ToApiResult();
    }
    
    private static async Task<IResult> GetProjectPermissions([FromServices] GetPermissionsHandler handler,
        [FromRoute] WorkspaceId workspaceId, [FromRoute] ProjectId projectId,CancellationToken ct)
    {
        return (await handler.Handle(new GetProjectPermission(workspaceId, projectId), ct)).ToApiResult();
    }
    
    public static void MapProjectsEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup("workspaces/{workspaceId}/projects");
        
        endpoints.MapPost("/", CreateProject)
            .RequireAuthorization(SchemaConstsGen.Workspace.Permissions.CreateProject);
        
        endpoints.MapGet("{projectId}", GetProject)
            .RequireAuthorization();

        endpoints.MapGet("{projectId}/permissions", GetProjectPermissions)
            .RequireAuthorization();
    }
}