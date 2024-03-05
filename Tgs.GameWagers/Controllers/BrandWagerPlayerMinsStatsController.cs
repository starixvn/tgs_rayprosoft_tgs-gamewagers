using Microsoft.AspNetCore.Mvc;
using Tgs.GameWagers.Common;
using Tgs.GameWagers.Core;
using Tgs.GameWagers.Core.Models;

namespace Tgs.GameWagers.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BrandWagerPlayerMinsStatsController : ControllerBase
{
	[HttpGet("{brand}", Name = "BrandWagerPlayerMinsStats_Query")]
	public async IAsyncEnumerable<BrandWagerPlayerStatsInfo> QueryAsync(
		[FromServices] IGameWagerStatCloseRepository repository,
		string brand,
		[FromQuery] DateTimeOffset startOn,
		[FromQuery] DateTimeOffset endOn,
		[FromQuery] BrandWagerPlayerMinsStatsSort sort,
		[FromQuery] bool isAscending = true,
		[FromQuery] int page = 1,
		[FromQuery] int pageSize = 1000)
	{
		var gameWagerStatCloseGroupByPlayers = await repository.QueryGroupByPlayerAsync(
			brandCode: brand,
			startOn: startOn,
			endOn: endOn,
			sort: sort,
			isAscending: isAscending,
			offset: page - 1,
			limit: pageSize,
			cancellationToken: HttpContext.RequestAborted)
			.ToArrayAsync(
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false)
			?? Enumerable.Empty<GameWagerStatCloseGroupByPlayer>();

		if (!gameWagerStatCloseGroupByPlayers.Any())
		{
			yield break;
		}

		foreach (var gameWagerStatCloseGroupByPlayer in gameWagerStatCloseGroupByPlayers)
		{
			yield return new BrandWagerPlayerStatsInfo(
				ResellerId: gameWagerStatCloseGroupByPlayer.ResellerId,
				ResellerCode: gameWagerStatCloseGroupByPlayer.ResellerCode,
				LicenseeId: gameWagerStatCloseGroupByPlayer.LicenseeId,
				LicenseeCode: gameWagerStatCloseGroupByPlayer.LicenseeCode,
				BrandId: gameWagerStatCloseGroupByPlayer.BrandId,
				BrandCode: gameWagerStatCloseGroupByPlayer.BrandCode,
				PlayerCode: gameWagerStatCloseGroupByPlayer.PlayerCode,
				Username: gameWagerStatCloseGroupByPlayer.Username,
				CurrencyCode: gameWagerStatCloseGroupByPlayer.CurrencyCode,
				Count: gameWagerStatCloseGroupByPlayer.Count,
				Bet: gameWagerStatCloseGroupByPlayer.Bet,
				ValidBet: gameWagerStatCloseGroupByPlayer.ValidBet,
				Settle: gameWagerStatCloseGroupByPlayer.Settle,
				Commission: gameWagerStatCloseGroupByPlayer.Commission,
				Turnover: gameWagerStatCloseGroupByPlayer.Turnover,
				WinLoss: gameWagerStatCloseGroupByPlayer.WinLoss);
		}
	}

	[HttpGet("{brand}/playerCodes", Name = "BrandWagerPlayerMinsStats_PlayerCodes_Query")]
	public async IAsyncEnumerable<BrandWagerPlayerStatsInfo> PlayerCodesQueryAsync(
		[FromServices] IGameWagerStatCloseRepository repository,
		string brand,
		[FromQuery] DateTimeOffset startOn,
		[FromQuery] DateTimeOffset endOn,
		[FromQuery] IEnumerable<string> playerCode)
	{
		var gameWagerStatCloseGroupByPlayers = await repository.QueryFilterPlayerGroupByPlayerAsync(
			brandCode: brand,
			startOn: startOn,
			endOn: endOn,
			playerCodes: playerCode,
			cancellationToken: HttpContext.RequestAborted)
			.ToArrayAsync(
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false)
			?? Enumerable.Empty<GameWagerStatCloseGroupByPlayer>();

		if (!gameWagerStatCloseGroupByPlayers.Any())
		{
			yield break;
		}

		foreach (var gameWagerStatCloseGroupByPlayer in gameWagerStatCloseGroupByPlayers)
		{
			yield return new BrandWagerPlayerStatsInfo(
				ResellerId: gameWagerStatCloseGroupByPlayer.ResellerId,
				ResellerCode: gameWagerStatCloseGroupByPlayer.ResellerCode,
				LicenseeId: gameWagerStatCloseGroupByPlayer.LicenseeId,
				LicenseeCode: gameWagerStatCloseGroupByPlayer.LicenseeCode,
				BrandId: gameWagerStatCloseGroupByPlayer.BrandId,
				BrandCode: gameWagerStatCloseGroupByPlayer.BrandCode,
				PlayerCode: gameWagerStatCloseGroupByPlayer.PlayerCode,
				Username: gameWagerStatCloseGroupByPlayer.Username,
				CurrencyCode: gameWagerStatCloseGroupByPlayer.CurrencyCode,
				Count: gameWagerStatCloseGroupByPlayer.Count,
				Bet: gameWagerStatCloseGroupByPlayer.Bet,
				ValidBet: gameWagerStatCloseGroupByPlayer.ValidBet,
				Settle: gameWagerStatCloseGroupByPlayer.Settle,
				Commission: gameWagerStatCloseGroupByPlayer.Commission,
				Turnover: gameWagerStatCloseGroupByPlayer.Turnover,
				WinLoss: gameWagerStatCloseGroupByPlayer.WinLoss);
		}
	}
}
