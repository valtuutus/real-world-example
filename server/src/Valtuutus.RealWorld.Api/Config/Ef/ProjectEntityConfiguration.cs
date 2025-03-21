using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Valtuutus.RealWorld.Api.Core.Entities;

namespace Valtuutus.RealWorld.Api.Config.Ef;

public class ProjectEntityConfiguration : IEntityTypeConfiguration<Project>, IEntityTypeConfiguration<ProjectTeamAssignee>, IEntityTypeConfiguration<ProjectUserAssignee>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion<ProjectId.EfCoreValueConverter>();

        builder.HasMany(x => x.TeamAssignees)
            .WithOne(x => x.Project)
            .HasForeignKey(x => x.ProjectId);
        
        builder.HasMany(x => x.UserAssignees)
            .WithOne(x => x.Project)
            .HasForeignKey(x => x.ProjectId);
        
        builder.HasMany(x => x.Statuses)
            .WithOne(x => x.Project)
            .HasForeignKey(x => x.ProjectId);
    }

    public void Configure(EntityTypeBuilder<ProjectTeamAssignee> builder)
    {
        builder.HasKey(x => new { x.ProjectId, x.TeamId });
        builder.HasOne(x => x.Team)
            .WithMany()
            .HasForeignKey(x => x.TeamId);
    }

    public void Configure(EntityTypeBuilder<ProjectUserAssignee> builder)
    {
        builder.HasKey(x => new { x.ProjectId, x.UserId });

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId);
    }
}