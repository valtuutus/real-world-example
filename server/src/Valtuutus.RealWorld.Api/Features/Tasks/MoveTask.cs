using Microsoft.EntityFrameworkCore;
using Valtuutus.RealWorld.Api.Core;
using Valtuutus.RealWorld.Api.Core.Entities;
using Valtuutus.RealWorld.Api.Results;

namespace Valtuutus.RealWorld.Api.Features.Tasks;

public record MoveTaskReqBody
{
    public required float NewOrder { get; init; }
    public required ProjectStatusId NewStatusId { get; init; }
}

public record MoveTask : MoveTaskReqBody
{
    public required ProjectId ProjectId { get; init; }
    public required TaskId TaskId { get; init; }
}

public class MoveTaskHandler(Context context) : IUseCase<MoveTask, Unit>
{
    public async Task<Result<Unit>> Handle(MoveTask req, CancellationToken ct)
    {
        await context.Tasks
            .Where(t => t.Id == req.TaskId && t.ProjectId == req.ProjectId)
            .ExecuteUpdateAsync(
            s => s.SetProperty(x => x.ProjectStatusId, req.NewStatusId).SetProperty(x => x.Order, req.NewOrder),
            cancellationToken: ct);
        
        return Result<Unit>.Ok();
    }
}