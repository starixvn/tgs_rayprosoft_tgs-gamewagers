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
	TgsContext context,
	IDbConnection dbConnection) : IGameWagerFirstRepository
{
	public async IAsyncEnumerable<GameWagerFirstBrandGameStatsInfo> BrandGameStatsQueryAsync(
		string brandCode,
		DateTimeOffset externalClosedStartOn,
		DateTimeOffset externalClosedEndOn,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		DynamicParameters parameters = new DynamicParameters();
		parameters.Add("@createdStartOn", externalClosedStartOn.AddDays(-7).UtcDateTime, DbType.DateTimeOffset, ParameterDirection.Input);
		parameters.Add("@createdEndOn", DateTimeOffset.UtcNow.UtcDateTime, DbType.DateTimeOffset, ParameterDirection.Input);
		parameters.Add("@externalClosedStartOn", externalClosedStartOn.UtcDateTime, DbType.DateTimeOffset, ParameterDirection.Input);
		parameters.Add("@externalClosedEndOn", externalClosedEndOn.UtcDateTime, DbType.DateTimeOffset, ParameterDirection.Input);
		parameters.Add("brandCode", brandCode, DbType.String, ParameterDirection.Input);

		var sqlQuery = @"
            select
	            gw.game_provider_id as ""GameProviderId"",
	            gp.code as ""GameProviderCode"",
	            gw.game_id as ""GameId"",
	            gw.reseller_id as ""ResellerId"",
	            r.code as ""ResellerCode"",
	            gw.licensee_id as ""LicenseeId"",
	            l.code as ""LicenseeCode"",
	            gw.brand_id as ""BrandId"",
	            b.code as ""BrandCode"",
	            gw.currency_code as ""CurrencyCode"",
	            count(1) as ""Count"",
	            SUM(gw.bet) as ""Bet"",
	            SUM(gw.valid_bet) as ""ValidBet"",
	            SUM(gw.settle) as ""Settle"",
	            SUM(gw.commission) as ""Commission"",
	            SUM(gw.turnover) as ""Turnover"",
	            SUM(gw.win_loss) as ""WinLoss""
            from
	            dbo.game_wager_firsts gw
            join dbo.resellers r
            on
	            gw.reseller_id = r.id
            join dbo.licensees l
            on
	            gw.licensee_id = l.id
            join dbo.brands b
            on
	            gw.brand_id = b.id
            join dbo.game_providers gp
            on
	            gw.game_provider_id = gp.id
            join dbo.games g
            on
	            gw.game_id = g.id
            where gw.created_on >= @createdStartOn
            and gw.created_on < @createdEndOn
            and gw.external_closed_on  >= @externalClosedStartOn
            and gw.external_closed_on < @externalClosedEndOn
            and b.code = @brandCode
            group by
	            gw.game_provider_id,
	            gp.code ,
	            gw.game_id ,
	            gw.reseller_id ,
	            r.code ,
	            gw.licensee_id ,
	            l.code ,
	            gw.brand_id ,
	            b.code ,
	            gw.currency_code";

		var infos = await dbConnection.QueryAsync<GameWagerFirstBrandGameStatsInfo>(
			sql: sqlQuery,
			param: parameters)
			.ConfigureAwait(false);

		if (!infos.Any())
		{
			yield break;
		}

		foreach (var info in infos)
		{
			yield return info;
		}
	}

	public async IAsyncEnumerable<GameWagerFirstBrandPlayerStatsInfo> BrandPlayerStatsQueryAsync(
		string brandCode,
		DateTimeOffset externalClosedStartOn,
		DateTimeOffset externalClosedEndOn,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		DynamicParameters parameters = new DynamicParameters();
		parameters.Add("@createdStartOn", externalClosedStartOn.AddDays(-7).UtcDateTime, DbType.DateTimeOffset, ParameterDirection.Input);
		parameters.Add("@createdEndOn", DateTimeOffset.UtcNow.UtcDateTime, DbType.DateTimeOffset, ParameterDirection.Input);
		parameters.Add("@externalClosedStartOn", externalClosedStartOn.UtcDateTime, DbType.DateTimeOffset, ParameterDirection.Input);
		parameters.Add("@externalClosedEndOn", externalClosedEndOn.UtcDateTime, DbType.DateTimeOffset, ParameterDirection.Input);
		parameters.Add("brandCode", brandCode, DbType.String, ParameterDirection.Input);

		var sqlQuery = @"
            select
	            gw.game_provider_id as ""GameProviderId"",
	            gp.code as ""GameProviderCode"",
	            gw.game_id as ""GameId"",
	            gw.reseller_id as ""ResellerId"",
	            r.code as ""ResellerCode"",
	            gw.licensee_id as ""LicenseeId"",
	            l.code as ""LicenseeCode"",
	            gw.brand_id as ""BrandId"",
	            b.code as ""BrandCode"",
	            gw.player_code as ""PlayerCode"",
	            p.username as ""Username"",
	            gw.currency_code as ""CurrencyCode"",
	            count(1) as ""Count"",
	            SUM(gw.bet) as ""Bet"",
	            SUM(gw.valid_bet) as ""ValidBet"",
	            SUM(gw.settle) as ""Settle"",
	            SUM(gw.commission) as ""Commission"",
	            SUM(gw.turnover) as ""Turnover"",
	            SUM(gw.win_loss) as ""WinLoss""
            from
	            dbo.game_wager_firsts gw
            join dbo.resellers r
            on
	            gw.reseller_id = r.id
            join dbo.licensees l
            on
	            gw.licensee_id = l.id
            join dbo.brands b
            on
	            gw.brand_id = b.id
            join dbo.players p
	            on
	            gw.player_code = p.player_code
            join dbo.game_providers gp
            on
	            gw.game_provider_id = gp.id
            where gw.created_on >= @createdStartOn
            and gw.created_on < @createdEndOn
            and gw.external_closed_on  >= @externalClosedStartOn
            and gw.external_closed_on < @externalClosedEndOn
            and b.code = @brandCode
            group by
	            gw.game_provider_id,
	            gp.code ,
	            gw.game_id ,
	            gw.reseller_id ,
	            r.code ,
	            gw.licensee_id ,
	            l.code ,
	            gw.brand_id ,
	            b.code ,
	            gw.player_code ,
	            p.username ,
	            gw.currency_code";

		var infos = await dbConnection.QueryAsync<GameWagerFirstBrandPlayerStatsInfo>(
			sql: sqlQuery,
			param: parameters)
			.ConfigureAwait(false);

		if (!infos.Any())
		{
			yield break;
		}

		foreach (var info in infos)
		{
			yield return info;
		}
	}

	public async IAsyncEnumerable<GameWagerFirstGameStatsInfo> GameStatsQueryAsync(
		string gameProviderCode,
		DateTimeOffset externalClosedStartOn,
		DateTimeOffset externalClosedEndOn,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		DynamicParameters parameters = new DynamicParameters();
		parameters.Add("@createdStartOn", externalClosedStartOn.AddDays(-7).UtcDateTime, DbType.DateTimeOffset, ParameterDirection.Input);
		parameters.Add("@createdEndOn", DateTimeOffset.UtcNow.UtcDateTime, DbType.DateTimeOffset, ParameterDirection.Input);
		parameters.Add("@externalClosedStartOn", externalClosedStartOn.UtcDateTime, DbType.DateTimeOffset, ParameterDirection.Input);
		parameters.Add("@externalClosedEndOn", externalClosedEndOn.UtcDateTime, DbType.DateTimeOffset, ParameterDirection.Input);
		parameters.Add("@gameProviderCode", gameProviderCode, DbType.String, ParameterDirection.Input);

		var sqlQuery = @"
            select
	            gw.game_provider_id as ""GameProviderId"",
	            gp.code as ""GameProviderCode"",
	            gw.game_id as ""GameId"",
	            gw.currency_code as ""CurrencyCode"",
	            count(1) as ""Count"",
	            SUM(gw.bet) as ""Bet"",
	            SUM(gw.valid_bet) as ""ValidBet"",
	            SUM(gw.settle) as ""Settle"",
	            SUM(gw.commission) as ""Commission"",
	            SUM(gw.turnover) as ""Turnover"",
	            SUM(gw.win_loss) as ""WinLoss""
            from
	            dbo.game_wager_firsts gw
            join dbo.game_providers gp
            on
	            gw.game_provider_id = gp.id
            where gw.created_on >= @createdStartOn
            and gw.created_on < @createdEndOn
            and gw.external_closed_on  >= @externalClosedStartOn
            and gw.external_closed_on < @externalClosedEndOn
            and gp.code = @gameProviderCode
            group by
	            gw.game_provider_id,
	            gp.code ,
	            gw.game_id ,
	            gw.currency_code";

		var infos = await dbConnection.QueryAsync<GameWagerFirstGameStatsInfo>(
			sql: sqlQuery,
			param: parameters)
			.ConfigureAwait(false);

		if (!infos.Any())
		{
			yield break;
		}

		foreach (var info in infos)
		{
			yield return info;
		}
	}

	public async ValueTask<GameWagerFirstModel?> GetAsync(long wagerId, DateTimeOffset createdOn, CancellationToken cancellationToken = default)
		=> await context.GameWagerFirsts
		.Where(predicate: first => first.FirstOn >= createdOn.AddDays(-7) &&
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
		.FirstOrDefaultAsync(cancellationToken: cancellationToken)
		.ConfigureAwait(false);

	public async ValueTask<GameWagerFirstModel?> GetAsync(Guid gameProviderId, int gameId, Guid walletId, string externalWagerId, DateTimeOffset createdOn, CancellationToken cancellationToken = default)
		=> await context.GameWagerFirsts
		.Where(predicate: first => first.FirstOn >= createdOn.AddDays(-7) &&
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
		.FirstOrDefaultAsync(cancellationToken: cancellationToken)
		.ConfigureAwait(false);
}
