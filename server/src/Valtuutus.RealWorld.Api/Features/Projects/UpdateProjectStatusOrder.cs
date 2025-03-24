using Microsoft.EntityFrameworkCore;
using Valtuutus.RealWorld.Api.Core;
using Valtuutus.RealWorld.Api.Core.Entities;
using Valtuutus.RealWorld.Api.Results;

namespace Valtuutus.RealWorld.Api.Features.Projects;

public record UpdateProjectStatusOrder(
    ProjectId ProjectId, ProjectStatusId StatusId, float Order);

public class UpdateProjectStatusOrderHandler(Context context) : IUseCase<UpdateProjectStatusOrder, bool>
{
    public async Task<Result<bool>> Handle(UpdateProjectStatusOrder req, CancellationToken ct)
    {
        return await context.ProjectStatuses
            .Where(x => x.ProjectId == req.ProjectId && x.Id == req.StatusId)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.Order, req.Order), ct) > 0;
    }
}