namespace Valtuutus.RealWorld.Api.Core.Entities;

public class TaskAssignee
{
    public required TaskId TaskId { get; init; }
    public Task Task { get; init; } = null!;
    public required UserId UserId { get; init; }
    public User User { get; init; } = null!;
}