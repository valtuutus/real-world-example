using System.Text.Json.Nodes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Valtuutus.Core;
using Valtuutus.Data.Db;
using Valtuutus.Lang;
using Valtuutus.RealWorld.Api.Core;
using Valtuutus.RealWorld.Api.Core.Entities;
using Valtuutus.RealWorld.Api.Results;

namespace Valtuutus.RealWorld.Api.Features.Workspaces;

public record AssignUserBodyReq
{
    public required UserId UserId { get; init; }
    public required WorkspaceAssigneeType Type { get; init; }
}

public record AssignUser
{
    public required WorkspaceId WorkspaceId { get; init; }
    public required AssignUserBodyReq Body { get; init; }
}

public class AssignUserToWorkspaceHandler(Context context, IDbDataWriterProvider dataWriterProvider) : IUseCase<AssignUser, Unit>
{
    public async Task<Result<Unit>> Handle(AssignUser req, CancellationToken ct)
    {
        var workspaceAssignee = new WorkspaceAssignee
        {
            UserId = req.Body.UserId,
            Type = req.Body.Type,
            WorkspaceId = req.WorkspaceId
        };
        
        context.WorkspaceAssignees.Add(workspaceAssignee);

        var transaction = await context.Database.BeginTransactionAsync(ct);

        await dataWriterProvider.Write(context.Database.GetDbConnection(), transaction.GetDbTransaction(), [
            new RelationTuple(SchemaConstsGen.Workspace.Name, req.WorkspaceId.ToString(), req.Body.Type.ToVttString(), SchemaConstsGen.User.Name, req.Body.UserId.ToString())
        ], [], ct);
        await context.SaveChangesAsync(ct);
        
        await transaction.CommitAsync(ct);

        return Result.Ok();
    }
}