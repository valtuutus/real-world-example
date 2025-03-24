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
    
    public record View : WorkspaceRequirement, IWithPermissionRequirement
    {
        private View()
        {
        }

        public static View Instance { get; } = new();
        public string Permission { get; } = SchemaConstsGen.Workspace.Permissions.View;
    }
    
    public record AssignUser : WorkspaceRequirement, IWithPermissionRequirement
    {
        private AssignUser()
        {
        }

        public static AssignUser Instance { get; } = new();
        public string Permission { get; } = SchemaConstsGen.Workspace.Permissions.AssignUser;
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

public static class ProjectRequirements
{
    public abstract record ProjectRequirement : IWithEntityTypeRequirement, IAuthorizationRequirement
    {
        public string EntityType { get; } = SchemaConstsGen.Project.Name;
    }
    
    public record View : ProjectRequirement, IWithPermissionRequirement
    {
        private View()
        {
        }

        public static View Instance { get; } = new();
        public string Permission { get; } = SchemaConstsGen.Project.Permissions.View;
    }
    
    public record Edit : ProjectRequirement, IWithPermissionRequirement
    {
        private Edit()
        {
        }

        public static Edit Instance { get; } = new();
        public string Permission { get; } = SchemaConstsGen.Project.Permissions.Edit;
    }
    
    public record CreateTask : ProjectRequirement, IWithPermissionRequirement
    {
        private CreateTask()
        {
        }

        public static CreateTask Instance { get; } = new();
        public string Permission { get; } = SchemaConstsGen.Project.Permissions.CreateTask;
    }
}


