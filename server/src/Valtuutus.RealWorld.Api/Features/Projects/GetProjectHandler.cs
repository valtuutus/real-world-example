using Microsoft.EntityFrameworkCore;
using Valtuutus.RealWorld.Api.Core;
using Valtuutus.RealWorld.Api.Core.Entities;
using Valtuutus.RealWorld.Api.Results;

namespace Valtuutus.RealWorld.Api.Features.Projects;

public record GetProjectRequest(ProjectId ProjectId);

public record GetProjectResponse
{
    public required ProjectId Id { get; set; }
    public required string Name { get; set; }
    public List<ProjectStatusDto> Statuses { get; set; } = new();
}

public record ProjectStatusDto
{
    public required ProjectStatusId Id { get; set; }
    public required ProjectStatusType Type { get; set; }
    public required string Name { get; set; }
    public required float Order { get; set; }
    
    
}


public class GetProjectHandler(Context context) : IUseCase<GetProjectRequest, GetProjectResponse>
{
    public async Task<Result<GetProjectResponse>> Handle(GetProjectRequest req, CancellationToken ct)
    {
        var project = await context.Projects
            .Where(x => x.Id == req.ProjectId)
            .Select(x => new GetProjectResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Statuses = x.Statuses.Select(y => new ProjectStatusDto
                    {
                        Id = y.Id,
                        Type = y.Type,
                        Order = y.Order,
                        Name = y.Name,
                    }).OrderBy(y => y.Order).ToList()
                }
            ).FirstOrDefaultAsync(ct);

        if (project is null) return Result.NotFound();
        return project;
    }
}