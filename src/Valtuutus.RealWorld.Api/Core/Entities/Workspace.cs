using StronglyTypedIds;

namespace Valtuutus.RealWorld.Api.Core.Entities;

[StronglyTypedId]
public partial struct WorkspaceId;

public class Workspace
{
    public required WorkspaceId Id { get; init; }
    public required string Name { get; init; }
    public required bool Public { get; init; }
    
    public List<Project> Projects { get; init; } = null!;
    public List<WorkspaceAssignee> Assignees { get; init; } = null!;
    public List<Team> Teams { get; init; } = null!;
}