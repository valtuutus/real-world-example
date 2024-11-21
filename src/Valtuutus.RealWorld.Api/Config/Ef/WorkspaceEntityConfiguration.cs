using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Valtuutus.RealWorld.Api.Core.Entities;

namespace Valtuutus.RealWorld.Api.Config.Ef;

public class WorkspaceEntityConfiguration : IEntityTypeConfiguration<Workspace>
{
    public void Configure(EntityTypeBuilder<Workspace> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion<WorkspaceId.EfCoreValueConverter>();
        
        builder.HasMany(x => x.Projects)
            .WithOne(x => x.Workspace)
            .HasForeignKey(x => x.WorkspaceId);
        
        builder.HasMany(x => x.Assignees)
            .WithOne(x => x.Workspace)
            .HasForeignKey(x => x.WorkspaceId);
        
        builder.HasMany(x => x.Teams)
            .WithOne(x => x.Workspace)
            .HasForeignKey(x => x.WorkspaceId);
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);
    }

}