using System.Data;
using Microsoft.EntityFrameworkCore;
using Tgs.GameWagers.Core;
using Tgs.GameWagers.Repositories;
using Tgs.GameWagers.Repositories.Entities;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddGameWagersRepositories(
		this IServiceCollection services,
		Action<IServiceProvider, DbContextOptionsBuilder> tgsConfigure)
		=> services
		.AddDbContext<TgsContext>(tgsConfigure)
		.AddScoped<IDbConnection>((sp) => sp.GetRequiredService<TgsContext>().Database.GetDbConnection())
		.AddTransient<IGameWagerFirstRepository, GameWagerFirstRepository>()
		.AddTransient<IGameWagerRepository, GameWagerRepository>()
		.AddTransient<IGameWagerPlayerAccumRepository, GameWagerPlayerAccumRepository>()
		.AddTransient<IGameWagerStatCloseRepository, GameWagerStatCloseRepository>()
		.AddTransient<IGameWagerStatHourlyCloseRepository, GameWagerStatHourlyCloseRepository>()
		.AddTransient<IGameWagerStatDailyCloseRepository, GameWagerStatDailyCloseRepository>();
}
