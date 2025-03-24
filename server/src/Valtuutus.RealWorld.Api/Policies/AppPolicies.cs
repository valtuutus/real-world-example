namespace Valtuutus.RealWorld.Api.Policies;

public static class AppPolicies
{
    public static class Workspace
    {
        public const string View = "Workspace.View";
        public const string AssignUser = "Workspace.AssignUser";
        public const string CreateProject = "Workspace.CreateProject";
    }

    public static class Project
    {
        public const string View = "Project.View";
        public const string AssignUser = "Project.AssignUser";
        public const string CreateTask = "Project.CreateTask";
        public const string Edit  = "Project.Edit";
    }
}