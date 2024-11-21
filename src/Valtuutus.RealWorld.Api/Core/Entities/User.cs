
using StronglyTypedIds;

namespace Valtuutus.RealWorld.Api.Core.Entities;

[StronglyTypedId]
public partial struct UserId;

public class User
{
    public required UserId Id { get; init; }
    public required string Name { get; init; }

    public List<TaskAssignee> TaskAssignments { get; init; } = [];
    
    public List<WorkspaceAssignee> WorkspaceAssignments { get; init; } = [];
}