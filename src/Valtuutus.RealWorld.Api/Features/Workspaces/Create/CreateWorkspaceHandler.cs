using MediatR;
using Valtuutus.RealWorld.Api.Core;
using Valtuutus.RealWorld.Api.Core.Entities;
using Valtuutus.RealWorld.Api.Results;

namespace Valtuutus.RealWorld.Api.Features.Workspaces.Create;


public record CreateWorkspaceRequest(string Name, bool Public) : IRequest<Result<WorkspaceId>>;

public class CreateWorkspaceHandler : IRequestHandler<CreateWorkspaceRequest, Result<WorkspaceId>>
{
    private readonly Context _context;
    public CreateWorkspaceHandler(Context context)
    {
        _context = context;
    }
    public async Task<Result<WorkspaceId>> Handle(CreateWorkspaceRequest command, CancellationToken cancellationToken)
    {
        var workspace = new Workspace
        {
            Name = command.Name,
            Public = command.Public,
        };
        _context.Add(workspace);
        
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Ok(workspace.Id);

    }
}