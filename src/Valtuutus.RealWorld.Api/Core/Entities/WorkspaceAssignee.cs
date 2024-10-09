namespace Valtuutus.RealWorld.Api.Core.Entities;

public enum WorkspaceAssigneeType
{
    Admin,
    Member,
    Guest
}

public class WorkspaceAssignee
{
    public required WorkspaceId WorkspaceId { get; init; }
    public Workspace Workspace { get; init; } = null!;
    public required UserId UserId { get; init; }
    public User User { get; init; } = null!;
    public required WorkspaceAssigneeType Type { get; init; }
}