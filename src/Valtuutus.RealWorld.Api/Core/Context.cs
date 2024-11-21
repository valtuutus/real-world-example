using Microsoft.EntityFrameworkCore;
using Valtuutus.RealWorld.Api.Core.Entities;
using Task = Valtuutus.RealWorld.Api.Core.Entities.Task;

namespace Valtuutus.RealWorld.Api.Core;

public class Context : DbContext
{
    
    public Context(DbContextOptions<Context> options) : base(options) { }
    
    public DbSet<Workspace> Workspaces { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Task> Tasks { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<ProjectStatus> ProjectStatuses { get; set; }
    public DbSet<ProjectUserAssignee> ProjectUserAssignees { get; set; }
    public DbSet<ProjectTeamAssignee> ProjectTeamAssignees { get; set; }
    public DbSet<TaskAssignee> TaskAssignees { get; set; }
    public DbSet<WorkspaceAssignee> WorkspaceAssignees { get; set; }
        
}