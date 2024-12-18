using StronglyTypedIds;

namespace Valtuutus.RealWorld.Api.Core.Entities;

public enum ProjectStatusType
{
    Waiting,
    Active,
    Done,
    Archived
}

[StronglyTypedId]
public partial struct ProjectStatusId;

public class ProjectStatus
{
    public required ProjectId ProjectId { get; init; }
    public Project Project { get; init; } = null!;
    public required ProjectStatusId Id { get; init; }
    public required ProjectStatusType Type { get; init; }
    public required string Name { get; init; }
    public required float Order { get; init; }

    public List<Task> Tasks { get; init; } = null!;
}