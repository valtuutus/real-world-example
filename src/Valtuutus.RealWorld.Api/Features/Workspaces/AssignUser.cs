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

public class AssignUserToWorkspaceHandler(Context context) : IUseCase<AssignUser, Unit>
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

        await context.SaveChangesAsync(ct);

        return Result.Ok();
    }
}