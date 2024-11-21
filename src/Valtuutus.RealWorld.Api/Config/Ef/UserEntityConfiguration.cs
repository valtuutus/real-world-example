using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Valtuutus.RealWorld.Api.Core.Entities;

namespace Valtuutus.RealWorld.Api.Config.Ef;

public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion<UserId.EfCoreValueConverter>();

        builder.HasMany(x => x.TaskAssignments)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);
        
        builder.HasMany(x => x.WorkspaceAssignments)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);
    }
}