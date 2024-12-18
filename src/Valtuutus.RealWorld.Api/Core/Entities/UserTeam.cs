namespace Valtuutus.RealWorld.Api.Core.Entities;

public class UserTeam
{
    public required UserId UserId { get; init; }
    public User User { get; init; }
    public required TeamId TeamId { get; init; }
    public Team Team { get; init; }
}