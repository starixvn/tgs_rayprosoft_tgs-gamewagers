using Microsoft.AspNetCore.Mvc;
using Tgs.GameWagers.Common;
using Tgs.GameWagers.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameWagers.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BrandWagerGameMinsStatsController : ControllerBase
{
	[HttpGet("{brand}", Name = "BrandWagerGameMinsStats_Query")]
	public async IAsyncEnumerable<BrandWagerGameStatsInfo> QueryAsync(
		[FromServices] IQueryService<BrandWagerGameStatsQuery, BrandWagerGameStatsReslut> service,
		string brand,
	   [FromQuery] DateTimeOffset startOn,
	   [FromQuery] DateTimeOffset endOn,
	   [FromQuery] BrandWagerGameMinsStatsSort sort,
	   [FromQuery] bool isAscending = true,
	   [FromQuery] int page = 1,
	   [FromQuery] int pageSize = 1000)
	{
		var query = new BrandWagerGameStatsQuery(
			BrandCode: brand,
			StartOn: startOn,
			EndOn: endOn,
			Sort: sort,
			IsAscending: isAscending,
			Page: page,
			PageSize: pageSize);
		var resluts = await service
			.QueryAsync(
			query: query,
			cancellationToken: HttpContext.RequestAborted)
			.ToArrayAsync(
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);

		if (resluts.Length == 0)
		{
			yield break;
		}

		foreach (var reslut in resluts)
		{
			yield return new BrandWagerGameStatsInfo(
				GameProviderId: reslut.GameProviderId,
				GameProviderCode: reslut.GameProviderCode,
				GameId: reslut.GameId,
				ResellerId: reslut.ResellerId,
				ResellerCode: reslut.ResellerCode,
				LicenseeId: reslut.LicenseeId,
				LicenseeCode: reslut.LicenseeCode,
				BrandId: reslut.BrandId,
				BrandCode: reslut.BrandCode,
				CurrencyCode: reslut.CurrencyCode,
				Count: reslut.Count,
				Bet: reslut.Bet,
				ValidBet: reslut.ValidBet,
				Settle: reslut.Settle,
				Commission: reslut.Commission,
				Turnover: reslut.Turnover,
				WinLoss: reslut.WinLoss
				);
		}
	}
}
