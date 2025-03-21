using System.Reflection;
using Valtuutus.RealWorld.Api.Core;

namespace Valtuutus.RealWorld.Api.Config;

public static class UseCasesSetup
{
    public static void AddUseCases(this IHostBuilder hostBuilder, Assembly assembly)
    {
        hostBuilder.ConfigureServices((services) =>
        {
            var handlers = assembly
                .GetTypes()
                .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                               type.GetInterface(typeof(IUseCase<,>).Name) != null)
                .ToArray();

            foreach (var handler in handlers)
            {
                services.AddScoped(handler);
            }
        });
    }
}