using System.Text.Json.Nodes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Valtuutus.Core;
using Valtuutus.Core.Data;
using Valtuutus.Data.Db;
using Valtuutus.Lang;
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

public class AddTeamMemberHandler(Context context, IDbDataWriterProvider dataWriterProvider) : IUseCase<AddTeamMember, Unit>
{
    public async Task<Result<Unit>> Handle(AddTeamMember req, CancellationToken ct)
    {
        var teamMember = new UserTeam()
        {
            UserId = req.Body.UserId,
            TeamId = req.TeamId
        };

        context.Add(teamMember);
        
        var transaction = await context.Database.BeginTransactionAsync(ct);

        await dataWriterProvider.Write(context.Database.GetDbConnection(), transaction.GetDbTransaction(), [
            new RelationTuple(SchemaConstsGen.Team.Name, req.TeamId.ToString(), SchemaConstsGen.Team.Relations.Member, SchemaConstsGen.User.Name, req.Body.UserId.ToString())
        ], [], ct);

        await context.SaveChangesAsync(ct);
        await transaction.CommitAsync(ct);
        
        return Result.Ok(Unit.Value);
    }
}