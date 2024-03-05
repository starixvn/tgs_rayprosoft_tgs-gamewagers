using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Tgs.GameWagers.Common;
using Tgs.GameWagers.Common.Enums;
using Tgs.GameWagers.Core;
using Tgs.GameWagers.Core.Models;
using Tgs.GameWagers.Repositories.Entities;
using Tgs.GameWagers.Repositories.Entities.Configurations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Tgs.GameWagers.Repositories;

internal class GameWagerFirstRepository(
	TgsContext context) : IGameWagerFirstRepository
{
	public async ValueTask<GameWagerFirstModel?> GetAsync(long wagerId, DateTimeOffset createdOn, CancellationToken cancellationToken = default)
		=> await context.GameWagerFirsts
		.Where(predicate: first => first.FirstOn >= createdOn.UtcDateTime.AddDays(-7) &&
		first.WagerId == wagerId)
		.Select(selector: first => new GameWagerFirstModel(
			first.WagerId,
			first.GameProviderId,
			first.GameProviderConfigurationId,
			first.GameId,
			first.ResellerId,
			first.LicenseeId,
			first.BrandId,
			first.PlayerCode,
			first.CurrencyCode,
			first.WalletId,
			(WagerState)first.WagerState,
			(WagerType)first.WagerType,
			(ClosedState)first.ClosedState,
			(VoidState)first.VoidState,
			(CancelState)first.CancelState,
			(FreeGameState)first.FreeGameState,
			(GambleState)first.GambleState,
			(CashoutState)first.CashoutState,
			(ResettleState)first.ResettleState,
			first.ExternalRoundId,
			first.ExternalWagerId,
			first.ExternalCreatedOn,
			first.ExternalClosedOn,
			first.ExternalPayoffOn,
			first.ExternalResettleOn,
			first.CurrencyRatio,
			first.Bet,
			first.ValidBet,
			first.Settle,
			first.Commission,
			first.Turnover,
			first.WinLoss,
			first.CreatedOn,
			first.ClosedOn,
			first.UpdatedOn))
		.FirstOrDefaultAsync(
			cancellationToken: cancellationToken)
		.ConfigureAwait(false);

	public async ValueTask<GameWagerFirstModel?> GetAsync(Guid gameProviderId, int gameId, Guid walletId, string externalWagerId, DateTimeOffset createdOn, CancellationToken cancellationToken = default)
		=> await context.GameWagerFirsts
		.Where(predicate: first => first.FirstOn >= createdOn.UtcDateTime.AddDays(-7) &&
		first.GameProviderId == gameProviderId &&
		first.GameId == gameId &&
		first.WalletId == walletId &&
		first.ExternalWagerId == externalWagerId)
		.Select(selector: first => new GameWagerFirstModel(
			first.WagerId,
			first.GameProviderId,
			first.GameProviderConfigurationId,
			first.GameId,
			first.ResellerId,
			first.LicenseeId,
			first.BrandId,
			first.PlayerCode,
			first.CurrencyCode,
			first.WalletId,
			(WagerState)first.WagerState,
			(WagerType)first.WagerType,
			(ClosedState)first.ClosedState,
			(VoidState)first.VoidState,
			(CancelState)first.CancelState,
			(FreeGameState)first.FreeGameState,
			(GambleState)first.GambleState,
			(CashoutState)first.CashoutState,
			(ResettleState)first.ResettleState,
			first.ExternalRoundId,
			first.ExternalWagerId,
			first.ExternalCreatedOn,
			first.ExternalClosedOn,
			first.ExternalPayoffOn,
			first.ExternalResettleOn,
			first.CurrencyRatio,
			first.Bet,
			first.ValidBet,
			first.Settle,
			first.Commission,
			first.Turnover,
			first.WinLoss,
			first.CreatedOn,
			first.ClosedOn,
			first.UpdatedOn))
		.AsAsyncEnumerable()
		.FirstOrDefaultAsync(
			cancellationToken: cancellationToken)
		.ConfigureAwait(false);
}
