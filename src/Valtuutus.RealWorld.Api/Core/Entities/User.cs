using Vogen;

namespace Valtuutus.RealWorld.Api.Core.Entities;

[ValueObject<Guid>]
public partial struct UserId;

public class User
{
    public required UserId Id { get; init; }
    public required string Name { get; init; }
}