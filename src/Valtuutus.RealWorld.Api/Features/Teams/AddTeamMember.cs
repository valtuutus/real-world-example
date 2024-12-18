using Valtuutus.RealWorld.Api.Core;
using Valtuutus.RealWorld.Api.Core.Entities;
using Valtuutus.RealWorld.Api.Results;

namespace Valtuutus.RealWorld.Api.Features.Teams;

public record AddTeamMemberReqBody
{
    public required UserId UserId { get; init; }
}

public record AddTeamMember
{
    public required TeamId TeamId { get; init; }
    public required AddTeamMemberReqBody Body { get; init; }
}

public class AddTeamMemberHandler(Context context) : IUseCase<AddTeamMember, Unit>
{
    public async Task<Result<Unit>> Handle(AddTeamMember req, CancellationToken ct)
    {
        var teamMember = new UserTeam()
        {
            UserId = req.Body.UserId,
            TeamId = req.TeamId
        };

        context.Add(teamMember);

        await context.SaveChangesAsync(ct);
        
        return Result.Ok(Unit.Value);
    }
}