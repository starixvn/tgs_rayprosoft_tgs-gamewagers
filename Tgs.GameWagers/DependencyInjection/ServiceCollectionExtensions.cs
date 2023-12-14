using Microsoft.EntityFrameworkCore;
using Tgs.GameWagers;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGameWagers(
        this IServiceCollection services,
        Action<IServiceProvider, DbContextOptionsBuilder> tgsConfigure,
        Action<MongoDBOptions, IServiceProvider> mongoDBconfigure)
        => services
        .AddOptions<MongoDBOptions>()
        .Configure(mongoDBconfigure)
        .Services
        .AddSingleton<MongoDBConnectionManager>()
        .AddGameWagersDomains((sp) => sp.GetRequiredService<MongoDBConnectionManager>().MongoClient)
        .AddGameWagersCore()
        .AddGameWagersRepositories(tgsConfigure: tgsConfigure);
}
