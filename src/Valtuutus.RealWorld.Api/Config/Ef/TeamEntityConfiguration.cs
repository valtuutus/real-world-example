using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Valtuutus.RealWorld.Api.Core.Entities;

namespace Valtuutus.RealWorld.Api.Config.Ef;

public class TeamEntityConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).
            HasConversion<TeamId.EfCoreValueConverter>();
        
    }
}