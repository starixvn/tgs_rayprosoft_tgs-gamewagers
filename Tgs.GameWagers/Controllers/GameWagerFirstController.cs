using Microsoft.AspNetCore.Mvc;
using Tgs.GameWagers.Common;
using Tgs.GameWagers.Core;

namespace Tgs.GameWagers.Controllers;

[Route("game-wager-first")]
[ApiController]
public class GameWagerFirstController : ControllerBase
{
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
