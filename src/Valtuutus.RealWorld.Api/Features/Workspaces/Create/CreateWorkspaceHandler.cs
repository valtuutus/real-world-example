using System.Text.Json.Nodes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Valtuutus.Core;
using Valtuutus.Data.Db;
using Valtuutus.RealWorld.Api.Core;
using Valtuutus.RealWorld.Api.Core.Auth;
using Valtuutus.RealWorld.Api.Core.Entities;
using Valtuutus.RealWorld.Api.Results;

namespace Valtuutus.RealWorld.Api.Features.Workspaces.Create;

public record CreateWorkspaceRequest(string Name, bool Public);

public class CreateWorkspaceHandler(
    Context context,
    ISessaoManager sessaoManager,
    IDbDataWriterProvider dataWriterProvider) : IUseCase<CreateWorkspaceRequest, WorkspaceId>
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

        var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        await dataWriterProvider.Write(context.Database.GetDbConnection(), transaction.GetDbTransaction(), [
            new RelationTuple("workspace", workspace.Id.ToString(), "owner", "user", sessaoManager.UsuarioId.ToString())
        ], [
            new AttributeTuple("workspace", workspace.Id.ToString(), "public", JsonValue.Create(command.Public))
        ], cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);
        return Result.Ok(workspace.Id);
    }
}