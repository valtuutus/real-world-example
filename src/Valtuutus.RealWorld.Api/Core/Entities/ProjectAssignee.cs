namespace Valtuutus.RealWorld.Api.Core.Entities;

public enum ProjectAssigneeType
{
    Admin,
    Member,
    Guest
}

public class ProjectUserAssignee
{
    public required ProjectId ProjectId { get; init; }
    public Project Project { get; init; } = null!;
    public required UserId UserId { get; init; }
    public User User { get; init; } = null!;
    public required ProjectAssigneeType Type { get; init; }
}

public class ProjectTeamAssignee
{
    public required ProjectId ProjectId { get; init; }
    public Project Project { get; init; } = null!;
    public required TeamId TeamId { get; init; }
    public Team Team { get; init; } = null!;
    public required ProjectAssigneeType Type { get; init; }
}