using Microsoft.AspNetCore.Mvc;
using Valtuutus.RealWorld.Api.Core.Entities;
using Valtuutus.RealWorld.Api.Features.Projects;
using Valtuutus.RealWorld.Api.Results;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Valtuutus.RealWorld.Api.Features.Tasks;

public static class TasksEndpoints
{
    private static async Task<IResult> CreateTask([FromServices] CreateTaskHandler handler,
        [FromRoute] ProjectId projectId, [FromBody] CreateTaskReqBody req, CancellationToken ct)
    {
        return (await handler.Handle(new CreateTask() {Body = req, ProjectId = projectId}, ct)).ToApiResult();
    }
    
    public static void MapProjectsEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup("projects/{projectId}/tasks");
        
        endpoints.MapPost("/", CreateTask)
            .RequireAuthorization();
    }
}