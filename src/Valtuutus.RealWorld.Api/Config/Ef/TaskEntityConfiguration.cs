using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Valtuutus.RealWorld.Api.Core.Entities;
using Task = Valtuutus.RealWorld.Api.Core.Entities.Task;

namespace Valtuutus.RealWorld.Api.Config.Ef;

public class TaskEntityConfiguration : IEntityTypeConfiguration<Task>
{
    public void Configure(EntityTypeBuilder<Task> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion<TaskId.EfCoreValueConverter>();

        builder.HasMany(x => x.Assignees)
            .WithOne(x => x.Task)
            .HasForeignKey(x => x.TaskId);
    }
}