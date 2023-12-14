using Microsoft.Extensions.Options;
using Tgs.GameWagers.Client;
using Tgs.VipLevelCommand.Client;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGameWagersClient(
        this IServiceCollection services,
        Action<GameWagersClientOptions, IServiceProvider> configure)
        => services
        .AddOptions<GameWagersClientOptions>()
        .Configure(configure)
        .Services
        .AddHttpClient<IGameWagersClient, GameWagersClient>()
        .ConfigureHttpClient((sp, httpClient) =>
        {
            var options = sp.GetRequiredService<IOptions<GameWagersClientOptions>>().Value;
            httpClient.BaseAddress = options.BaseUrl;
        })
        .Services;
}
