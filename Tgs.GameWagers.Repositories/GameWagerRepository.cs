using Microsoft.EntityFrameworkCore;
using Tgs.GameWagers.Common.Enums;
using Tgs.GameWagers.Core;
using Tgs.GameWagers.Core.Models;
using Tgs.GameWagers.Repositories.Entities;

namespace Tgs.GameWagers.Repositories;

internal class GameWagerRepository(TgsContext context) : IGameWagerRepository
{
	public async ValueTask<GameWagerModel?> GetAsync(long wagerId, DateTimeOffset createdOn, CancellationToken cancellationToken = default)
		=> await context.GameWagers
		.Where(predicate: gameWagers => gameWagers.CreatedOn >= createdOn.UtcDateTime.AddDays(-7) &&
		gameWagers.WagerId == wagerId)
		.Select(selector: gameWagers => new GameWagerModel(
			gameWagers.WagerId,
			gameWagers.GameProviderId,
			gameWagers.GameProviderConfigurationId,
			gameWagers.GameId,
			gameWagers.ResellerId,
			gameWagers.LicenseeId,
			gameWagers.BrandId,
			gameWagers.PlayerCode,
			gameWagers.CurrencyCode,
			gameWagers.WalletId,
			(WagerState)gameWagers.WagerState,
			(WagerType)gameWagers.WagerType,
			(ClosedState)gameWagers.ClosedState,
			(VoidState)gameWagers.VoidState,
			(CancelState)gameWagers.CancelState,
			(FreeGameState)gameWagers.FreeGameState,
			(GambleState)gameWagers.GambleState,
			(CashoutState)gameWagers.CashoutState,
			(ResettleState)gameWagers.ResettleState,
			gameWagers.ExternalRoundId,
			gameWagers.ExternalWagerId,
			gameWagers.ExternalCreatedOn,
			gameWagers.ExternalClosedOn,
			gameWagers.ExternalPayoffOn,
			gameWagers.ExternalResettleOn,
			gameWagers.CurrencyRatio,
			gameWagers.Bet,
			gameWagers.ValidBet,
			gameWagers.Settle,
			gameWagers.Commission,
			gameWagers.Turnover,
			gameWagers.WinLoss,
			gameWagers.CreatedOn,
			gameWagers.ClosedOn,
			gameWagers.UpdatedOn,
			(ResetState)gameWagers.ResetState,
			gameWagers.ResetOn))
		.FirstOrDefaultAsync(
			cancellationToken: cancellationToken)
		.ConfigureAwait(false);

	public async ValueTask<GameWagerModel?> GetAsync(Guid gameProviderId, int gameId, Guid walletId, string externalWagerId, DateTimeOffset createdOn, CancellationToken cancellationToken = default)
		=> await context.GameWagers
		.Where(predicate: gameWagers => gameWagers.CreatedOn >= createdOn.UtcDateTime.AddDays(-7) &&
		gameWagers.GameProviderId == gameProviderId &&
		gameWagers.GameId == gameId &&
		gameWagers.WalletId == walletId &&
		gameWagers.ExternalWagerId == externalWagerId)
		.Select(selector: gameWagers => new GameWagerModel(
			gameWagers.WagerId,
			gameWagers.GameProviderId,
			gameWagers.GameProviderConfigurationId,
			gameWagers.GameId,
			gameWagers.ResellerId,
			gameWagers.LicenseeId,
			gameWagers.BrandId,
			gameWagers.PlayerCode,
			gameWagers.CurrencyCode,
			gameWagers.WalletId,
			(WagerState)gameWagers.WagerState,
			(WagerType)gameWagers.WagerType,
			(ClosedState)gameWagers.ClosedState,
			(VoidState)gameWagers.VoidState,
			(CancelState)gameWagers.CancelState,
			(FreeGameState)gameWagers.FreeGameState,
			(GambleState)gameWagers.GambleState,
			(CashoutState)gameWagers.CashoutState,
			(ResettleState)gameWagers.ResettleState,
			gameWagers.ExternalRoundId,
			gameWagers.ExternalWagerId,
			gameWagers.ExternalCreatedOn,
			gameWagers.ExternalClosedOn,
			gameWagers.ExternalPayoffOn,
			gameWagers.ExternalResettleOn,
			gameWagers.CurrencyRatio,
			gameWagers.Bet,
			gameWagers.ValidBet,
			gameWagers.Settle,
			gameWagers.Commission,
			gameWagers.Turnover,
			gameWagers.WinLoss,
			gameWagers.CreatedOn,
			gameWagers.ClosedOn,
			gameWagers.UpdatedOn,
			(ResetState)gameWagers.ResetState,
			gameWagers.ResetOn))
		.FirstOrDefaultAsync(
			cancellationToken: cancellationToken)
		.ConfigureAwait(false);
}
