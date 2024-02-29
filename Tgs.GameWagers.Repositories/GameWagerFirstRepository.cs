using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Tgs.GameWagers.Common;
using Tgs.GameWagers.Core;
using Tgs.GameWagers.Core.Models;
using Tgs.GameWagers.Repositories.Entities;
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

	public async IAsyncEnumerable<GameWagerFirstGroupByGame> QueryGroupByGameAsync(string brandCode, DateTimeOffset startOn, DateTimeOffset endOn, BrandWagerGameMinsStatsSort sort, bool isAscending = true, int offset = 0, int limit = 20, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var query = context.GameWagerFirsts
			.Join(
			context.Brands,
			gameWagerFirst => gameWagerFirst.BrandId,
			brand => brand.Id,
			(gameWagerFirst, brand) => new
			{
				gameWagerFirst.GameProviderId,
				gameWagerFirst.GameId,
				gameWagerFirst.ResellerId,
				gameWagerFirst.LicenseeId,
				gameWagerFirst.BrandId,
				BrandCode = brand.Code,
				gameWagerFirst.CurrencyCode,
				GameWagerFirst = gameWagerFirst

			})
			.Join(
			context.Licensees,
			conform => conform.LicenseeId,
			licensee => licensee.Id,
			(conform, licensee) => new
			{
				conform.GameProviderId,
				conform.GameId,
				conform.ResellerId,
				conform.LicenseeId,
				LicenseeCode = licensee.Code,
				conform.BrandId,
				conform.BrandCode,
				conform.CurrencyCode,
				conform.GameWagerFirst
			})
			.Join(
			context.Resellers,
			conform => conform.ResellerId,
			reseller => reseller.Id,
			(conform, reseller) => new
			{
				conform.GameProviderId,
				conform.GameId,
				conform.ResellerId,
				ResellerCode = reseller.Code,
				conform.LicenseeId,
				conform.LicenseeCode,
				conform.BrandId,
				conform.BrandCode,
				conform.CurrencyCode,
				conform.GameWagerFirst
			})
			.Join(
			context.GameProviders,
			conform => conform.GameProviderId,
			gameProvider => gameProvider.Id,
			(conform, gameProvider) => new
			{
				conform.GameProviderId,
				GameProviderCode = gameProvider.Code,
				conform.GameId,
				conform.ResellerId,
				conform.ResellerCode,
				conform.LicenseeId,
				conform.LicenseeCode,
				conform.BrandId,
				conform.BrandCode,
				conform.CurrencyCode,
				conform.GameWagerFirst
			})
			.Where(predicate: conform => conform.GameWagerFirst.CreatedOn >= DateTime.UtcNow.AddMonths(-1) &&
			conform.GameWagerFirst.CreatedOn < DateTime.UtcNow &&
			conform.GameWagerFirst.ExternalClosedOn >= startOn.UtcDateTime &&
			conform.GameWagerFirst.ExternalClosedOn < endOn.UtcDateTime &&
			conform.BrandCode == brandCode)
			.GroupBy(keySelector: conform => new
			{
				conform.GameProviderId,
				conform.GameProviderCode,
				conform.GameId,
				conform.ResellerId,
				conform.ResellerCode,
				conform.LicenseeId,
				conform.LicenseeCode,
				conform.BrandId,
				conform.BrandCode,
				conform.CurrencyCode,
			})
			.Select(selector: conform => new GameWagerFirstGroupByGame(
				conform.Key.GameProviderId,
				conform.Key.GameProviderCode,
				conform.Key.GameId,
				conform.Key.ResellerId,
				conform.Key.ResellerCode,
				conform.Key.LicenseeId,
				conform.Key.LicenseeCode,
				conform.Key.BrandId,
				conform.Key.BrandCode,
				conform.Key.CurrencyCode,
				conform.Count(),
				conform.Sum(s => s.GameWagerFirst.Bet),
				conform.Sum(s => s.GameWagerFirst.ValidBet),
				conform.Sum(s => s.GameWagerFirst.Settle),
				conform.Sum(s => s.GameWagerFirst.Commission),
				conform.Sum(s => s.GameWagerFirst.Turnover),
				conform.Sum(s => s.GameWagerFirst.WinLoss)))
			.ToAsyncEnumerable();

		query = isAscending
			? sort switch
			{
				BrandWagerGameMinsStatsSort.Count => query.OrderBy(keySelector: conform => conform.Count),
				BrandWagerGameMinsStatsSort.Bet => query.OrderBy(keySelector: conform => conform.Bet),
				BrandWagerGameMinsStatsSort.ValidBet => query.OrderBy(keySelector: conform => conform.ValidBet),
				BrandWagerGameMinsStatsSort.Settle => query.OrderBy(keySelector: conform => conform.Settle),
				BrandWagerGameMinsStatsSort.Commission => query.OrderBy(keySelector: conform => conform.Commission),
				BrandWagerGameMinsStatsSort.Turnover => query.OrderBy(keySelector: conform => conform.Turnover),
				BrandWagerGameMinsStatsSort.WinLoss => query.OrderBy(keySelector: conform => conform.WinLoss),
				_ => throw new NotImplementedException()
			}
			: sort switch
			{
				BrandWagerGameMinsStatsSort.Count => query.OrderByDescending(keySelector: conform => conform.Count),
				BrandWagerGameMinsStatsSort.Bet => query.OrderByDescending(keySelector: conform => conform.Bet),
				BrandWagerGameMinsStatsSort.ValidBet => query.OrderByDescending(keySelector: conform => conform.ValidBet),
				BrandWagerGameMinsStatsSort.Settle => query.OrderByDescending(keySelector: conform => conform.Settle),
				BrandWagerGameMinsStatsSort.Commission => query.OrderByDescending(keySelector: conform => conform.Commission),
				BrandWagerGameMinsStatsSort.Turnover => query.OrderByDescending(keySelector: conform => conform.Turnover),
				BrandWagerGameMinsStatsSort.WinLoss => query.OrderByDescending(keySelector: conform => conform.WinLoss),
				_ => throw new NotImplementedException()
			};

		var list = await query.Skip(offset * limit)
			.Take(limit)
			.ToArrayAsync(
			cancellationToken: cancellationToken)
			.ConfigureAwait(false)
			?? Enumerable.Empty<GameWagerFirstGroupByGame>();

		if (!list.Any())
		{
			yield break;
		}

		foreach (var item in list)
		{
			yield return item;
		}
	}

	public async IAsyncEnumerable<GameWagerFirstGroupByPlayer> QueryGroupByPlayerAsync(string brandCode, DateTimeOffset startOn, DateTimeOffset endOn, BrandWagerPlayerMinsStatsSort sort, bool isAscending = true, int offset = 0, int limit = 20, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var query = context.GameWagerFirsts
			.Join(
			context.Brands,
			gameWagerFirst => gameWagerFirst.BrandId,
			brand => brand.Id,
			(gameWagerFirst, brand) => new
			{
				gameWagerFirst.ResellerId,
				gameWagerFirst.LicenseeId,
				gameWagerFirst.BrandId,
				BrandCode = brand.Code,
				gameWagerFirst.PlayerCode,
				gameWagerFirst.CurrencyCode,
				GameWagerFirst = gameWagerFirst

			})
			.Join(
			context.Licensees,
			conform => conform.LicenseeId,
			licensee => licensee.Id,
			(conform, licensee) => new
			{
				conform.ResellerId,
				conform.LicenseeId,
				LicenseeCode = licensee.Code,
				conform.BrandId,
				conform.BrandCode,
				conform.PlayerCode,
				conform.CurrencyCode,
				conform.GameWagerFirst
			})
			.Join(
			context.Resellers,
			conform => conform.ResellerId,
			reseller => reseller.Id,
			(conform, reseller) => new
			{
				conform.ResellerId,
				ResellerCode = reseller.Code,
				conform.LicenseeId,
				conform.LicenseeCode,
				conform.BrandId,
				conform.BrandCode,
				conform.PlayerCode,
				conform.CurrencyCode,
				conform.GameWagerFirst
			})
			.Join(
			context.Players,
			conform => conform.PlayerCode,
			player => player.PlayerCode,
			(conform, player) => new
			{
				conform.ResellerId,
				conform.ResellerCode,
				conform.LicenseeId,
				conform.LicenseeCode,
				conform.BrandId,
				conform.BrandCode,
				conform.PlayerCode,
				player.Username,
				conform.CurrencyCode,
				conform.GameWagerFirst
			})
			.Where(predicate: conform => conform.GameWagerFirst.CreatedOn >= DateTime.UtcNow.AddMonths(-1) &&
			conform.GameWagerFirst.CreatedOn < DateTime.UtcNow &&
			conform.GameWagerFirst.ExternalClosedOn >= startOn.UtcDateTime &&
			conform.GameWagerFirst.ExternalClosedOn < endOn.UtcDateTime &&
			conform.BrandCode == brandCode)
			.GroupBy(keySelector: conform => new
			{
				conform.ResellerId,
				conform.ResellerCode,
				conform.LicenseeId,
				conform.LicenseeCode,
				conform.BrandId,
				conform.BrandCode,
				conform.PlayerCode,
				conform.Username,
				conform.CurrencyCode
			})
			.Select(selector: conform => new GameWagerFirstGroupByPlayer(
				conform.Key.ResellerId,
				conform.Key.ResellerCode,
				conform.Key.LicenseeId,
				conform.Key.LicenseeCode,
				conform.Key.BrandId,
				conform.Key.BrandCode,
				conform.Key.PlayerCode,
				conform.Key.Username,
				conform.Key.CurrencyCode,
				conform.Count(),
				conform.Sum(s => s.GameWagerFirst.Bet),
				conform.Sum(s => s.GameWagerFirst.ValidBet),
				conform.Sum(s => s.GameWagerFirst.Settle),
				conform.Sum(s => s.GameWagerFirst.Commission),
				conform.Sum(s => s.GameWagerFirst.Turnover),
				conform.Sum(s => s.GameWagerFirst.WinLoss)))
			.ToAsyncEnumerable();

		query = isAscending
			? sort switch
			{
				BrandWagerPlayerMinsStatsSort.Count => query.OrderBy(keySelector: conform => conform.Count),
				BrandWagerPlayerMinsStatsSort.Bet => query.OrderBy(keySelector: conform => conform.Bet),
				BrandWagerPlayerMinsStatsSort.ValidBet => query.OrderBy(keySelector: conform => conform.ValidBet),
				BrandWagerPlayerMinsStatsSort.Settle => query.OrderBy(keySelector: conform => conform.Settle),
				BrandWagerPlayerMinsStatsSort.Commission => query.OrderBy(keySelector: conform => conform.Commission),
				BrandWagerPlayerMinsStatsSort.Turnover => query.OrderBy(keySelector: conform => conform.Turnover),
				BrandWagerPlayerMinsStatsSort.WinLoss => query.OrderBy(keySelector: conform => conform.WinLoss),
				_ => throw new NotImplementedException()
			}
			: sort switch
			{
				BrandWagerPlayerMinsStatsSort.Count => query.OrderByDescending(keySelector: conform => conform.Count),
				BrandWagerPlayerMinsStatsSort.Bet => query.OrderByDescending(keySelector: conform => conform.Bet),
				BrandWagerPlayerMinsStatsSort.ValidBet => query.OrderByDescending(keySelector: conform => conform.ValidBet),
				BrandWagerPlayerMinsStatsSort.Settle => query.OrderByDescending(keySelector: conform => conform.Settle),
				BrandWagerPlayerMinsStatsSort.Commission => query.OrderByDescending(keySelector: conform => conform.Commission),
				BrandWagerPlayerMinsStatsSort.Turnover => query.OrderByDescending(keySelector: conform => conform.Turnover),
				BrandWagerPlayerMinsStatsSort.WinLoss => query.OrderByDescending(keySelector: conform => conform.WinLoss),
				_ => throw new NotImplementedException()
			};

		var list = await query.Skip(offset * limit)
			.Take(limit)
			.ToArrayAsync(
			cancellationToken: cancellationToken)
			.ConfigureAwait(false)
			?? Enumerable.Empty<GameWagerFirstGroupByPlayer>();

		if (!list.Any())
		{
			yield break;
		}

		foreach (var item in list)
		{
			yield return item;
		}
	}

	public async IAsyncEnumerable<GameWagerFirstGroupByPlayer> QueryFilterPlayerGroupByPlayerAsync(string brandCode, DateTimeOffset startOn, DateTimeOffset endOn, IEnumerable<string> playerCodes, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var list = await context.GameWagerFirsts
			.Join(
			context.Brands,
			gameWagerFirst => gameWagerFirst.BrandId,
			brand => brand.Id,
			(gameWagerFirst, brand) => new
			{
				gameWagerFirst.ResellerId,
				gameWagerFirst.LicenseeId,
				gameWagerFirst.BrandId,
				BrandCode = brand.Code,
				gameWagerFirst.PlayerCode,
				gameWagerFirst.CurrencyCode,
				GameWagerFirst = gameWagerFirst

			})
			.Join(
			context.Licensees,
			conform => conform.LicenseeId,
			licensee => licensee.Id,
			(conform, licensee) => new
			{
				conform.ResellerId,
				conform.LicenseeId,
				LicenseeCode = licensee.Code,
				conform.BrandId,
				conform.BrandCode,
				conform.PlayerCode,
				conform.CurrencyCode,
				conform.GameWagerFirst
			})
			.Join(
			context.Resellers,
			conform => conform.ResellerId,
			reseller => reseller.Id,
			(conform, reseller) => new
			{
				conform.ResellerId,
				ResellerCode = reseller.Code,
				conform.LicenseeId,
				conform.LicenseeCode,
				conform.BrandId,
				conform.BrandCode,
				conform.PlayerCode,
				conform.CurrencyCode,
				conform.GameWagerFirst
			})
			.Join(
			context.Players,
			conform => conform.PlayerCode,
			player => player.PlayerCode,
			(conform, player) => new
			{
				conform.ResellerId,
				conform.ResellerCode,
				conform.LicenseeId,
				conform.LicenseeCode,
				conform.BrandId,
				conform.BrandCode,
				conform.PlayerCode,
				player.Username,
				conform.CurrencyCode,
				conform.GameWagerFirst
			})
			.Where(predicate: conform => conform.GameWagerFirst.CreatedOn >= DateTime.UtcNow.AddMonths(-1) &&
			conform.GameWagerFirst.CreatedOn < DateTime.UtcNow &&
			conform.GameWagerFirst.ExternalClosedOn >= startOn.UtcDateTime &&
			conform.GameWagerFirst.ExternalClosedOn < endOn.UtcDateTime &&
			conform.BrandCode == brandCode &&
			playerCodes.Contains(conform.PlayerCode))
			.GroupBy(keySelector: conform => new
			{
				conform.ResellerId,
				conform.ResellerCode,
				conform.LicenseeId,
				conform.LicenseeCode,
				conform.BrandId,
				conform.BrandCode,
				conform.PlayerCode,
				conform.Username,
				conform.CurrencyCode
			})
			.Select(selector: conform => new GameWagerFirstGroupByPlayer(
				conform.Key.ResellerId,
				conform.Key.ResellerCode,
				conform.Key.LicenseeId,
				conform.Key.LicenseeCode,
				conform.Key.BrandId,
				conform.Key.BrandCode,
				conform.Key.PlayerCode,
				conform.Key.Username,
				conform.Key.CurrencyCode,
				conform.Count(),
				conform.Sum(s => s.GameWagerFirst.Bet),
				conform.Sum(s => s.GameWagerFirst.ValidBet),
				conform.Sum(s => s.GameWagerFirst.Settle),
				conform.Sum(s => s.GameWagerFirst.Commission),
				conform.Sum(s => s.GameWagerFirst.Turnover),
				conform.Sum(s => s.GameWagerFirst.WinLoss)))
			.ToArrayAsync(
			cancellationToken: cancellationToken)
			.ConfigureAwait(false)
			?? Enumerable.Empty<GameWagerFirstGroupByPlayer>();

		if (!list.Any())
		{
			yield break;
		}

		foreach (var item in list)
		{
			yield return item;
		}
	}
}
