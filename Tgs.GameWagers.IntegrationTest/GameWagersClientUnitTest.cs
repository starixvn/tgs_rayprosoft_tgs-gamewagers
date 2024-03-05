using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Tgs.GameWagers.Common;
using Tgs.GameWagers.Core;
using Tgs.GameWagers.Core.ApplicationServices;
using Tgs.GameWagers.Core.Models;
using Tgs.GameWagers.Repositories.Entities;
using Tgs.ServiceUtility;
using Tgs.VipLevelCommand.IntegrationTest;

namespace Tgs.GameWagers.Client.IntegrationTest;

public class GameWagersClientUnitTest
{
	[Fact]
	public async Task Brand_Wager_Game_Stats_Query()
	{
		var brandCode = "PB";
		var startOn = DateTimeOffset.UtcNow.AddHours(-1);
		var endOn = DateTimeOffset.UtcNow;
		var sort = BrandWagerGameMinsStatsSort.WinLoss;
		var isAscending = false;
		var page = 1;
		var pageSize = 1000;

		var web = new GameWagersClientServiceApplication(builder => _ = builder.ConfigureServices(services =>
		{
			var fakeRepository = Substitute.For<IGameWagerStatCloseRepository>();
			_ = fakeRepository.QueryGroupByGameAsync(
				brandCode: Arg.Is(brandCode),
				startOn: Arg.Is(startOn),
				endOn: Arg.Is(endOn),
				sort: Arg.Is(sort),
				isAscending: Arg.Is(isAscending),
				offset: Arg.Is(page - 1),
				limit: Arg.Is(pageSize),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(Enumerable.Empty<GameWagerStatCloseGroupByGame>().ToAsyncEnumerable());

			_ = services.AddSingleton(fakeRepository);
		}));
		var httpClient = web.CreateClient();

		var sut = new GameWagersClient(httpClient);

		_ = await sut.BrandWagerGameStatsQueryAsync(
			brandCode: brandCode,
			startOn: startOn,
			endOn: endOn,
			sort: sort,
			isAscending: isAscending,
			page: page,
			pageSize: pageSize)
			.ToArrayAsync()
			?? Enumerable.Empty<BrandWagerGameStatsInfo>();
	}

	[Fact]
	public async Task Brand_Wager_Player_Stats_Query()
	{
		var brandCode = "PB";
		var startOn = DateTimeOffset.UtcNow.AddHours(-1);
		var endOn = DateTimeOffset.UtcNow;
		var sort = BrandWagerPlayerMinsStatsSort.WinLoss;
		var isAscending = false;
		var page = 1;
		var pageSize = 1000;

		var web = new GameWagersClientServiceApplication(builder => _ = builder.ConfigureServices(services =>
		{
			var fakeRepository = Substitute.For<IGameWagerStatCloseRepository>();
			_ = fakeRepository.QueryGroupByPlayerAsync(
				brandCode: Arg.Is(brandCode),
				startOn: Arg.Is(startOn),
				endOn: Arg.Is(endOn),
				sort: Arg.Is(sort),
				isAscending: Arg.Is(isAscending),
				offset: Arg.Is(page - 1),
				limit: Arg.Is(pageSize),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(Enumerable.Empty<GameWagerStatCloseGroupByPlayer>().ToAsyncEnumerable());

			_ = services.AddSingleton(fakeRepository);
		}));
		var httpClient = web.CreateClient();

		var sut = new GameWagersClient(httpClient);

		_ = await sut
			.BrandWagerPlayerStatsQueryAsync(
			brandCode: brandCode,
			startOn: startOn,
			endOn: endOn,
			sort: sort,
			isAscending: isAscending,
			page: page,
			pageSize: pageSize)
			.ToArrayAsync()
			?? Enumerable.Empty<BrandWagerPlayerStatsInfo>();
	}

	[Fact]
	public async Task Brand_Wager_Player_Stats_Player_Query()
	{
		var brandCode = "PB";
		var startOn = DateTimeOffset.UtcNow.AddHours(-1);
		var endOn = DateTimeOffset.UtcNow;
		var playerCodes = new string[] { "test1", "test2" };

		var web = new GameWagersClientServiceApplication(builder => _ = builder.ConfigureServices(services =>
		{
			var fakeRepository = Substitute.For<IGameWagerStatCloseRepository>();
			_ = fakeRepository.QueryFilterPlayerGroupByPlayerAsync(
				brandCode: Arg.Is(brandCode),
				startOn: Arg.Is(startOn),
				endOn: Arg.Is(endOn),
				playerCodes: Arg.Is(playerCodes),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(Enumerable.Empty<GameWagerStatCloseGroupByPlayer>().ToAsyncEnumerable());

			_ = services.AddSingleton(fakeRepository);
		}));
		var httpClient = web.CreateClient();

		var sut = new GameWagersClient(httpClient);

		_ = await sut
			.BrandWagerPlayerStatsPlayerQueryAsync(
			brandCode: brandCode,
			startOn: startOn,
			endOn: endOn,
			playerCodes: playerCodes)
			.ToArrayAsync()
			?? Enumerable.Empty<BrandWagerPlayerStatsInfo>();
	}

	[Fact]
	public async Task Game_Wager_Stat_Ge_Player_Accum()
	{
		var playerCode = "4ucmqplxb029";

		var web = new GameWagersClientServiceApplication(builder => _ = builder.ConfigureServices(services =>
		{
			var fakeService = Substitute.For<IGetService<GameWagerStatPlayerAccumQuery, GameWagerStatPlayerAccumInfo>>();

			var query = new GameWagerStatPlayerAccumQuery(
				PlayerCode: playerCode);
			var info = new GameWagerStatPlayerAccumInfo(
				PlayerCode: playerCode,
				Count: 100,
				Bet: 10000,
				ValidBet: 10000,
				Settle: 20000,
				Commission: 0,
				Turnover: 10000,
				WinLoss: 10000);

			_ = fakeService.GetAsync(
				query: Arg.Is(query),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(info);

			_ = services.AddSingleton(fakeService);
		}));
		var httpClient = web.CreateClient();

		var sut = new GameWagersClient(httpClient);

		var actual = await sut.GameWagerStatGetPlayerAccumAsync(playerCode);

		Assert.NotNull(actual);
		Assert.Equal(100, actual.Count);
		Assert.Equal(10000.00m, actual.Bet);
		Assert.Equal(10000.00m, actual.ValidBet);
		Assert.Equal(20000.00m, actual.Settle);
		Assert.Equal(0.00m, actual.Commission);
		Assert.Equal(10000.00m, actual.Turnover);
		Assert.Equal(10000.00m, actual.WinLoss);
	}
}
