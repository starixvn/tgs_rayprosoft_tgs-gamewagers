using System.Text.Json.Nodes;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Tgs.GameWagers.Client;
using Tgs.GameWagers.Common;
using Tgs.GameWagers.Common.Enums;
using Tgs.GameWagers.Core;
using Tgs.GameWagers.Core.Models;
using Tgs.VipLevelCommand.IntegrationTest;

namespace Tgs.GameWagers.IntegrationTest;

public class GameWagersClientUnitTest
{
	[Fact]
	public async Task GameBetsEventSource_QueryAll()
	{
		var gameProviderId = Guid.NewGuid();
		var id = Guid.NewGuid();
		var limit = 100;

		var externalRoundId = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
		var externalBetId = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();

		var model = new GameBetEventSourceModel(
			Id: Guid.NewGuid(),
			CreatedOn: DateTimeOffset.UtcNow,
			GameProviderId: gameProviderId,
			GameProviderConfigurationId: Guid.NewGuid(),
			GameId: 1,
			ResellerId: Guid.NewGuid(),
			LicenseeId: Guid.NewGuid(),
			BrandId: Guid.NewGuid(),
			PlayerCode: "test",
			CurrencyCode: "CNY",
			WalletId: Guid.NewGuid(),
			ExternalRoundId: externalRoundId,
			ExternalBetId: externalBetId,
			ExternalCreatedOn: DateTimeOffset.UtcNow.AddSeconds(-11),
			ExternalClosedOn: DateTimeOffset.UtcNow.AddSeconds(-7),
			ExternalPayoffOn: DateTimeOffset.UtcNow.AddSeconds(-3),
			ExternalResettleOn: DateTimeOffset.UnixEpoch,
			BetState: BetState.Win,
			BetType: BetType.Normal,
			FreeGameState: FreeGameState.None,
			GambleState: GambleState.None,
			CashoutState: CashoutState.None,
			ResettleState: ResettleState.None,
			JackpotState: JackpotState.None,
			CurrencyRatio: 1,
			Bet: 100,
			ValidBet: 75,
			Settle: 146.25m,
			Commission: 0,
			Turnover: 71.25m,
			WinLoss: 71.25m,
			Jackpot: 0.0m,
			AdditionalData: new JsonObject
			{
				["ExternalRoundId"] = externalRoundId,
				["ExternalBetId"] = externalBetId
			}
			.ToString());
		var web = new GameWagersClientServiceApplication(builder => _ = builder.ConfigureServices(services =>
		{
			var fakeRepository = Substitute.For<IGameBetEventSourceRepository>();
			_ = fakeRepository.QueryAllAsync(
				gameProviderId: Arg.Is(gameProviderId),
				id: Arg.Is(id),
				limit: Arg.Is(limit),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(new GameBetEventSourceModel[] { model }.ToAsyncEnumerable());

			_ = services.AddSingleton(fakeRepository);
		}));
		var httpClient = web.CreateClient();

		var sut = new GameWagersClient(httpClient);

		var actual = await sut.GameBetsEventSourceQueryAllAsync(
			gameProviderId: gameProviderId,
			id: id,
			limit: limit)
			.ToArrayAsync()
			?? Enumerable.Empty<GameBetsEventSourceInfo>();

		Assert.Equal(model.Id, actual.First().Id);
		Assert.Equal(model.CreatedOn, actual.First().CreatedOn);
		Assert.Equal(model.GameProviderId, actual.First().GameProviderId);
		Assert.Equal(model.GameProviderConfigurationId, actual.First().GameProviderConfigurationId);
		Assert.Equal(model.GameId, actual.First().GameId);
		Assert.Equal(model.ResellerId, actual.First().ResellerId);
		Assert.Equal(model.LicenseeId, actual.First().LicenseeId);
		Assert.Equal(model.BrandId, actual.First().BrandId);
		Assert.Equal(model.PlayerCode, actual.First().PlayerCode);
		Assert.Equal(model.CurrencyCode, actual.First().CurrencyCode);
		Assert.Equal(model.WalletId, actual.First().WalletId);
		Assert.Equal(model.BetState, actual.First().BetState);
		Assert.Equal(model.BetType, actual.First().BetType);
		Assert.Equal(model.FreeGameState, actual.First().FreeGameState);
		Assert.Equal(model.GambleState, actual.First().GambleState);
		Assert.Equal(model.CashoutState, actual.First().CashoutState);
		Assert.Equal(model.ResettleState, actual.First().ResettleState);
		Assert.Equal(model.ExternalRoundId, actual.First().ExternalRoundId);
		Assert.Equal(model.ExternalBetId, actual.First().ExternalBetId);
		Assert.Equal(model.ExternalCreatedOn, actual.First().ExternalCreatedOn);
		Assert.Equal(model.ExternalClosedOn, actual.First().ExternalClosedOn);
		Assert.Equal(model.ExternalPayoffOn, actual.First().ExternalPayoffOn);
		Assert.Equal(model.ExternalResettleOn, actual.First().ExternalResettleOn);
		Assert.Equal(model.CurrencyRatio, actual.First().CurrencyRatio);
		Assert.Equal(model.Bet, actual.First().Bet);
		Assert.Equal(model.ValidBet, actual.First().ValidBet);
		Assert.Equal(model.Settle, actual.First().Settle);
		Assert.Equal(model.Commission, actual.First().Commission);
		Assert.Equal(model.Turnover, actual.First().Turnover);
		Assert.Equal(model.WinLoss, actual.First().WinLoss);
		Assert.Equal(JsonNode.Parse(model.AdditionalData)!.ToJsonString(), actual.First().AdditionalData.ToJsonString());
	}

	[Fact]
	public async Task GameBetsEventSource_QuerySettlements_Id()
	{
		var gameProviderId = Guid.NewGuid();
		var id = Guid.NewGuid();
		var limit = 100;

		var externalRoundId = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
		var externalBetId = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();

		var model = new GameBetEventSourceModel(
			Id: Guid.NewGuid(),
			CreatedOn: DateTimeOffset.UtcNow,
			GameProviderId: gameProviderId,
			GameProviderConfigurationId: Guid.NewGuid(),
			GameId: 1,
			ResellerId: Guid.NewGuid(),
			LicenseeId: Guid.NewGuid(),
			BrandId: Guid.NewGuid(),
			PlayerCode: "test",
			CurrencyCode: "CNY",
			WalletId: Guid.NewGuid(),
			ExternalRoundId: externalRoundId,
			ExternalBetId: externalBetId,
			ExternalCreatedOn: DateTimeOffset.UtcNow.AddSeconds(-11),
			ExternalClosedOn: DateTimeOffset.UtcNow.AddSeconds(-7),
			ExternalPayoffOn: DateTimeOffset.UtcNow.AddSeconds(-3),
			ExternalResettleOn: DateTimeOffset.UnixEpoch,
			BetState: BetState.Win,
			BetType: BetType.Normal,
			FreeGameState: FreeGameState.None,
			GambleState: GambleState.None,
			CashoutState: CashoutState.None,
			ResettleState: ResettleState.None,
			JackpotState: JackpotState.None,
			CurrencyRatio: 1,
			Bet: 100,
			ValidBet: 75,
			Settle: 146.25m,
			Commission: 0,
			Turnover: 71.25m,
			WinLoss: 71.25m,
			Jackpot: 0.0m,
			AdditionalData: new JsonObject
			{
				["ExternalRoundId"] = externalRoundId,
				["ExternalBetId"] = externalBetId
			}
			.ToString());
		var web = new GameWagersClientServiceApplication(builder => _ = builder.ConfigureServices(services =>
		{
			var fakeRepository = Substitute.For<IGameBetEventSourceRepository>();
			_ = fakeRepository.QuerySettlementsAsync(
				gameProviderId: Arg.Is(gameProviderId),
				id: Arg.Is(id),
				limit: Arg.Is(limit),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(new GameBetEventSourceModel[] { model }.ToAsyncEnumerable());

			_ = services.AddSingleton(fakeRepository);
		}));
		var httpClient = web.CreateClient();

		var sut = new GameWagersClient(httpClient);

		var actual = await sut.GameBetsEventSourceQuerySettlementsAsync(
			gameProviderId: gameProviderId,
			id: id,
			limit: limit)
			.ToArrayAsync()
			?? Enumerable.Empty<GameBetsEventSourceInfo>();

		Assert.Equal(model.Id, actual.First().Id);
		Assert.Equal(model.CreatedOn, actual.First().CreatedOn);
		Assert.Equal(model.GameProviderId, actual.First().GameProviderId);
		Assert.Equal(model.GameProviderConfigurationId, actual.First().GameProviderConfigurationId);
		Assert.Equal(model.GameId, actual.First().GameId);
		Assert.Equal(model.ResellerId, actual.First().ResellerId);
		Assert.Equal(model.LicenseeId, actual.First().LicenseeId);
		Assert.Equal(model.BrandId, actual.First().BrandId);
		Assert.Equal(model.PlayerCode, actual.First().PlayerCode);
		Assert.Equal(model.CurrencyCode, actual.First().CurrencyCode);
		Assert.Equal(model.WalletId, actual.First().WalletId);
		Assert.Equal(model.BetState, actual.First().BetState);
		Assert.Equal(model.BetType, actual.First().BetType);
		Assert.Equal(model.FreeGameState, actual.First().FreeGameState);
		Assert.Equal(model.GambleState, actual.First().GambleState);
		Assert.Equal(model.CashoutState, actual.First().CashoutState);
		Assert.Equal(model.ResettleState, actual.First().ResettleState);
		Assert.Equal(model.ExternalRoundId, actual.First().ExternalRoundId);
		Assert.Equal(model.ExternalBetId, actual.First().ExternalBetId);
		Assert.Equal(model.ExternalCreatedOn, actual.First().ExternalCreatedOn);
		Assert.Equal(model.ExternalClosedOn, actual.First().ExternalClosedOn);
		Assert.Equal(model.ExternalPayoffOn, actual.First().ExternalPayoffOn);
		Assert.Equal(model.ExternalResettleOn, actual.First().ExternalResettleOn);
		Assert.Equal(model.CurrencyRatio, actual.First().CurrencyRatio);
		Assert.Equal(model.Bet, actual.First().Bet);
		Assert.Equal(model.ValidBet, actual.First().ValidBet);
		Assert.Equal(model.Settle, actual.First().Settle);
		Assert.Equal(model.Commission, actual.First().Commission);
		Assert.Equal(model.Turnover, actual.First().Turnover);
		Assert.Equal(model.WinLoss, actual.First().WinLoss);
		Assert.Equal(JsonNode.Parse(model.AdditionalData)!.ToJsonString(), actual.First().AdditionalData.ToJsonString());
	}

	[Fact]
	public async Task GameBetsEventSource_QuerySettlements_Timepoint()
	{
		var gameProviderId = Guid.NewGuid();
		var timepoint = DateTimeOffset.UtcNow.AddMinutes(-1);
		var limit = 100;

		var externalRoundId = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
		var externalBetId = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();

		var model = new GameBetEventSourceModel(
			Id: Guid.NewGuid(),
			CreatedOn: DateTimeOffset.UtcNow,
			GameProviderId: gameProviderId,
			GameProviderConfigurationId: Guid.NewGuid(),
			GameId: 1,
			ResellerId: Guid.NewGuid(),
			LicenseeId: Guid.NewGuid(),
			BrandId: Guid.NewGuid(),
			PlayerCode: "test",
			CurrencyCode: "CNY",
			WalletId: Guid.NewGuid(),
			ExternalRoundId: externalRoundId,
			ExternalBetId: externalBetId,
			ExternalCreatedOn: DateTimeOffset.UtcNow.AddSeconds(-11),
			ExternalClosedOn: DateTimeOffset.UtcNow.AddSeconds(-7),
			ExternalPayoffOn: DateTimeOffset.UtcNow.AddSeconds(-3),
			ExternalResettleOn: DateTimeOffset.UnixEpoch,
			BetState: BetState.Win,
			BetType: BetType.Normal,
			FreeGameState: FreeGameState.None,
			GambleState: GambleState.None,
			CashoutState: CashoutState.None,
			ResettleState: ResettleState.None,
			JackpotState: JackpotState.None,
			CurrencyRatio: 1,
			Bet: 100,
			ValidBet: 75,
			Settle: 146.25m,
			Commission: 0,
			Turnover: 71.25m,
			WinLoss: 71.25m,
			Jackpot: 0.0m,
			AdditionalData: new JsonObject
			{
				["ExternalRoundId"] = externalRoundId,
				["ExternalBetId"] = externalBetId
			}
			.ToString());
		var web = new GameWagersClientServiceApplication(builder => _ = builder.ConfigureServices(services =>
		{
			var fakeRepository = Substitute.For<IGameBetEventSourceRepository>();
			_ = fakeRepository.QuerySettlementsAsync(
				gameProviderId: Arg.Is(gameProviderId),
				timepoint: Arg.Is(DateTimeOffset.Parse(timepoint.ToString("u"))),
				limit: Arg.Is(limit),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(new GameBetEventSourceModel[] { model }.ToAsyncEnumerable());

			_ = services.AddSingleton(fakeRepository);
		}));
		var httpClient = web.CreateClient();

		var sut = new GameWagersClient(httpClient);

		var actual = await sut.GameBetsEventSourceQuerySettlementsAsync(
			gameProviderId: gameProviderId,
			timepoint: timepoint,
			limit: limit)
			.ToArrayAsync()
			?? Enumerable.Empty<GameBetsEventSourceInfo>();

		Assert.Equal(model.Id, actual.First().Id);
		Assert.Equal(model.CreatedOn, actual.First().CreatedOn);
		Assert.Equal(model.GameProviderId, actual.First().GameProviderId);
		Assert.Equal(model.GameProviderConfigurationId, actual.First().GameProviderConfigurationId);
		Assert.Equal(model.GameId, actual.First().GameId);
		Assert.Equal(model.ResellerId, actual.First().ResellerId);
		Assert.Equal(model.LicenseeId, actual.First().LicenseeId);
		Assert.Equal(model.BrandId, actual.First().BrandId);
		Assert.Equal(model.PlayerCode, actual.First().PlayerCode);
		Assert.Equal(model.CurrencyCode, actual.First().CurrencyCode);
		Assert.Equal(model.WalletId, actual.First().WalletId);
		Assert.Equal(model.BetState, actual.First().BetState);
		Assert.Equal(model.BetType, actual.First().BetType);
		Assert.Equal(model.FreeGameState, actual.First().FreeGameState);
		Assert.Equal(model.GambleState, actual.First().GambleState);
		Assert.Equal(model.CashoutState, actual.First().CashoutState);
		Assert.Equal(model.ResettleState, actual.First().ResettleState);
		Assert.Equal(model.ExternalRoundId, actual.First().ExternalRoundId);
		Assert.Equal(model.ExternalBetId, actual.First().ExternalBetId);
		Assert.Equal(model.ExternalCreatedOn, actual.First().ExternalCreatedOn);
		Assert.Equal(model.ExternalClosedOn, actual.First().ExternalClosedOn);
		Assert.Equal(model.ExternalPayoffOn, actual.First().ExternalPayoffOn);
		Assert.Equal(model.ExternalResettleOn, actual.First().ExternalResettleOn);
		Assert.Equal(model.CurrencyRatio, actual.First().CurrencyRatio);
		Assert.Equal(model.Bet, actual.First().Bet);
		Assert.Equal(model.ValidBet, actual.First().ValidBet);
		Assert.Equal(model.Settle, actual.First().Settle);
		Assert.Equal(model.Commission, actual.First().Commission);
		Assert.Equal(model.Turnover, actual.First().Turnover);
		Assert.Equal(model.WinLoss, actual.First().WinLoss);
		Assert.Equal(JsonNode.Parse(model.AdditionalData)!.ToJsonString(), actual.First().AdditionalData.ToJsonString());
	}

	[Fact]
	public async Task GameBetsEventSource_QuerySettlements_TimeInterval()
	{
		var gameProviderId = Guid.NewGuid();
		var fromOn = DateTimeOffset.UtcNow.AddMinutes(-10);
		var toOn = DateTimeOffset.UtcNow;
		var page = 1;
		var pageSize = 100;

		var externalRoundId = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
		var externalBetId = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();

		var model = new GameBetEventSourceModel(
			Id: Guid.NewGuid(),
			CreatedOn: DateTimeOffset.UtcNow,
			GameProviderId: gameProviderId,
			GameProviderConfigurationId: Guid.NewGuid(),
			GameId: 1,
			ResellerId: Guid.NewGuid(),
			LicenseeId: Guid.NewGuid(),
			BrandId: Guid.NewGuid(),
			PlayerCode: "test",
			CurrencyCode: "CNY",
			WalletId: Guid.NewGuid(),
			ExternalRoundId: externalRoundId,
			ExternalBetId: externalBetId,
			ExternalCreatedOn: DateTimeOffset.UtcNow.AddSeconds(-11),
			ExternalClosedOn: DateTimeOffset.UtcNow.AddSeconds(-7),
			ExternalPayoffOn: DateTimeOffset.UtcNow.AddSeconds(-3),
			ExternalResettleOn: DateTimeOffset.UnixEpoch,
			BetState: BetState.Win,
			BetType: BetType.Normal,
			FreeGameState: FreeGameState.None,
			GambleState: GambleState.None,
			CashoutState: CashoutState.None,
			ResettleState: ResettleState.None,
			JackpotState: JackpotState.None,
			CurrencyRatio: 1,
			Bet: 100,
			ValidBet: 75,
			Settle: 146.25m,
			Commission: 0,
			Turnover: 71.25m,
			WinLoss: 71.25m,
			Jackpot: 0.0m,
			AdditionalData: new JsonObject
			{
				["ExternalRoundId"] = externalRoundId,
				["ExternalBetId"] = externalBetId
			}
			.ToString());
		var web = new GameWagersClientServiceApplication(builder => _ = builder.ConfigureServices(services =>
		{
			var fakeRepository = Substitute.For<IGameBetEventSourceRepository>();
			_ = fakeRepository.QuerySettlementsAsync(
				gameProviderId: Arg.Is(gameProviderId),
				fromOn: Arg.Is(DateTimeOffset.Parse(fromOn.ToString("u"))),
				toOn: Arg.Is(DateTimeOffset.Parse(toOn.ToString("u"))),
				offset: Arg.Is(page - 1),
				limit: Arg.Is(pageSize),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(new GameBetEventSourceModel[] { model }.ToAsyncEnumerable());

			_ = services.AddSingleton(fakeRepository);
		}));
		var httpClient = web.CreateClient();

		var sut = new GameWagersClient(httpClient);

		var actual = await sut.GameBetsEventSourceQuerySettlementsAsync(
			gameProviderId: gameProviderId,
			fromOn: fromOn,
			toOn: toOn,
			page: page,
			pageSize: pageSize)
			.ToArrayAsync()
			?? Enumerable.Empty<GameBetsEventSourceInfo>();

		Assert.Equal(model.Id, actual.First().Id);
		Assert.Equal(model.CreatedOn, actual.First().CreatedOn);
		Assert.Equal(model.GameProviderId, actual.First().GameProviderId);
		Assert.Equal(model.GameProviderConfigurationId, actual.First().GameProviderConfigurationId);
		Assert.Equal(model.GameId, actual.First().GameId);
		Assert.Equal(model.ResellerId, actual.First().ResellerId);
		Assert.Equal(model.LicenseeId, actual.First().LicenseeId);
		Assert.Equal(model.BrandId, actual.First().BrandId);
		Assert.Equal(model.PlayerCode, actual.First().PlayerCode);
		Assert.Equal(model.CurrencyCode, actual.First().CurrencyCode);
		Assert.Equal(model.WalletId, actual.First().WalletId);
		Assert.Equal(model.BetState, actual.First().BetState);
		Assert.Equal(model.BetType, actual.First().BetType);
		Assert.Equal(model.FreeGameState, actual.First().FreeGameState);
		Assert.Equal(model.GambleState, actual.First().GambleState);
		Assert.Equal(model.CashoutState, actual.First().CashoutState);
		Assert.Equal(model.ResettleState, actual.First().ResettleState);
		Assert.Equal(model.ExternalRoundId, actual.First().ExternalRoundId);
		Assert.Equal(model.ExternalBetId, actual.First().ExternalBetId);
		Assert.Equal(model.ExternalCreatedOn, actual.First().ExternalCreatedOn);
		Assert.Equal(model.ExternalClosedOn, actual.First().ExternalClosedOn);
		Assert.Equal(model.ExternalPayoffOn, actual.First().ExternalPayoffOn);
		Assert.Equal(model.ExternalResettleOn, actual.First().ExternalResettleOn);
		Assert.Equal(model.CurrencyRatio, actual.First().CurrencyRatio);
		Assert.Equal(model.Bet, actual.First().Bet);
		Assert.Equal(model.ValidBet, actual.First().ValidBet);
		Assert.Equal(model.Settle, actual.First().Settle);
		Assert.Equal(model.Commission, actual.First().Commission);
		Assert.Equal(model.Turnover, actual.First().Turnover);
		Assert.Equal(model.WinLoss, actual.First().WinLoss);
		Assert.Equal(JsonNode.Parse(model.AdditionalData)!.ToJsonString(), actual.First().AdditionalData.ToJsonString());
	}
}
