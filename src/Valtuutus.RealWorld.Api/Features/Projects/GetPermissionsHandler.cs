using Valtuutus.Core.Engines.Check;
using Valtuutus.Lang;
using Valtuutus.RealWorld.Api.Core;
using Valtuutus.RealWorld.Api.Core.Auth;
using Valtuutus.RealWorld.Api.Core.Entities;
using Valtuutus.RealWorld.Api.Results;

namespace Valtuutus.RealWorld.Api.Features.Projects;


public record GetProjectPermission(ProjectId ProjectId);

public class GetPermissionsHandler(ICheckEngine checkEngine, ISessaoManager sessionManager) : IUseCase<GetProjectPermission, Dictionary<string, bool>>
{
    public async Task<Result<Dictionary<string, bool>>> Handle(GetProjectPermission req, CancellationToken ct)
    {
        return await checkEngine.SubjectPermission(new SubjectPermissionRequest
        {
            EntityType = SchemaConstsGen.Project.Name,
            EntityId = req.ProjectId.ToString(),
            SubjectType = SchemaConstsGen.Project.Name,
            SubjectId = sessionManager.UserId.ToString()
        }, ct);
    }
}