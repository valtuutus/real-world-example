using Valtuutus.RealWorld.Api.Core;
using Valtuutus.RealWorld.Api.Core.Entities;
using Valtuutus.RealWorld.Api.Results;

namespace Valtuutus.RealWorld.Api.Features.Projects;

public record GetProjectRequest(ProjectId ProjectId);

public record GetProjectResponse
{
    public ProjectId ProjectId { get; set; }
    public string Name { get; set; }
    public List<ProjectStatusDto> Statuses { get; set; }
    
}

public record ProjectStatusDto
{
    public ProjectStatusId Id { get; set; }
    public ProjectStatusType Type { get; set; }
    public string Name { get; set; }
    public float Order { get; set; }
    
    
}


public class GetProject : IUseCase<GetProjectRequest, GetProjectResponse>
{
    public Task<Result<GetProjectResponse>> Handle(GetProjectRequest req, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}