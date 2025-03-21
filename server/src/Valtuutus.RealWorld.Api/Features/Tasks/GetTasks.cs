using Microsoft.EntityFrameworkCore;
using Valtuutus.Core.Engines.LookupEntity;
using Valtuutus.Lang;
using Valtuutus.RealWorld.Api.Core;
using Valtuutus.RealWorld.Api.Core.Auth;
using Valtuutus.RealWorld.Api.Core.Entities;
using Valtuutus.RealWorld.Api.Features.Projects;
using Valtuutus.RealWorld.Api.Results;

namespace Valtuutus.RealWorld.Api.Features.Tasks;

public record GetTasks
{
    private GetTasks()
    {
    }

    public static GetTasks Instance { get; } = new();
}

public record TaskDto
{
    public TaskId Id { get; init; }
    public string Name { get; init; }
    public float Order { get; init; }
    public ProjectStatusId StatusId { get; init; }
    public List<UserId> Assignees { get; init; } = new();
}

public class GetTasksHandler(
    ILookupEntityEngine lookupEngine, 
    ISessionManager sessionManager,
    Context context) : IUseCase<GetTasks, TaskDto[]>
{
    public async Task<Result<TaskDto[]>> Handle(GetTasks req, CancellationToken ct)
    {
        var tasks = await lookupEngine.LookupEntity(new ()
        {
            EntityType = SchemaConstsGen.Task.Name,
            SubjectType = SchemaConstsGen.User.Name,
            SubjectId = sessionManager.UserId.ToString(),
            Permission = SchemaConstsGen.Task.Permissions.View,
            Depth = 10
        }, ct);

        var taskIds = tasks.Select(TaskId.Parse);
        
        return await context.Tasks.Where(x => taskIds.Contains(x.Id))
            .Select(x => new TaskDto
            {
                Id = x.Id,
                Name = x.Name,
                Order = x.Order,
                StatusId = x.ProjectStatusId,
                Assignees = x.Assignees.Select(y => y.UserId).ToList(),
            })
            .ToArrayAsync(ct);
    }
}