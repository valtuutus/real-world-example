using Valtuutus.RealWorld.Api.Core;
using Valtuutus.RealWorld.Api.Core.Auth;
using Valtuutus.RealWorld.Api.Core.Entities;
using Valtuutus.RealWorld.Api.Results;

namespace Valtuutus.RealWorld.Api.Features.Workspaces.Create;

public record CreateWorkspaceRequest(string Name, bool Public);

public class CreateWorkspaceHandler(Context context, ISessaoManager sessaoManager) : IUseCase<CreateWorkspaceRequest, WorkspaceId>
{
    public async Task<Result<WorkspaceId>> Handle(CreateWorkspaceRequest command, CancellationToken cancellationToken)
    {
        var workspace = new Workspace
        {
            Name = command.Name,
            Public = command.Public,
        };
        context.Add(workspace);

        context.Add(new WorkspaceAssignee()
        {
            WorkspaceId = workspace.Id,
            UserId = sessaoManager.UsuarioId,
            Type = WorkspaceAssigneeType.Owner
        });

        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok(workspace.Id);
    }
}