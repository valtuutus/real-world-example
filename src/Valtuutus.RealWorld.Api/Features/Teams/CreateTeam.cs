using Valtuutus.RealWorld.Api.Core;
using Valtuutus.RealWorld.Api.Core.Entities;
using Valtuutus.RealWorld.Api.Results;

namespace Valtuutus.RealWorld.Api.Features.Teams;

public record CreateTeamReqBody
{
    public required string Name { get; init; }
}

public record CreateTeam(WorkspaceId WorkspaceId, CreateTeamReqBody Body);

public class CreateTeamHandler(Context context) : IUseCase<CreateTeam, TeamId>
{
    public async Task<Result<TeamId>> Handle(CreateTeam req, CancellationToken ct)
    {
        var team = new Team()
        {
            Name = req.Body.Name,
            Id = TeamId.New(),
            WorkspaceId = req.WorkspaceId
        };
        
        context.Teams.Add(team);

        await context.SaveChangesAsync(ct);

        return team.Id;
    }
}