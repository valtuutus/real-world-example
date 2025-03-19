using Microsoft.AspNetCore.Mvc;
using Valtuutus.Lang;
using Valtuutus.RealWorld.Api.Core.Entities;
using Valtuutus.RealWorld.Api.Features.Workspaces.Create;
using Valtuutus.RealWorld.Api.Results;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Valtuutus.RealWorld.Api.Features.Workspaces;

public static class WorkspaceEndpointsDefinitions
{
    private static async Task<IResult> CreateWorkspace([FromServices] CreateWorkspaceHandler handler,
        [FromBody] CreateWorkspaceRequest req, CancellationToken ct)
    {
        return (await handler.Handle(req, ct)).ToApiResult();
    }

    private static async Task<IResult> AssignUser([FromServices] AssignUserToWorkspaceHandler handler,
        [FromRoute] WorkspaceId workspaceId, [FromBody] AssignUserBodyReq req, CancellationToken ct)
    {
        return (await handler.Handle(new AssignUser {Body = req, WorkspaceId = workspaceId}, ct)).ToApiResult();
    }
        
    private static async Task<IResult> GetWorkspacePermissions([FromServices] GetPermissionsHandler handler,
        [FromRoute] WorkspaceId workspaceId, CancellationToken ct)
    {
        return (await handler.Handle(new GetWorkspacePermission(workspaceId), ct)).ToApiResult();
    }

    
    public static void MapWorkspaceEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("workspaces", CreateWorkspace)
            .RequireAuthorization();

        endpoints.MapPost("workspaces/{workspaceId}/assign", AssignUser)
            .RequireAuthorization(SchemaConstsGen.Workspace.Permissions.AssignUser);

        endpoints.MapGet("workspaces/{workspaceId}/permissions", GetWorkspacePermissions)
            .RequireAuthorization(SchemaConstsGen.Workspace.Permissions.View);
    }
}