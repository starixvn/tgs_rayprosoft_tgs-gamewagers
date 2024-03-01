using Amazon.Runtime;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NpgsqlTypes;
using Tgs.GameWagers.Common;
using Tgs.GameWagers.Core;
using Tgs.GameWagers.Core.Models;

namespace Tgs.GameWagers.Controllers;

[Route("game-wager-first")]
[ApiController]
public class GameWagerFirstController : ControllerBase
{
	[HttpGet("{brand}/brand/game/stats", Name = "GameWagerFirst_Brand_Game_Stats_Query")]
	public IAsyncEnumerable<GameWagerFirstBrandGameStatsInfo> BrandGameStatsQueryAsync(
		[FromServices] IGameWagerFirstRepository repository,
		string brand,
		[FromQuery] DateTimeOffset startOn,
		[FromQuery] DateTimeOffset endOn)
		=> repository
		.BrandGameStatsQueryAsync(
		brandCode: brand,
		externalClosedStartOn: startOn,
		externalClosedEndOn: endOn,
		cancellationToken: HttpContext.RequestAborted);

	[HttpGet("{brand}/brand/player/stats", Name = "GameWagerFirst_Brand_Player_Stats_Query")]
	public IAsyncEnumerable<GameWagerFirstBrandPlayerStatsInfo> BrandPlayerStatsQueryAsync(
		[FromServices] IGameWagerFirstRepository repository,
		string brand,
		[FromQuery] DateTimeOffset startOn,
		[FromQuery] DateTimeOffset endOn)
		=> repository
		.BrandPlayerStatsQueryAsync(
		brandCode: brand,
		externalClosedStartOn: startOn,
		externalClosedEndOn: endOn,
		cancellationToken: HttpContext.RequestAborted);

	[HttpGet("{gameProviderCode}/game/stats", Name = "GameWagerFirst_Game_Stats_Query")]
	public IAsyncEnumerable<GameWagerFirstGameStatsInfo> GameStatsQueryAsync(
		[FromServices] IGameWagerFirstRepository repository,
		string gameProviderCode,
		[FromQuery] DateTimeOffset startOn,
		[FromQuery] DateTimeOffset endOn)
		=> repository
		.GameStatsQueryAsync(
		gameProviderCode: gameProviderCode,
		externalClosedStartOn: startOn,
		externalClosedEndOn: endOn,
		cancellationToken: HttpContext.RequestAborted);

	[HttpGet("{gameProviderId:guid}/{gameId:int}/{walletId:guid}/{externalWagerId}", Name = "GameWagerFirst_GameProviderId_GameId_WalletId_ExternalWagerId_CreatedOn_Get")]
	public async ValueTask<GameWagerFirstInfo?> GameWagerFirstGetAsync(
		[FromServices] IGameWagerFirstRepository repository,
		Guid gameProviderId,
		int gameId,
		Guid walletId,
		string externalWagerId,
		[FromQuery] DateTimeOffset createdOn)
	{
		var model = await repository.GetAsync(
			gameProviderId: gameProviderId,
			gameId: gameId,
			walletId: walletId,
			externalWagerId: externalWagerId,
			createdOn: createdOn)
			.ConfigureAwait(false);

		return model is null
			? null
			: new GameWagerFirstInfo(
				WagerId: model.WagerId,
				GameProviderId: model.GameProviderId,
				GameProviderConfigurationId: model.GameProviderConfigurationId,
				GameId: model.GameId,
				ResellerId: model.ResellerId,
				LicenseeId: model.LicenseeId,
				BrandId: model.BrandId,
				PlayerCode: model.PlayerCode,
				CurrencyCode: model.CurrencyCode,
				WalletId: model.WalletId,
				WagerState: model.WagerState,
				WagerType: model.WagerType,
				ClosedState: model.ClosedState,
				VoidState: model.VoidState,
				CancelState: model.CancelState,
				FreeGameState: model.FreeGameState,
				GambleState: model.GambleState,
				CashoutState: model.CashoutState,
				ResettleState: model.ResettleState,
				ExternalRoundId: model.ExternalRoundId,
				ExternalWagerId: model.ExternalWagerId,
				ExternalCreatedOn: model.ExternalCreatedOn,
				ExternalClosedOn: model.ExternalClosedOn,
				ExternalPayoffOn: model.ExternalPayoffOn,
				ExternalResettleOn: model.ExternalResettleOn,
				CurrencyRatio: model.CurrencyRatio,
				Bet: model.Bet,
				ValidBet: model.ValidBet,
				Settle: model.Settle,
				Commission: model.Commission,
				Turnover: model.Turnover,
				WinLoss: model.WinLoss,
				CreatedOn: model.CreatedOn,
				ClosedOn: model.ClosedOn,
				UpdatedOn: model.UpdatedOn);
	}

	[HttpGet("{wagerId:long}", Name = "GameWagerFirst_WagerId_CreatedOn_Get")]
	public async ValueTask<GameWagerFirstInfo?> GameWagerFirstGetAsync(
		[FromServices] IGameWagerFirstRepository repository,
		long wagerId,
		[FromQuery] DateTimeOffset createdOn)
	{
		var model = await repository.GetAsync(
			wagerId: wagerId,
			createdOn: createdOn,
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);

		return model is null
			? null
			: new GameWagerFirstInfo(
				WagerId: model.WagerId,
				GameProviderId: model.GameProviderId,
				GameProviderConfigurationId: model.GameProviderConfigurationId,
				GameId: model.GameId,
				ResellerId: model.ResellerId,
				LicenseeId: model.LicenseeId,
				BrandId: model.BrandId,
				PlayerCode: model.PlayerCode,
				CurrencyCode: model.CurrencyCode,
				WalletId: model.WalletId,
				WagerState: model.WagerState,
				WagerType: model.WagerType,
				ClosedState: model.ClosedState,
				VoidState: model.VoidState,
				CancelState: model.CancelState,
				FreeGameState: model.FreeGameState,
				GambleState: model.GambleState,
				CashoutState: model.CashoutState,
				ResettleState: model.ResettleState,
				ExternalRoundId: model.ExternalRoundId,
				ExternalWagerId: model.ExternalWagerId,
				ExternalCreatedOn: model.ExternalCreatedOn,
				ExternalClosedOn: model.ExternalClosedOn,
				ExternalPayoffOn: model.ExternalPayoffOn,
				ExternalResettleOn: model.ExternalResettleOn,
				CurrencyRatio: model.CurrencyRatio,
				Bet: model.Bet,
				ValidBet: model.ValidBet,
				Settle: model.Settle,
				Commission: model.Commission,
				Turnover: model.Turnover,
				WinLoss: model.WinLoss,
				CreatedOn: model.CreatedOn,
				ClosedOn: model.ClosedOn,
				UpdatedOn: model.UpdatedOn);
	}
}
