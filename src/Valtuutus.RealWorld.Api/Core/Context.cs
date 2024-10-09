using Microsoft.EntityFrameworkCore;

namespace Valtuutus.RealWorld.Api.Core;

public class Context : DbContext
{
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
    }
}