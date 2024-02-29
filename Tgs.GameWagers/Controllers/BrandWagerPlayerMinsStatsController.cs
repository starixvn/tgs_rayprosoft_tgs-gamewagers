using Microsoft.AspNetCore.Mvc;
using Tgs.GameWagers.Common;
using Tgs.GameWagers.Core;
using Tgs.GameWagers.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameWagers.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BrandWagerPlayerMinsStatsController : ControllerBase
{
	[HttpGet("{brand}", Name = "BrandWagerPlayerMinsStats_Query")]
	public async IAsyncEnumerable<BrandWagerPlayerStatsInfo> QueryAsync(
		[FromServices] IQueryService<BrandWagerPlayerStatsQuery, BrandWagerPlayerStatsReslut> service,
		string brand,
		[FromQuery] DateTimeOffset startOn,
		[FromQuery] DateTimeOffset endOn,
		[FromQuery] BrandWagerPlayerMinsStatsSort sort,
		[FromQuery] bool isAscending = true,
		[FromQuery] int page = 1,
		[FromQuery] int pageSize = 1000)
	{
		var query = new BrandWagerPlayerStatsQuery(
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
			yield return new BrandWagerPlayerStatsInfo(
				ResellerId: reslut.ResellerId,
				ResellerCode: reslut.ResellerCode,
				LicenseeId: reslut.LicenseeId,
				LicenseeCode: reslut.LicenseeCode,
				BrandId: reslut.BrandId,
				BrandCode: reslut.BrandCode,
				PlayerCode: reslut.PlayerCode,
				Username: reslut.Username,
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

	[HttpGet("{brand}/playerCodes", Name = "BrandWagerPlayerMinsStats_PlayerCodes_Query")]
	public IAsyncEnumerable<BrandWagerPlayerStatsInfo> PlayerCodesQueryAsync(
		[FromServices] IBrandWagerPlayerMinsStatsRepository repository,
		string brand,
		[FromQuery] DateTimeOffset startOn,
		[FromQuery] DateTimeOffset endOn,
		[FromQuery] IEnumerable<string> playerCode)
		=> repository
		.PlayerCodesQueryAsync(
			brandCode: brand,
			startOn: startOn,
			endOn: endOn,
			playerCodes: playerCode,
			cancellationToken: HttpContext.RequestAborted);
}
