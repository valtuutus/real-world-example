using Microsoft.AspNetCore.Mvc;
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
    
    public static void MapProjectsEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup("workspaces/{workspaceId}/projects");
        
        endpoints.MapPost("/", CreateProject)
            .RequireAuthorization();
    }
}