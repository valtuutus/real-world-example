using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Valtuutus.Core;
using Valtuutus.Data.Db;
using Valtuutus.RealWorld.Api.Core;
using Valtuutus.RealWorld.Api.Core.Auth;
using Valtuutus.RealWorld.Api.Core.Entities;
using Valtuutus.RealWorld.Api.Results;

namespace Valtuutus.RealWorld.Api.Features.Projects;

public record CreateProjectReqBody
{
    public required string Name { get; init; }
}

public record CreateProject
{
    public required WorkspaceId WorkspaceId { get; init; }
    public required CreateProjectReqBody Body { get; init; }
}

public class CreateProjectHandler(Context context, ISessaoManager manager, IDbDataWriterProvider dataWriterProvider) : IUseCase<CreateProject, ProjectId>
{
    public async Task<Result<ProjectId>> Handle(CreateProject req, CancellationToken ct)
    {
        var project = new Project
        {
            Id = ProjectId.New(),
            Name = req.Body.Name,
            WorkspaceId = req.WorkspaceId,
        };

        context.Projects.Add(project);

        var projectAdmin = new ProjectUserAssignee()
            { ProjectId = project.Id, UserId = manager.UsuarioId, Type = ProjectAssigneeType.Admin };
        
        context.ProjectUserAssignees.Add(projectAdmin);
        
        context.ProjectStatuses.AddRange([
            new ProjectStatus
            {
                Id = ProjectStatusId.New(),
                Name = "Todo",
                Type = ProjectStatusType.Waiting,
                ProjectId = project.Id,
                Order = 1.0f
            },
            new ProjectStatus
            {
                Id = ProjectStatusId.New(),
                Name = "Doing",
                Type = ProjectStatusType.Active,
                ProjectId = project.Id,
                Order = 2.0f
            },
            new ProjectStatus
            {
                Id = ProjectStatusId.New(),
                Name = "Done",
                Type = ProjectStatusType.Done,
                ProjectId = project.Id,
                Order = 3.0f
            },
        ]);
        var transaction = await context.Database.BeginTransactionAsync(ct);

        await dataWriterProvider.Write(context.Database.GetDbConnection(), transaction.GetDbTransaction(), [
            new RelationTuple("project", project.Id.ToString(), "admin", "user", manager.UsuarioId.ToString())
        ], [], ct);

        
        await context.SaveChangesAsync(ct);
        await transaction.CommitAsync(ct);
        
        return project.Id;
    }
}