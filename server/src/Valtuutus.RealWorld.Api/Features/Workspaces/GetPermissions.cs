using Valtuutus.Core.Engines.Check;
using Valtuutus.Lang;
using Valtuutus.RealWorld.Api.Core;
using Valtuutus.RealWorld.Api.Core.Auth;
using Valtuutus.RealWorld.Api.Core.Entities;
using Valtuutus.RealWorld.Api.Results;

namespace Valtuutus.RealWorld.Api.Features.Workspaces;

public record GetWorkspacePermission(WorkspaceId WorkspaceId);

public class GetPermissionsHandler(ICheckEngine checkEngine, ISessionManager sessionManager) : IUseCase<GetWorkspacePermission, Dictionary<string, bool>>
{
    public async Task<Result<Dictionary<string, bool>>> Handle(GetWorkspacePermission req, CancellationToken ct)
    {
        return await checkEngine.SubjectPermission(new SubjectPermissionRequest
        {
            EntityType = SchemaConstsGen.Workspace.Name,
            EntityId = req.WorkspaceId.ToString(),
            SubjectType = SchemaConstsGen.Project.Name,
            SubjectId = sessionManager.UserId.ToString()
        }, ct);
    }
}