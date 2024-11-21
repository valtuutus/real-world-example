
using StronglyTypedIds;

namespace Valtuutus.RealWorld.Api.Core.Entities;

[StronglyTypedId]
public partial struct TeamId;


public class Team
{
    public required WorkspaceId WorkspaceId { get; init; }
    public Workspace Workspace { get; init; } = null!;
    public required TeamId Id { get; init; }
    public required string Name { get; init; }
    
    public List<User> Users { get; init; } = null!;
}