using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tgs.GameWagers.Common.Enums;
using Tgs.GameWagers.Core;
using Tgs.GameWagers.Core.Models;
using Tgs.GameWagers.Repositories.Entities;

namespace Tgs.GameWagers.Repositories;
internal class GameWagersEventSourceRepository(
	TgsContext context) : IGameWagersEventSourceRepository
{
	public IAsyncEnumerable<GameWagersEventSourceModel> QueryAllAsync(Guid gameProviderId, Guid id, int limit = 100, CancellationToken cancellationToken = default)
		=> context.GameWagersEventSources
		.Where(predicate: gwes => gwes.CreatedOn >= DateTime.UtcNow.AddDays(-45) &&
		gwes.GameProviderId == gameProviderId &&
		gwes.Id > id)
		.Select(selector: gwes => new GameWagersEventSourceModel(
			gwes.Id,
			gwes.CreatedOn,
			gwes.GameProviderId,
			gwes.GameProviderConfigurationId,
			gwes.GameId,
			gwes.ResellerId,
			gwes.LicenseeId,
			gwes.BrandId,
			gwes.PlayerCode,
			gwes.CurrencyCode,
			gwes.WalletId,
			(WagerState)gwes.WagerState,
			(WagerType)gwes.WagerType,
			(FreeGameState)gwes.FreeGameState,
			(GambleState)gwes.GambleState,
			(CashoutState)gwes.CashoutState,
			(ResettleState)gwes.ResettleState,
			gwes.ExternalRoundId,
			gwes.ExternalWagerId,
			gwes.ExternalCreatedOn,
			gwes.ExternalClosedOn,
			gwes.ExternalPayoffOn,
			gwes.ExternalResettleOn,
			gwes.CurrencyRatio,
			gwes.Bet,
			gwes.ValidBet,
			gwes.Settle,
			gwes.Commission,
			gwes.Turnover,
			gwes.WinLoss,
			gwes.AdditionalData))
		.Take(limit)
		.AsAsyncEnumerable();
	public IAsyncEnumerable<GameWagersEventSourceModel> QuerySettlementsAsync(Guid gameProviderId, Guid id, int limit = 100, CancellationToken cancellationToken = default)
		=> context.GameWagersEventSources
		.Where(predicate: gwes => gwes.CreatedOn >= DateTime.UtcNow.AddDays(-45) &&
		gwes.GameProviderId == gameProviderId &&
		gwes.WagerState != (int)WagerState.Waiting &&
		gwes.WagerState != (int)WagerState.Running &&
		gwes.Id > id)
		.Select(selector: gwes => new GameWagersEventSourceModel(
			gwes.Id,
			gwes.CreatedOn,
			gwes.GameProviderId,
			gwes.GameProviderConfigurationId,
			gwes.GameId,
			gwes.ResellerId,
			gwes.LicenseeId,
			gwes.BrandId,
			gwes.PlayerCode,
			gwes.CurrencyCode,
			gwes.WalletId,
			(WagerState)gwes.WagerState,
			(WagerType)gwes.WagerType,
			(FreeGameState)gwes.FreeGameState,
			(GambleState)gwes.GambleState,
			(CashoutState)gwes.CashoutState,
			(ResettleState)gwes.ResettleState,
			gwes.ExternalRoundId,
			gwes.ExternalWagerId,
			gwes.ExternalCreatedOn,
			gwes.ExternalClosedOn,
			gwes.ExternalPayoffOn,
			gwes.ExternalResettleOn,
			gwes.CurrencyRatio,
			gwes.Bet,
			gwes.ValidBet,
			gwes.Settle,
			gwes.Commission,
			gwes.Turnover,
			gwes.WinLoss,
			gwes.AdditionalData))
		.Take(limit)
		.AsAsyncEnumerable();
	public IAsyncEnumerable<GameWagersEventSourceModel> QuerySettlementsAsync(Guid gameProviderId, DateTimeOffset fromOn, DateTimeOffset toOn, int offset = 0, int limit = 100, CancellationToken cancellationToken = default)
		=> context.GameWagersEventSources
		.Where(predicate: gwes => gwes.CreatedOn >= fromOn.UtcDateTime &&
		gwes.CreatedOn < toOn.UtcDateTime &&
		gwes.GameProviderId == gameProviderId &&
		gwes.WagerState != (int)WagerState.Waiting &&
		gwes.WagerState != (int)WagerState.Running)
		.Select(selector: gwes => new GameWagersEventSourceModel(
			gwes.Id,
			gwes.CreatedOn,
			gwes.GameProviderId,
			gwes.GameProviderConfigurationId,
			gwes.GameId,
			gwes.ResellerId,
			gwes.LicenseeId,
			gwes.BrandId,
			gwes.PlayerCode,
			gwes.CurrencyCode,
			gwes.WalletId,
			(WagerState)gwes.WagerState,
			(WagerType)gwes.WagerType,
			(FreeGameState)gwes.FreeGameState,
			(GambleState)gwes.GambleState,
			(CashoutState)gwes.CashoutState,
			(ResettleState)gwes.ResettleState,
			gwes.ExternalRoundId,
			gwes.ExternalWagerId,
			gwes.ExternalCreatedOn,
			gwes.ExternalClosedOn,
			gwes.ExternalPayoffOn,
			gwes.ExternalResettleOn,
			gwes.CurrencyRatio,
			gwes.Bet,
			gwes.ValidBet,
			gwes.Settle,
			gwes.Commission,
			gwes.Turnover,
			gwes.WinLoss,
			gwes.AdditionalData))
		.Skip(offset * limit)
		.Take(limit)
		.AsAsyncEnumerable();
	public IAsyncEnumerable<GameWagersEventSourceModel> QuerySettlementsAsync(Guid gameProviderId, DateTimeOffset timepoint, int limit = 100, CancellationToken cancellationToken = default)
		=> context.GameWagersEventSources
		.Where(predicate: gwes => gwes.CreatedOn >= timepoint.UtcDateTime &&
		gwes.GameProviderId == gameProviderId &&
		gwes.WagerState != (int)WagerState.Waiting &&
		gwes.WagerState != (int)WagerState.Running)
		.Select(selector: gwes => new GameWagersEventSourceModel(
			gwes.Id,
			gwes.CreatedOn,
			gwes.GameProviderId,
			gwes.GameProviderConfigurationId,
			gwes.GameId,
			gwes.ResellerId,
			gwes.LicenseeId,
			gwes.BrandId,
			gwes.PlayerCode,
			gwes.CurrencyCode,
			gwes.WalletId,
			(WagerState)gwes.WagerState,
			(WagerType)gwes.WagerType,
			(FreeGameState)gwes.FreeGameState,
			(GambleState)gwes.GambleState,
			(CashoutState)gwes.CashoutState,
			(ResettleState)gwes.ResettleState,
			gwes.ExternalRoundId,
			gwes.ExternalWagerId,
			gwes.ExternalCreatedOn,
			gwes.ExternalClosedOn,
			gwes.ExternalPayoffOn,
			gwes.ExternalResettleOn,
			gwes.CurrencyRatio,
			gwes.Bet,
			gwes.ValidBet,
			gwes.Settle,
			gwes.Commission,
			gwes.Turnover,
			gwes.WinLoss,
			gwes.AdditionalData))
		.Take(limit)
		.AsAsyncEnumerable();
}
