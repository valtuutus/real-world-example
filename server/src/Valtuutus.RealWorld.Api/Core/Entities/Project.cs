using StronglyTypedIds;

namespace Valtuutus.RealWorld.Api.Core.Entities;

[StronglyTypedId]
public partial struct ProjectId
{
    public static implicit operator ProjectId(Guid id) => new ProjectId(id);

}

public class Project
{
    public ProjectId Id { get; init; } = Guid.CreateVersion7();
    public required WorkspaceId WorkspaceId { get; init; }
    public Workspace Workspace { get; init; } = null!;
    public required string Name { get; init; }
    public List<ProjectStatus> Statuses { get; init; } = null!;
    public List<Task> Tasks { get; init; } = null!;
    
    public List<ProjectUserAssignee> UserAssignees { get; init; } = null!;
    public List<ProjectTeamAssignee> TeamAssignees { get; init; } = null!;
}