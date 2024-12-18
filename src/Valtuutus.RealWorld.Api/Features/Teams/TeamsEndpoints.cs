using Microsoft.AspNetCore.Mvc;
using Valtuutus.RealWorld.Api.Core.Entities;
using Valtuutus.RealWorld.Api.Features.Projects;
using Valtuutus.RealWorld.Api.Results;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Valtuutus.RealWorld.Api.Features.Teams;

public static class TeamsEndpoints
{
    private static async Task<IResult> CreateTeam([FromServices] CreateTeamHandler handler,
        [FromRoute] WorkspaceId workspaceId, [FromBody] CreateTeamReqBody req, CancellationToken ct)
    {
        return (await handler.Handle(new CreateTeam(workspaceId, req), ct)).ToApiResult();
    }
    
    public static void MapTeamsEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup("workspaces/{workspaceId}/teams");
        
        endpoints.MapPost("/", CreateTeam)
            .RequireAuthorization();
    }
}