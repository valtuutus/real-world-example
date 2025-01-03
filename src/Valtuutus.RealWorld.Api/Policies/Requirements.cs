using Microsoft.AspNetCore.Authorization;
using Valtuutus.Lang;

namespace Valtuutus.RealWorld.Api.Policies;

public interface IWithEntityTypeRequirement
{
    public string EntityType { get; }
}
public interface IWithPermissionRequirement
{
    public string Permission { get; }
}

public static class WorkspaceRequirements {
    public abstract record WorkspaceRequirement : IWithEntityTypeRequirement, IAuthorizationRequirement
    {
        public string EntityType { get; } = SchemaConstsGen.Workspace.Name;
    }
    public record CreateProject : WorkspaceRequirement, IWithPermissionRequirement
    {
        private CreateProject()
        {
        }

        public static CreateProject Instance { get; } = new();
        public string Permission { get; } = SchemaConstsGen.Workspace.Permissions.CreateProject;
    }
}



