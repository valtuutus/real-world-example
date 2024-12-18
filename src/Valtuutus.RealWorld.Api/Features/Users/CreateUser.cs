using Valtuutus.RealWorld.Api.Core;
using Valtuutus.RealWorld.Api.Core.Entities;
using Valtuutus.RealWorld.Api.Results;

namespace Valtuutus.RealWorld.Api.Features.Users;

public record CreateUser
{
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string Name { get; init; }
}

public class CreateUserHandler(Context context) : IUseCase<CreateUser, Unit>
{
    public async Task<Result<Unit>> Handle(CreateUser req, CancellationToken ct)
    {
        var user = new User()
        {
            Name = req.Name,
            Id = UserId.New(),
            Email = req.Email.ToLower(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password),
        };
        
        context.Users.Add(user);
        
        await context.SaveChangesAsync(ct);
        
        return Result.Ok(Unit.Value);
    }
}