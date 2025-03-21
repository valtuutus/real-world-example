using Microsoft.AspNetCore.Mvc;
using Valtuutus.Lang;
using Valtuutus.RealWorld.Api.Core.Entities;
using Valtuutus.RealWorld.Api.Features.Projects;
using Valtuutus.RealWorld.Api.Policies;
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
    
    private static async Task<IResult> GetTasks([FromServices] GetTasksHandler handler,
        [FromRoute] ProjectId projectId, CancellationToken ct)
    {
        return (await handler.Handle(Tasks.GetTasks.Instance, ct)).ToApiResult();
    }
    
    public static void MapTaskEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup("projects/{projectId}/tasks");
        
        endpoints.MapPost("/", CreateTask)
            .RequireAuthorization(AppPolicies.Project.CreateTask);
        
                
        endpoints.MapGet("", GetTasks)
            .RequireAuthorization(AppPolicies.Project.View);
    }
}