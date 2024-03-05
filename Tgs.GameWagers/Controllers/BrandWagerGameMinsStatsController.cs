using Microsoft.AspNetCore.Mvc;
using Tgs.GameWagers.Common;
using Tgs.GameWagers.Core;
using Tgs.GameWagers.Core.Models;

namespace Tgs.GameWagers.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BrandWagerGameMinsStatsController : ControllerBase
{
	[HttpGet("{brand}", Name = "BrandWagerGameMinsStats_Query")]
	public async IAsyncEnumerable<BrandWagerGameStatsInfo> QueryAsync(
		[FromServices] IGameWagerStatCloseRepository repository,
		string brand,
	   [FromQuery] DateTimeOffset startOn,
	   [FromQuery] DateTimeOffset endOn,
	   [FromQuery] BrandWagerGameMinsStatsSort sort,
	   [FromQuery] bool isAscending = true,
	   [FromQuery] int page = 1,
	   [FromQuery] int pageSize = 1000)
	{
		var gameWagerStatCloseGroupByGames = await repository.QueryGroupByGameAsync(
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
			?? Enumerable.Empty<GameWagerStatCloseGroupByGame>();

		if (!gameWagerStatCloseGroupByGames.Any())
		{
			yield break;
		}

		foreach (var gameWagerStatCloseGroupByGame in gameWagerStatCloseGroupByGames)
		{
			yield return new BrandWagerGameStatsInfo(
				GameProviderId: gameWagerStatCloseGroupByGame.GameProviderId,
				GameProviderCode: gameWagerStatCloseGroupByGame.GameProviderCode,
				GameId: gameWagerStatCloseGroupByGame.GameId,
				ResellerId: gameWagerStatCloseGroupByGame.ResellerId,
				ResellerCode: gameWagerStatCloseGroupByGame.ResellerCode,
				LicenseeId: gameWagerStatCloseGroupByGame.LicenseeId,
				LicenseeCode: gameWagerStatCloseGroupByGame.LicenseeCode,
				BrandId: gameWagerStatCloseGroupByGame.BrandId,
				BrandCode: gameWagerStatCloseGroupByGame.BrandCode,
				CurrencyCode: gameWagerStatCloseGroupByGame.CurrencyCode,
				Count: gameWagerStatCloseGroupByGame.Count,
				Bet: gameWagerStatCloseGroupByGame.Bet,
				ValidBet: gameWagerStatCloseGroupByGame.ValidBet,
				Settle: gameWagerStatCloseGroupByGame.Settle,
				Commission: gameWagerStatCloseGroupByGame.Commission,
				Turnover: gameWagerStatCloseGroupByGame.Turnover,
				WinLoss: gameWagerStatCloseGroupByGame.WinLoss
				);
		}
	}
}
