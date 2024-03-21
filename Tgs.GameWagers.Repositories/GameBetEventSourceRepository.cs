using Microsoft.EntityFrameworkCore;
using Tgs.GameWagers.Common.Enums;
using Tgs.GameWagers.Core;
using Tgs.GameWagers.Core.Models;
using Tgs.GameWagers.Repositories.Entities;

namespace Tgs.GameWagers.Repositories;

internal class GameBetEventSourceRepository(
	TgsContext context) : IGameBetEventSourceRepository
{
	public IAsyncEnumerable<GameBetEventSourceModel> QueryAllAsync(Guid gameProviderId, Guid id, int limit = 100, CancellationToken cancellationToken = default)
		=> context.GameBetEventSources
		.Where(predicate: gbes => gbes.CreatedOn >= DateTime.UtcNow.AddDays(-45) &&
		gbes.GameProviderId == gameProviderId &&
		gbes.Id > id)
		.Distinct()
		.OrderBy(keySelector: gbes => gbes.Id)
		.Select(gbes => new GameBetEventSourceModel(
			gbes.Id,
			gbes.CreatedOn,
			gbes.GameProviderId,
			gbes.GameProviderConfigurationId,
			gbes.GameId,
			gbes.ResellerId,
			gbes.LicenseeId,
			gbes.BrandId,
			gbes.PlayerCode,
			gbes.CurrencyCode,
			gbes.WalletId,
			gbes.ExternalRoundId,
			gbes.ExternalWagerId,
			gbes.ExternalCreatedOn,
			gbes.ExternalClosedOn,
			gbes.ExternalPayoffOn,
			gbes.ExternalResettleOn,
			(BetState)gbes.BetState,
			(BetType)gbes.BetType,
			(FreeGameState)gbes.FreeGameState,
			(GambleState)gbes.GambleState,
			(CashoutState)gbes.CashoutState,
			(ResettleState)gbes.ResettleState,
			(JackpotState)gbes.JackpotState,
			gbes.CurrencyRatio,
			gbes.Bet,
			gbes.ValidBet,
			gbes.Settle,
			gbes.Commission,
			gbes.Turnover,
			gbes.WinLoss,
			gbes.Jackpot,
			gbes.AdditionalData))
		.Skip(0)
		.Take(limit)
		.AsAsyncEnumerable();

	public IAsyncEnumerable<GameBetEventSourceModel> QuerySettlementsAsync(Guid gameProviderId, Guid id, int limit = 100, CancellationToken cancellationToken = default)
		=> context.GameBetEventSources
		.Where(predicate: gwes => gwes.CreatedOn >= DateTime.UtcNow.AddDays(-45) &&
		gwes.GameProviderId == gameProviderId &&
		gwes.BetState != (int)BetState.Waiting &&
		gwes.BetState != (int)BetState.Running &&
		gwes.Id > id)
		.Distinct()
		.OrderBy(keySelector: gbes => gbes.Id)
		.Select(gbes => new GameBetEventSourceModel(
			gbes.Id,
			gbes.CreatedOn,
			gbes.GameProviderId,
			gbes.GameProviderConfigurationId,
			gbes.GameId,
			gbes.ResellerId,
			gbes.LicenseeId,
			gbes.BrandId,
			gbes.PlayerCode,
			gbes.CurrencyCode,
			gbes.WalletId,
			gbes.ExternalRoundId,
			gbes.ExternalWagerId,
			gbes.ExternalCreatedOn,
			gbes.ExternalClosedOn,
			gbes.ExternalPayoffOn,
			gbes.ExternalResettleOn,
			(BetState)gbes.BetState,
			(BetType)gbes.BetType,
			(FreeGameState)gbes.FreeGameState,
			(GambleState)gbes.GambleState,
			(CashoutState)gbes.CashoutState,
			(ResettleState)gbes.ResettleState,
			(JackpotState)gbes.JackpotState,
			gbes.CurrencyRatio,
			gbes.Bet,
			gbes.ValidBet,
			gbes.Settle,
			gbes.Commission,
			gbes.Turnover,
			gbes.WinLoss,
			gbes.Jackpot,
			gbes.AdditionalData))
		.Skip(0)
		.Take(limit)
		.AsAsyncEnumerable();

	public IAsyncEnumerable<GameBetEventSourceModel> QuerySettlementsAsync(Guid gameProviderId, DateTimeOffset fromOn, DateTimeOffset toOn, int offset = 0, int limit = 100, CancellationToken cancellationToken = default)
		=> context.GameBetEventSources
		.Where(predicate: gwes => gwes.CreatedOn >= fromOn.UtcDateTime &&
		gwes.CreatedOn < toOn.UtcDateTime &&
		gwes.GameProviderId == gameProviderId &&
		gwes.BetState != (int)BetState.Waiting &&
		gwes.BetState != (int)BetState.Running)
		.Distinct()
		.OrderBy(keySelector: gwes => gwes.Id)
		.Select(gbes => new GameBetEventSourceModel(
			gbes.Id,
			gbes.CreatedOn,
			gbes.GameProviderId,
			gbes.GameProviderConfigurationId,
			gbes.GameId,
			gbes.ResellerId,
			gbes.LicenseeId,
			gbes.BrandId,
			gbes.PlayerCode,
			gbes.CurrencyCode,
			gbes.WalletId,
			gbes.ExternalRoundId,
			gbes.ExternalWagerId,
			gbes.ExternalCreatedOn,
			gbes.ExternalClosedOn,
			gbes.ExternalPayoffOn,
			gbes.ExternalResettleOn,
			(BetState)gbes.BetState,
			(BetType)gbes.BetType,
			(FreeGameState)gbes.FreeGameState,
			(GambleState)gbes.GambleState,
			(CashoutState)gbes.CashoutState,
			(ResettleState)gbes.ResettleState,
			(JackpotState)gbes.JackpotState,
			gbes.CurrencyRatio,
			gbes.Bet,
			gbes.ValidBet,
			gbes.Settle,
			gbes.Commission,
			gbes.Turnover,
			gbes.WinLoss,
			gbes.Jackpot,
			gbes.AdditionalData))
		.Skip(offset * limit)
		.Take(limit)
		.AsAsyncEnumerable();

	public IAsyncEnumerable<GameBetEventSourceModel> QuerySettlementsAsync(Guid gameProviderId, DateTimeOffset timepoint, int limit = 100, CancellationToken cancellationToken = default)
		=> context.GameBetEventSources
		.Where(predicate: gwes => gwes.CreatedOn >= timepoint.UtcDateTime &&
		gwes.GameProviderId == gameProviderId &&
		gwes.BetState != (int)BetState.Waiting &&
		gwes.BetState != (int)BetState.Running)
		.Distinct()
		.OrderBy(keySelector: gwes => gwes.Id)
		.Select(gbes => new GameBetEventSourceModel(
			gbes.Id,
			gbes.CreatedOn,
			gbes.GameProviderId,
			gbes.GameProviderConfigurationId,
			gbes.GameId,
			gbes.ResellerId,
			gbes.LicenseeId,
			gbes.BrandId,
			gbes.PlayerCode,
			gbes.CurrencyCode,
			gbes.WalletId,
			gbes.ExternalRoundId,
			gbes.ExternalWagerId,
			gbes.ExternalCreatedOn,
			gbes.ExternalClosedOn,
			gbes.ExternalPayoffOn,
			gbes.ExternalResettleOn,
			(BetState)gbes.BetState,
			(BetType)gbes.BetType,
			(FreeGameState)gbes.FreeGameState,
			(GambleState)gbes.GambleState,
			(CashoutState)gbes.CashoutState,
			(ResettleState)gbes.ResettleState,
			(JackpotState)gbes.JackpotState,
			gbes.CurrencyRatio,
			gbes.Bet,
			gbes.ValidBet,
			gbes.Settle,
			gbes.Commission,
			gbes.Turnover,
			gbes.WinLoss,
			gbes.Jackpot,
			gbes.AdditionalData))
		.Skip(0)
		.Take(limit)
		.AsAsyncEnumerable();
}
