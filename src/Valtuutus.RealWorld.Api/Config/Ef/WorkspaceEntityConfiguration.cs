using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Valtuutus.RealWorld.Api.Core.Entities;

namespace Valtuutus.RealWorld.Api.Config.Ef;

public class WorkspaceEntityConfiguration : IEntityTypeConfiguration<Workspace>
{
    public void Configure(EntityTypeBuilder<Workspace> builder)
    {
        builder.HasKey(x => x.Id);
        
        // builder.Property(x => x.Id)
        //     .HasConversion<WorkspaceId.>()
        //
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);
    }
}