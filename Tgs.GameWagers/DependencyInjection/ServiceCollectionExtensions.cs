using Microsoft.EntityFrameworkCore;
using Tgs.GameWagers.Domains;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddGameWagers(
		this IServiceCollection services,
		Action<IServiceProvider, DbContextOptionsBuilder> tgsConfigure,
		Action<MongoDBOptions, IServiceProvider> mongoDBconfigure)
		=> services
		.AddGameWagersDomains(mongoDBconfigure)
		.AddGameWagersCore()
		.AddGameWagersRepositories(tgsConfigure: tgsConfigure);
}
