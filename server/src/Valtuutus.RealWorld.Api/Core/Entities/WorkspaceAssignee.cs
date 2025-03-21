namespace Valtuutus.RealWorld.Api.Core.Entities;

public enum WorkspaceAssigneeType
{
    Owner,
    Admin,
    Member,
    Guest
}

public static class VttExtensions
{
    public static string ToVttString(this WorkspaceAssigneeType assigneeType) =>
        assigneeType switch
        {
            WorkspaceAssigneeType.Owner => "owner",
            WorkspaceAssigneeType.Admin => "admin",
            WorkspaceAssigneeType.Guest => "guest",
            WorkspaceAssigneeType.Member => "member",
            _ => throw new ArgumentOutOfRangeException(nameof(assigneeType), assigneeType, null)
        };
}

public class WorkspaceAssignee
{
    public required WorkspaceId WorkspaceId { get; init; }
    public Workspace Workspace { get; init; } = null!;
    public required UserId UserId { get; init; }
    public User User { get; init; } = null!;
    public required WorkspaceAssigneeType Type { get; init; }
}