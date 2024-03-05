using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Tgs.GameWagers.Common;
using Tgs.GameWagers.Common.Enums;
using Tgs.GameWagers.Core;
using Tgs.GameWagers.Core.Models;
using Tgs.GameWagers.Repositories.Entities;

namespace Tgs.GameWagers.Repositories;

internal class GameWagerStatCloseRepository(
	TgsContext context) : IGameWagerStatCloseRepository
{
	public async ValueTask<GameWagerStatPlayerAccumModel?> GetPlayerAccumAsync(string playerCode, DateTimeOffset startOn, DateTimeOffset endOn, CancellationToken cancellationToken = default)
		=> await context.GameWagerStatCloses
		.Where(predicate: stat => stat.StatOn >= startOn.UtcDateTime &&
		stat.StatOn < endOn.UtcDateTime &&
		stat.PlayerCode == playerCode)
		.GroupBy(keySelector: stat => stat.PlayerCode)
		.Select(selector: stat => new GameWagerStatPlayerAccumModel(
			stat.Key,
			stat.Sum(sum => sum.Count),
			stat.Sum(sum => sum.Bet),
			stat.Sum(sum => sum.ValidBet),
			stat.Sum(sum => sum.Settle),
			stat.Sum(sum => sum.Commission),
			stat.Sum(sum => sum.Turnover),
			stat.Sum(sum => sum.WinLoss)))
		.FirstOrDefaultAsync(
			cancellationToken: cancellationToken)
		.ConfigureAwait(false);

	public async IAsyncEnumerable<GameWagerStatCloseGroupByPlayerModel> QueryFilterPlayerGroupByPlayerAsync(string brandCode, DateTimeOffset startOn, DateTimeOffset endOn, IEnumerable<string> playerCodes, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var list = await context.GameWagerStatCloses
			.Join(
			context.Players,
			gameWagerStatClose => gameWagerStatClose.PlayerCode,
			player => player.PlayerCode,
			(gameWagerStatClose, player) => new
			{
				gameWagerStatClose.ResellerId,
				gameWagerStatClose.LicenseeId,
				gameWagerStatClose.BrandId,
				player.PlayerCode,
				player.Username,
				CurrencyCode = player.DefaultWalletCurrencyCode,
				GameWagerStatClose = gameWagerStatClose
			})
			.Join(
			context.Brands,
			conform => conform.BrandId,
			brand => brand.Id,
			(conform, brand) => new
			{
				conform.ResellerId,
				conform.LicenseeId,
				conform.BrandId,
				BrandCode = brand.Code,
				conform.PlayerCode,
				conform.Username,
				conform.CurrencyCode,
				conform.GameWagerStatClose
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
				conform.Username,
				conform.CurrencyCode,
				conform.GameWagerStatClose
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
				conform.Username,
				conform.CurrencyCode,
				conform.GameWagerStatClose
			})
			.Where(predicate: conform => conform.GameWagerStatClose.StatOn >= startOn.UtcDateTime &&
			conform.GameWagerStatClose.StatOn < endOn.UtcDateTime &&
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
				conform.CurrencyCode,
			})
			.Select(selector: conform => new GameWagerStatCloseGroupByPlayerModel(
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
				conform.Sum(s => s.GameWagerStatClose.Bet),
				conform.Sum(s => s.GameWagerStatClose.ValidBet),
				conform.Sum(s => s.GameWagerStatClose.Settle),
				conform.Sum(s => s.GameWagerStatClose.Commission),
				conform.Sum(s => s.GameWagerStatClose.Turnover),
				conform.Sum(s => s.GameWagerStatClose.WinLoss)))
			.ToArrayAsync(
			cancellationToken: cancellationToken)
			.ConfigureAwait(false)
			?? Enumerable.Empty<GameWagerStatCloseGroupByPlayerModel>();

		if (!list.Any())
		{
			yield break;
		}

		foreach (var item in list)
		{
			yield return item;
		}
	}

	public async IAsyncEnumerable<GameWagerStatCloseGroupByGameModel> QueryGroupByGameAsync(string brandCode, DateTimeOffset startOn, DateTimeOffset endOn, BrandWagerGameMinsStatsSort sort, bool isAscending = true, int offset = 0, int limit = 20, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var query = context.GameWagerStatCloses
			.Join(
			context.Players,
			gameWagerStatClose => gameWagerStatClose.PlayerCode,
			player => player.PlayerCode,
			(gameWagerStatClose, player) => new
			{
				gameWagerStatClose.GameProviderId,
				gameWagerStatClose.GameId,
				gameWagerStatClose.ResellerId,
				gameWagerStatClose.LicenseeId,
				gameWagerStatClose.BrandId,
				CurrencyCode = player.DefaultWalletCurrencyCode,
				GameWagerStatClose = gameWagerStatClose
			})
			.Join(
			context.Brands,
			conform => conform.BrandId,
			brand => brand.Id,
			(conform, brand) => new
			{
				conform.GameProviderId,
				conform.GameId,
				conform.ResellerId,
				conform.LicenseeId,
				conform.BrandId,
				BrandCode = brand.Code,
				conform.CurrencyCode,
				conform.GameWagerStatClose
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
				conform.GameWagerStatClose
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
				conform.GameWagerStatClose
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
				conform.GameWagerStatClose
			})
			.Where(predicate: conform => conform.GameWagerStatClose.StatOn >= startOn.UtcDateTime &&
			conform.GameWagerStatClose.StatOn < endOn.UtcDateTime &&
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
			.Select(selector: conform => new GameWagerStatCloseGroupByGameModel(
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
				conform.Sum(s => s.GameWagerStatClose.Bet),
				conform.Sum(s => s.GameWagerStatClose.ValidBet),
				conform.Sum(s => s.GameWagerStatClose.Settle),
				conform.Sum(s => s.GameWagerStatClose.Commission),
				conform.Sum(s => s.GameWagerStatClose.Turnover),
				conform.Sum(s => s.GameWagerStatClose.WinLoss)))
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
			?? Enumerable.Empty<GameWagerStatCloseGroupByGameModel>();

		if (!list.Any())
		{
			yield break;
		}

		foreach (var item in list)
		{
			yield return item;
		}
	}

	public async IAsyncEnumerable<GameWagerStatCloseGroupByPlayerModel> QueryGroupByPlayerAsync(string brandCode, DateTimeOffset startOn, DateTimeOffset endOn, BrandWagerPlayerMinsStatsSort sort, bool isAscending = true, int offset = 0, int limit = 20, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var query = context.GameWagerStatCloses
			.Join(
			context.Players,
			gameWagerStatClose => gameWagerStatClose.PlayerCode,
			player => player.PlayerCode,
			(gameWagerStatClose, player) => new
			{
				gameWagerStatClose.ResellerId,
				gameWagerStatClose.LicenseeId,
				gameWagerStatClose.BrandId,
				player.PlayerCode,
				player.Username,
				CurrencyCode = player.DefaultWalletCurrencyCode,
				GameWagerStatClose = gameWagerStatClose
			})
			.Join(
			context.Brands,
			conform => conform.BrandId,
			brand => brand.Id,
			(conform, brand) => new
			{
				conform.ResellerId,
				conform.LicenseeId,
				conform.BrandId,
				BrandCode = brand.Code,
				conform.PlayerCode,
				conform.Username,
				conform.CurrencyCode,
				conform.GameWagerStatClose
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
				conform.Username,
				conform.CurrencyCode,
				conform.GameWagerStatClose
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
				conform.Username,
				conform.CurrencyCode,
				conform.GameWagerStatClose
			})
			.Where(predicate: conform => conform.GameWagerStatClose.StatOn >= startOn.UtcDateTime &&
			conform.GameWagerStatClose.StatOn < endOn.UtcDateTime &&
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
				conform.CurrencyCode,
			})
			.Select(selector: conform => new GameWagerStatCloseGroupByPlayerModel(
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
				conform.Sum(s => s.GameWagerStatClose.Bet),
				conform.Sum(s => s.GameWagerStatClose.ValidBet),
				conform.Sum(s => s.GameWagerStatClose.Settle),
				conform.Sum(s => s.GameWagerStatClose.Commission),
				conform.Sum(s => s.GameWagerStatClose.Turnover),
				conform.Sum(s => s.GameWagerStatClose.WinLoss)))
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
			?? Enumerable.Empty<GameWagerStatCloseGroupByPlayerModel>();

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
