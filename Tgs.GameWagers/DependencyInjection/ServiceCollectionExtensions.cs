using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddGameWagers(
		this IServiceCollection services,
		Action<IServiceProvider, DbContextOptionsBuilder> tgsConfigure)
		=> services
		.AddGameWagersCore()
		.AddGameWagersRepositories(tgsConfigure: tgsConfigure);
}
