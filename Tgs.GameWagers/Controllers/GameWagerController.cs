using Microsoft.AspNetCore.Mvc;
using Tgs.GameWagers.Common;
using Tgs.GameWagers.Core;

namespace Tgs.GameWagers.Controllers;

[Route("game-wager")]
[ApiController]
public class GameWagerController : ControllerBase
{
	[HttpGet("{gameProviderId:guid}/{gameId:int}/{walletId:guid}/{externalWagerId}", Name = "GameWager_GameProviderId_GameId_WalletId_ExternalWagerId_CreatedOn_Get")]
	public async ValueTask<GameWagerInfo?> GameWagerGetAsync(
		[FromServices] IGameWagerRepository repository,
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
			: new GameWagerInfo(
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
				UpdatedOn: model.UpdatedOn,
				ResetState: model.ResetState,
				ResetOn: model.ResetOn);
	}

	[HttpGet("{wagerId:long}", Name = "GameWager_WagerId_CreatedOn_Get")]
	public async ValueTask<GameWagerInfo?> GameWagerGetAsync(
		[FromServices] IGameWagerRepository repository,
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
			: new GameWagerInfo(
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
				UpdatedOn: model.UpdatedOn,
				ResetState: model.ResetState,
				ResetOn: model.ResetOn);
	}
}
