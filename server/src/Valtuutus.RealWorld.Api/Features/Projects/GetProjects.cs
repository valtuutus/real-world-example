using Microsoft.EntityFrameworkCore;
using Valtuutus.Core.Engines.LookupEntity;
using Valtuutus.Lang;
using Valtuutus.RealWorld.Api.Core;
using Valtuutus.RealWorld.Api.Core.Auth;
using Valtuutus.RealWorld.Api.Core.Entities;
using Valtuutus.RealWorld.Api.Features.Workspaces;
using Valtuutus.RealWorld.Api.Results;

namespace Valtuutus.RealWorld.Api.Features.Projects;

public record GetProjects
{
    private GetProjects()
    {
    }

    public static GetProjects Instance { get; } = new();
}

public record ProjectDto
{
    public required ProjectId Id { get; init; }
    public required string Name { get; init; }
    public required int TaskCount { get; init; }
    public required int CompletedTaskCount { get; init; }
}

public class GetProjectsHandler(
    ILookupEntityEngine lookupEngine, 
    ISessionManager sessionManager,
    Context context) : IUseCase<GetProjects, ProjectDto[]>
{
    public async Task<Result<ProjectDto[]>> Handle(GetProjects req, CancellationToken ct)
    {
        var workspaces = await lookupEngine.LookupEntity(new ()
        {
            EntityType = SchemaConstsGen.Project.Name,
            SubjectType = SchemaConstsGen.User.Name,
            SubjectId = sessionManager.UserId.ToString(),
            Permission = SchemaConstsGen.Project.Permissions.View,
            Depth = 10
        }, ct);

        var projectIds = workspaces.Select(ProjectId.Parse);
        
        return await context.Projects.Where(x => projectIds.Contains(x.Id))
            .Select(x => new ProjectDto
            {
                Id = x.Id,
                Name = x.Name,
                TaskCount = x.Tasks.Count,
                CompletedTaskCount = x.Tasks.Count(y => y.ProjectStatus.Type == ProjectStatusType.Done)
            })
            .ToArrayAsync(ct);
    }
}