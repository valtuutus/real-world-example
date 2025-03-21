using Microsoft.EntityFrameworkCore;
using Valtuutus.Core.Engines.Check;
using Valtuutus.Core.Engines.LookupEntity;
using Valtuutus.Core.Engines.LookupSubject;
using Valtuutus.Lang;
using Valtuutus.RealWorld.Api.Core;
using Valtuutus.RealWorld.Api.Core.Auth;
using Valtuutus.RealWorld.Api.Core.Entities;
using Valtuutus.RealWorld.Api.Results;

namespace Valtuutus.RealWorld.Api.Features.Workspaces;

public record GetWorkspaces
{
    private GetWorkspaces()
    {
    }

    public static GetWorkspaces Instance { get; } = new();
}

public record WorkspaceDto(WorkspaceId Id, string Name);

public class GetWorkspacesHandler(
    ILookupEntityEngine lookupEngine, 
    ISessionManager sessionManager,
    Context context) : IUseCase<GetWorkspaces, WorkspaceDto[]>
{
    public async Task<Result<WorkspaceDto[]>> Handle(GetWorkspaces req, CancellationToken ct)
    {
        var workspaces = await lookupEngine.LookupEntity(new ()
        {
            EntityType = SchemaConstsGen.Workspace.Name,
            SubjectType = SchemaConstsGen.User.Name,
            SubjectId = sessionManager.UserId.ToString(),
            Permission = SchemaConstsGen.Workspace.Permissions.View,
            Depth = 10
        }, ct);

        var workspacesIds = workspaces.Select(WorkspaceId.Parse);
        
        return await context.Workspaces.Where(x => workspacesIds.Contains(x.Id))
            .Select(x => new WorkspaceDto(x.Id, x.Name))
            .ToArrayAsync(ct);
    }
}