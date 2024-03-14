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
		.Distinct()
		.OrderBy(keySelector: gwes => gwes.Id)
		.Join(
			context.GameWagerPrimaries,
			gameWagersEventSource => new { gameWagersEventSource.GameProviderId, gameWagersEventSource.GameId, gameWagersEventSource.WalletId, gameWagersEventSource.ExternalWagerId },
			gameWagerPrimarie => new { gameWagerPrimarie.GameProviderId, gameWagerPrimarie.GameId, gameWagerPrimarie.WalletId, gameWagerPrimarie.ExternalWagerId },
			(gameWagersEventSource, gameWagerPrimarie) => new GameWagersEventSourceModel(
			gameWagersEventSource.Id,
			gameWagersEventSource.CreatedOn,
			gameWagerPrimarie.Id,
			gameWagersEventSource.GameProviderId,
			gameWagersEventSource.GameProviderConfigurationId,
			gameWagersEventSource.GameId,
			gameWagersEventSource.ResellerId,
			gameWagersEventSource.LicenseeId,
			gameWagersEventSource.BrandId,
			gameWagersEventSource.PlayerCode,
			gameWagersEventSource.CurrencyCode,
			gameWagersEventSource.WalletId,
			(WagerState)gameWagersEventSource.WagerState,
			(WagerType)gameWagersEventSource.WagerType,
			(FreeGameState)gameWagersEventSource.FreeGameState,
			(GambleState)gameWagersEventSource.GambleState,
			(CashoutState)gameWagersEventSource.CashoutState,
			(ResettleState)gameWagersEventSource.ResettleState,
			gameWagersEventSource.ExternalRoundId,
			gameWagersEventSource.ExternalWagerId,
			gameWagersEventSource.ExternalCreatedOn,
			gameWagersEventSource.ExternalClosedOn,
			gameWagersEventSource.ExternalPayoffOn,
			gameWagersEventSource.ExternalResettleOn,
			gameWagersEventSource.CurrencyRatio,
			gameWagersEventSource.Bet,
			gameWagersEventSource.ValidBet,
			gameWagersEventSource.Settle,
			gameWagersEventSource.Commission,
			gameWagersEventSource.Turnover,
			gameWagersEventSource.WinLoss,
			gameWagersEventSource.AdditionalData))
		.Skip(0)
		.Take(limit)
		.AsAsyncEnumerable();

	public IAsyncEnumerable<GameWagersEventSourceModel> QuerySettlementsAsync(Guid gameProviderId, Guid id, int limit = 100, CancellationToken cancellationToken = default)
		=> context.GameWagersEventSources
		.Where(predicate: gwes => gwes.CreatedOn >= DateTime.UtcNow.AddDays(-45) &&
		gwes.GameProviderId == gameProviderId &&
		gwes.WagerState != (int)WagerState.Waiting &&
		gwes.WagerState != (int)WagerState.Running &&
		gwes.Id > id)
		.Distinct()
		.OrderBy(keySelector: gwes => gwes.Id)
		.Join(
			context.GameWagerPrimaries,
			gameWagersEventSource => new { gameWagersEventSource.GameProviderId, gameWagersEventSource.GameId, gameWagersEventSource.WalletId, gameWagersEventSource.ExternalWagerId },
			gameWagerPrimarie => new { gameWagerPrimarie.GameProviderId, gameWagerPrimarie.GameId, gameWagerPrimarie.WalletId, gameWagerPrimarie.ExternalWagerId },
			(gameWagersEventSource, gameWagerPrimarie) => new GameWagersEventSourceModel(
			gameWagersEventSource.Id,
			gameWagersEventSource.CreatedOn,
			gameWagerPrimarie.Id,
			gameWagersEventSource.GameProviderId,
			gameWagersEventSource.GameProviderConfigurationId,
			gameWagersEventSource.GameId,
			gameWagersEventSource.ResellerId,
			gameWagersEventSource.LicenseeId,
			gameWagersEventSource.BrandId,
			gameWagersEventSource.PlayerCode,
			gameWagersEventSource.CurrencyCode,
			gameWagersEventSource.WalletId,
			(WagerState)gameWagersEventSource.WagerState,
			(WagerType)gameWagersEventSource.WagerType,
			(FreeGameState)gameWagersEventSource.FreeGameState,
			(GambleState)gameWagersEventSource.GambleState,
			(CashoutState)gameWagersEventSource.CashoutState,
			(ResettleState)gameWagersEventSource.ResettleState,
			gameWagersEventSource.ExternalRoundId,
			gameWagersEventSource.ExternalWagerId,
			gameWagersEventSource.ExternalCreatedOn,
			gameWagersEventSource.ExternalClosedOn,
			gameWagersEventSource.ExternalPayoffOn,
			gameWagersEventSource.ExternalResettleOn,
			gameWagersEventSource.CurrencyRatio,
			gameWagersEventSource.Bet,
			gameWagersEventSource.ValidBet,
			gameWagersEventSource.Settle,
			gameWagersEventSource.Commission,
			gameWagersEventSource.Turnover,
			gameWagersEventSource.WinLoss,
			gameWagersEventSource.AdditionalData))
		.Skip(0)
		.Take(limit)
		.AsAsyncEnumerable();

	public IAsyncEnumerable<GameWagersEventSourceModel> QuerySettlementsAsync(Guid gameProviderId, DateTimeOffset fromOn, DateTimeOffset toOn, int offset = 0, int limit = 100, CancellationToken cancellationToken = default)
		=> context.GameWagersEventSources
		.Where(predicate: gwes => gwes.CreatedOn >= fromOn.UtcDateTime &&
		gwes.CreatedOn < toOn.UtcDateTime &&
		gwes.GameProviderId == gameProviderId &&
		gwes.WagerState != (int)WagerState.Waiting &&
		gwes.WagerState != (int)WagerState.Running)
		.Distinct()
		.OrderBy(keySelector: gwes => gwes.Id)
		.Join(
			context.GameWagerPrimaries,
			gameWagersEventSource => new { gameWagersEventSource.GameProviderId, gameWagersEventSource.GameId, gameWagersEventSource.WalletId, gameWagersEventSource.ExternalWagerId },
			gameWagerPrimarie => new { gameWagerPrimarie.GameProviderId, gameWagerPrimarie.GameId, gameWagerPrimarie.WalletId, gameWagerPrimarie.ExternalWagerId },
			(gameWagersEventSource, gameWagerPrimarie) => new GameWagersEventSourceModel(
			gameWagersEventSource.Id,
			gameWagersEventSource.CreatedOn,
			gameWagerPrimarie.Id,
			gameWagersEventSource.GameProviderId,
			gameWagersEventSource.GameProviderConfigurationId,
			gameWagersEventSource.GameId,
			gameWagersEventSource.ResellerId,
			gameWagersEventSource.LicenseeId,
			gameWagersEventSource.BrandId,
			gameWagersEventSource.PlayerCode,
			gameWagersEventSource.CurrencyCode,
			gameWagersEventSource.WalletId,
			(WagerState)gameWagersEventSource.WagerState,
			(WagerType)gameWagersEventSource.WagerType,
			(FreeGameState)gameWagersEventSource.FreeGameState,
			(GambleState)gameWagersEventSource.GambleState,
			(CashoutState)gameWagersEventSource.CashoutState,
			(ResettleState)gameWagersEventSource.ResettleState,
			gameWagersEventSource.ExternalRoundId,
			gameWagersEventSource.ExternalWagerId,
			gameWagersEventSource.ExternalCreatedOn,
			gameWagersEventSource.ExternalClosedOn,
			gameWagersEventSource.ExternalPayoffOn,
			gameWagersEventSource.ExternalResettleOn,
			gameWagersEventSource.CurrencyRatio,
			gameWagersEventSource.Bet,
			gameWagersEventSource.ValidBet,
			gameWagersEventSource.Settle,
			gameWagersEventSource.Commission,
			gameWagersEventSource.Turnover,
			gameWagersEventSource.WinLoss,
			gameWagersEventSource.AdditionalData))
		.Skip(offset * limit)
		.Take(limit)
		.AsAsyncEnumerable();

	public IAsyncEnumerable<GameWagersEventSourceModel> QuerySettlementsAsync(Guid gameProviderId, DateTimeOffset timepoint, int limit = 100, CancellationToken cancellationToken = default)
		=> context.GameWagersEventSources
		.Where(predicate: gwes => gwes.CreatedOn >= timepoint.UtcDateTime &&
		gwes.GameProviderId == gameProviderId &&
		gwes.WagerState != (int)WagerState.Waiting &&
		gwes.WagerState != (int)WagerState.Running)
		.Distinct()
		.OrderBy(keySelector: gwes => gwes.Id)
		.Join(
			context.GameWagerPrimaries,
			gameWagersEventSource => new { gameWagersEventSource.GameProviderId, gameWagersEventSource.GameId, gameWagersEventSource.WalletId, gameWagersEventSource.ExternalWagerId },
			gameWagerPrimarie => new { gameWagerPrimarie.GameProviderId, gameWagerPrimarie.GameId, gameWagerPrimarie.WalletId, gameWagerPrimarie.ExternalWagerId },
			(gameWagersEventSource, gameWagerPrimarie) => new GameWagersEventSourceModel(
			gameWagersEventSource.Id,
			gameWagersEventSource.CreatedOn,
			gameWagerPrimarie.Id,
			gameWagersEventSource.GameProviderId,
			gameWagersEventSource.GameProviderConfigurationId,
			gameWagersEventSource.GameId,
			gameWagersEventSource.ResellerId,
			gameWagersEventSource.LicenseeId,
			gameWagersEventSource.BrandId,
			gameWagersEventSource.PlayerCode,
			gameWagersEventSource.CurrencyCode,
			gameWagersEventSource.WalletId,
			(WagerState)gameWagersEventSource.WagerState,
			(WagerType)gameWagersEventSource.WagerType,
			(FreeGameState)gameWagersEventSource.FreeGameState,
			(GambleState)gameWagersEventSource.GambleState,
			(CashoutState)gameWagersEventSource.CashoutState,
			(ResettleState)gameWagersEventSource.ResettleState,
			gameWagersEventSource.ExternalRoundId,
			gameWagersEventSource.ExternalWagerId,
			gameWagersEventSource.ExternalCreatedOn,
			gameWagersEventSource.ExternalClosedOn,
			gameWagersEventSource.ExternalPayoffOn,
			gameWagersEventSource.ExternalResettleOn,
			gameWagersEventSource.CurrencyRatio,
			gameWagersEventSource.Bet,
			gameWagersEventSource.ValidBet,
			gameWagersEventSource.Settle,
			gameWagersEventSource.Commission,
			gameWagersEventSource.Turnover,
			gameWagersEventSource.WinLoss,
			gameWagersEventSource.AdditionalData))
		.Skip(0)
		.Take(limit)
		.AsAsyncEnumerable();
}
