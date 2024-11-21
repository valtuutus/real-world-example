using StronglyTypedIds;

namespace Valtuutus.RealWorld.Api.Core.Entities;

[StronglyTypedId]
public partial struct ProjectId;

public class Project
{
    public required WorkspaceId WorkspaceId { get; init; }
    public Workspace Workspace { get; init; } = null!;
    public required ProjectId Id { get; init; }
    public required string Name { get; init; }
    public List<ProjectStatus> Statuses { get; init; } = null!;
    public List<Task> Tasks { get; init; } = null!;
    
    public List<ProjectUserAssignee> UserAssignees { get; init; } = null!;
    public List<ProjectTeamAssignee> TeamAssignees { get; init; } = null!;
}