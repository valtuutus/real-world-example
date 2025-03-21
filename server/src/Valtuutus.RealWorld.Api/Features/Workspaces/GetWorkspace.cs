using Microsoft.EntityFrameworkCore;
using Valtuutus.RealWorld.Api.Core;
using Valtuutus.RealWorld.Api.Core.Entities;
using Valtuutus.RealWorld.Api.Results;

namespace Valtuutus.RealWorld.Api.Features.Workspaces;

public record GetWorkspace(WorkspaceId Id);

public class GetWorkspaceHandler(Context context) : IUseCase<GetWorkspace, WorkspaceDto>
{
    public async Task<Result<WorkspaceDto>> Handle(GetWorkspace req, CancellationToken ct)
    {
        return await context.Workspaces.Where(x => x.Id == req.Id)
            .Select(x => new WorkspaceDto(x.Id, x.Name))
            .FirstAsync(ct);
    }
}