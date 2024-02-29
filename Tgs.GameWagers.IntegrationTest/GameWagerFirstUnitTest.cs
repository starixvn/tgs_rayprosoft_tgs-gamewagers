using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Tgs.GameWagers.Client;
using Tgs.GameWagers.Core;
using Tgs.GameWagers.Core.Models;
using Tgs.VipLevelCommand.IntegrationTest;

namespace Tgs.GameWagers.IntegrationTest;

public class GameWagerFirstUnitTest
{
	[Fact]
	public async Task GameWagerFirst_WagerId_CreatedOn_Get()
	{
		var wagerId = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		var createdOn = DateTimeOffset.Parse("2024-02-27T16:29:10.681+08:00");

		var result = new GameWagerFirstModel(
			WagerId: wagerId,
			GameProviderId: Guid.NewGuid(),
			GameProviderConfigurationId: Guid.NewGuid(),
			GameId: 1,
			ResellerId: Guid.NewGuid(),
			LicenseeId: Guid.NewGuid(),
			BrandId: Guid.NewGuid(),
			PlayerCode: "4ucmqplxb029",
			CurrencyCode: "CNY",
			WalletId: Guid.NewGuid(),
			WagerState: Common.Enums.WagerState.Win,
			WagerType: Common.Enums.WagerType.Normal,
			ClosedState: Common.Enums.ClosedState.Closed,
			VoidState: Common.Enums.VoidState.None,
			CancelState: Common.Enums.CancelState.None,
			FreeGameState: Common.Enums.FreeGameState.None,
			GambleState: Common.Enums.GambleState.None,
			CashoutState: Common.Enums.CashoutState.None,
			ResettleState: Common.Enums.ResettleState.None,
			ExternalRoundId: "1762394359405350912",
			ExternalWagerId: "PG-1762394359405350912",
			ExternalCreatedOn: DateTimeOffset.Parse("2024-02-27T16:28:53.000+08:00"),
			ExternalClosedOn: DateTimeOffset.Parse("2024-02-27T16:28:53.000+08:00"),
			ExternalPayoffOn: DateTimeOffset.Parse("2024-02-27T16:28:53.000+08:00"),
			ExternalResettleOn: DateTimeOffset.Parse("1970-01-01T08:00:00.000+08:00"),
			CurrencyRatio: 1.0m,
			Bet: 0.5m,
			ValidBet: 0.5m,
			Settle: 2.8m,
			Commission: 0.0m,
			Turnover: 0.5m,
			WinLoss: 2.3m,
			CreatedOn: createdOn,
			ClosedOn: DateTimeOffset.Parse("2024-02-27T16:29:11.267+08:00"),
			UpdatedOn: DateTimeOffset.Parse("2024-02-27T16:29:11.267+08:00"));

		var web = new GameWagersClientServiceApplication(builder => _ = builder.ConfigureServices(services =>
		{
			var fakeRepository = Substitute.For<IGameWagerFirstRepository>();

			_ = fakeRepository.GetAsync(
				wagerId: Arg.Is(wagerId),
				createdOn: Arg.Is(createdOn),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(result);

			_ = services.AddSingleton(fakeRepository);
		}));
		var httpClient = web.CreateDefaultClient();

		var client = new GameWagersClient(httpClient);

		var actual = await client.GetAsync(wagerId: wagerId, createdOn: createdOn);

		Assert.NotNull(actual);
		Assert.Equal(result.WagerId, actual.WagerId);
		Assert.Equal(result.GameProviderId, actual.GameProviderId);
		Assert.Equal(result.GameProviderConfigurationId, actual.GameProviderConfigurationId);
		Assert.Equal(result.GameId, actual.GameId);
		Assert.Equal(result.ResellerId, actual.ResellerId);
		Assert.Equal(result.LicenseeId, actual.LicenseeId);
		Assert.Equal(result.BrandId, actual.BrandId);
		Assert.Equal(result.PlayerCode, actual.PlayerCode);
		Assert.Equal(result.CurrencyCode, actual.CurrencyCode);
		Assert.Equal(result.WalletId, actual.WalletId);
		Assert.Equal(result.WagerState, actual.WagerState);
		Assert.Equal(result.WagerType, actual.WagerType);
		Assert.Equal(result.ClosedState, actual.ClosedState);
		Assert.Equal(result.VoidState, actual.VoidState);
		Assert.Equal(result.CancelState, actual.CancelState);
		Assert.Equal(result.FreeGameState, actual.FreeGameState);
		Assert.Equal(result.GambleState, actual.GambleState);
		Assert.Equal(result.CashoutState, actual.CashoutState);
		Assert.Equal(result.ResettleState, actual.ResettleState);
		Assert.Equal(result.ExternalRoundId, actual.ExternalRoundId);
		Assert.Equal(result.ExternalWagerId, actual.ExternalWagerId);
		Assert.Equal(result.ExternalCreatedOn, actual.ExternalCreatedOn);
		Assert.Equal(result.ExternalClosedOn, actual.ExternalClosedOn);
		Assert.Equal(result.ExternalPayoffOn, actual.ExternalPayoffOn);
		Assert.Equal(result.ExternalResettleOn, actual.ExternalResettleOn);
		Assert.Equal(result.CurrencyRatio, actual.CurrencyRatio);
		Assert.Equal(result.Bet, actual.Bet);
		Assert.Equal(result.ValidBet, actual.ValidBet);
		Assert.Equal(result.Settle, actual.Settle);
		Assert.Equal(result.Commission, actual.Commission);
		Assert.Equal(result.Turnover, actual.Turnover);
		Assert.Equal(result.WinLoss, actual.WinLoss);
		Assert.Equal(result.CreatedOn, actual.CreatedOn);
		Assert.Equal(result.ClosedOn, actual.ClosedOn);
		Assert.Equal(result.UpdatedOn, actual.UpdatedOn);
	}

	[Fact]
	public async Task GameWagerFirst_WagerId_CreatedOn_Get_Is_Null()
	{
		var wagerId = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		var createdOn = DateTimeOffset.Parse("2024-02-27T16:29:10.681+08:00");

		var web = new GameWagersClientServiceApplication(builder => _ = builder.ConfigureServices(services =>
		{
			var fakeRepository = Substitute.For<IGameWagerFirstRepository>();

			_ = fakeRepository.GetAsync(
				wagerId: Arg.Is(wagerId),
				createdOn: Arg.Is(createdOn),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns((GameWagerFirstModel?)null);

			_ = services.AddSingleton(fakeRepository);
		}));
		var httpClient = web.CreateDefaultClient();

		var client = new GameWagersClient(httpClient);

		var actual = await client.GetAsync(wagerId: wagerId, createdOn: createdOn);

		Assert.Null(actual);
	}

	[Fact]
	public async Task GameWagerFirst_GameProviderId_GameId_WalletId_ExternalWagerId_CreatedOn_Get()
	{
		var gameProviderId = Guid.NewGuid();
		var gameId = 1;
		var walletId = Guid.NewGuid();
		var externalWagerId = "PG-1762394359405350912";
		var createdOn = DateTimeOffset.Parse("2024-02-27T16:29:10.681+08:00");

		var result = new GameWagerFirstModel(
			WagerId: DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
			GameProviderId: Guid.NewGuid(),
			GameProviderConfigurationId: Guid.NewGuid(),
			GameId: gameId,
			ResellerId: Guid.NewGuid(),
			LicenseeId: Guid.NewGuid(),
			BrandId: Guid.NewGuid(),
			PlayerCode: "4ucmqplxb029",
			CurrencyCode: "CNY",
			WalletId: walletId,
			WagerState: Common.Enums.WagerState.Win,
			WagerType: Common.Enums.WagerType.Normal,
			ClosedState: Common.Enums.ClosedState.Closed,
			VoidState: Common.Enums.VoidState.None,
			CancelState: Common.Enums.CancelState.None,
			FreeGameState: Common.Enums.FreeGameState.None,
			GambleState: Common.Enums.GambleState.None,
			CashoutState: Common.Enums.CashoutState.None,
			ResettleState: Common.Enums.ResettleState.None,
			ExternalRoundId: "1762394359405350912",
			ExternalWagerId: externalWagerId,
			ExternalCreatedOn: DateTimeOffset.Parse("2024-02-27T16:28:53.000+08:00"),
			ExternalClosedOn: DateTimeOffset.Parse("2024-02-27T16:28:53.000+08:00"),
			ExternalPayoffOn: DateTimeOffset.Parse("2024-02-27T16:28:53.000+08:00"),
			ExternalResettleOn: DateTimeOffset.Parse("1970-01-01T08:00:00.000+08:00"),
			CurrencyRatio: 1.0m,
			Bet: 0.5m,
			ValidBet: 0.5m,
			Settle: 2.8m,
			Commission: 0.0m,
			Turnover: 0.5m,
			WinLoss: 2.3m,
			CreatedOn: createdOn,
			ClosedOn: DateTimeOffset.Parse("2024-02-27T16:29:11.267+08:00"),
			UpdatedOn: DateTimeOffset.Parse("2024-02-27T16:29:11.267+08:00"));

		var web = new GameWagersClientServiceApplication(builder => _ = builder.ConfigureServices(services =>
		{
			var fakeRepository = Substitute.For<IGameWagerFirstRepository>();

			_ = fakeRepository.GetAsync(
				gameProviderId: Arg.Is(gameProviderId),
				gameId: Arg.Is(gameId),
				walletId: Arg.Is(walletId),
				externalWagerId: Arg.Is(externalWagerId),
				createdOn: Arg.Is(createdOn),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(result);

			_ = services.AddSingleton(fakeRepository);
		}));
		var httpClient = web.CreateDefaultClient();

		var client = new GameWagersClient(httpClient);

		var actual = await client.GetAsync(gameProviderId: gameProviderId, gameId: gameId, walletId: walletId, externalWagerId: externalWagerId, createdOn: createdOn);

		Assert.NotNull(actual);
		Assert.Equal(result.WagerId, actual.WagerId);
		Assert.Equal(result.GameProviderId, actual.GameProviderId);
		Assert.Equal(result.GameProviderConfigurationId, actual.GameProviderConfigurationId);
		Assert.Equal(result.GameId, actual.GameId);
		Assert.Equal(result.ResellerId, actual.ResellerId);
		Assert.Equal(result.LicenseeId, actual.LicenseeId);
		Assert.Equal(result.BrandId, actual.BrandId);
		Assert.Equal(result.PlayerCode, actual.PlayerCode);
		Assert.Equal(result.CurrencyCode, actual.CurrencyCode);
		Assert.Equal(result.WalletId, actual.WalletId);
		Assert.Equal(result.WagerState, actual.WagerState);
		Assert.Equal(result.WagerType, actual.WagerType);
		Assert.Equal(result.ClosedState, actual.ClosedState);
		Assert.Equal(result.VoidState, actual.VoidState);
		Assert.Equal(result.CancelState, actual.CancelState);
		Assert.Equal(result.FreeGameState, actual.FreeGameState);
		Assert.Equal(result.GambleState, actual.GambleState);
		Assert.Equal(result.CashoutState, actual.CashoutState);
		Assert.Equal(result.ResettleState, actual.ResettleState);
		Assert.Equal(result.ExternalRoundId, actual.ExternalRoundId);
		Assert.Equal(result.ExternalWagerId, actual.ExternalWagerId);
		Assert.Equal(result.ExternalCreatedOn, actual.ExternalCreatedOn);
		Assert.Equal(result.ExternalClosedOn, actual.ExternalClosedOn);
		Assert.Equal(result.ExternalPayoffOn, actual.ExternalPayoffOn);
		Assert.Equal(result.ExternalResettleOn, actual.ExternalResettleOn);
		Assert.Equal(result.CurrencyRatio, actual.CurrencyRatio);
		Assert.Equal(result.Bet, actual.Bet);
		Assert.Equal(result.ValidBet, actual.ValidBet);
		Assert.Equal(result.Settle, actual.Settle);
		Assert.Equal(result.Commission, actual.Commission);
		Assert.Equal(result.Turnover, actual.Turnover);
		Assert.Equal(result.WinLoss, actual.WinLoss);
		Assert.Equal(result.CreatedOn, actual.CreatedOn);
		Assert.Equal(result.ClosedOn, actual.ClosedOn);
		Assert.Equal(result.UpdatedOn, actual.UpdatedOn);
	}

	[Fact]
	public async Task GameWagerFirst_GameProviderId_GameId_WalletId_ExternalWagerId_CreatedOn_Get_Is_Null()
	{
		var gameProviderId = Guid.NewGuid();
		var gameId = 1;
		var walletId = Guid.NewGuid();
		var externalWagerId = "PG-1762394359405350912";
		var createdOn = DateTimeOffset.Parse("2024-02-27T16:29:10.681+08:00");

		var web = new GameWagersClientServiceApplication(builder => _ = builder.ConfigureServices(services =>
		{
			var fakeRepository = Substitute.For<IGameWagerFirstRepository>();

			_ = fakeRepository.GetAsync(
				gameProviderId: Arg.Is(gameProviderId),
				gameId: Arg.Is(gameId),
				walletId: Arg.Is(walletId),
				externalWagerId: Arg.Is(externalWagerId),
				createdOn: Arg.Is(createdOn),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns((GameWagerFirstModel?)null);

			_ = services.AddSingleton(fakeRepository);
		}));
		var httpClient = web.CreateDefaultClient();

		var client = new GameWagersClient(httpClient);

		var actual = await client.GetAsync(gameProviderId: gameProviderId, gameId: gameId, walletId: walletId, externalWagerId: externalWagerId, createdOn: createdOn);

		Assert.Null(actual);
	}
}
