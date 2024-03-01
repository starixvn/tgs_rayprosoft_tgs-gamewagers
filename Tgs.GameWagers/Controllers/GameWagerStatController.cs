using Microsoft.AspNetCore.Mvc;
using Tgs.GameWagers.Common;
using Tgs.GameWagers.Core;
using Tgs.GameWagers.Core.ApplicationServices;
using Tgs.GameWagers.Core.Models;
using Tgs.ServiceUtility;

namespace Tgs.GameWagers.Controllers;

[Route("stat")]
[ApiController]
public class GameWagerStatController : ControllerBase
{
	[HttpGet("{playerCode}/accum", Name = "GameWager_Stat_PlayerAccum_Get")]
	public ValueTask<GameWagerStatPlayerAccumInfo> GetPlayerAccumAsync(
		[FromServices] IGetService<GameWagerStatPlayerAccumQuery, GameWagerStatPlayerAccumInfo> service,
		string playerCode)
		=> service.GetAsync(
			query: new GameWagerStatPlayerAccumQuery(playerCode),
			cancellationToken: HttpContext.RequestAborted);

	[HttpGet("{brand}/query/group/game", Name = "GameWager_Stat_Query_GroupBy_Game")]
	public IAsyncEnumerable<GameWagerStatCloseGroupByGame> GameWagerFirstQueryGroupByGameAsync(
		[FromServices] IGameWagerStatCloseRepository repository,
		string brand,
		[FromQuery] DateTimeOffset startOn,
		[FromQuery] DateTimeOffset endOn,
		[FromQuery] BrandWagerGameMinsStatsSort sort,
		[FromQuery] bool isAscending,
		[FromQuery] int pageIndex = 1,
		[FromQuery] int pageSize = 20)
		=> repository.QueryGroupByGameAsync(
		brandCode: brand,
		startOn: startOn,
		endOn: endOn,
		sort: sort,
		isAscending: isAscending,
		offset: pageIndex - 1,
		limit: pageSize,
		cancellationToken: HttpContext.RequestAborted);

	[HttpGet("{brand}/query/group/player", Name = "GameWager_Stat_Query_GroupBy_player")]
	public IAsyncEnumerable<GameWagerStatCloseGroupByPlayer> GameWagerFirstQueryGroupByPlayerAsync(
		[FromServices] IGameWagerStatCloseRepository repository,
		string brand,
		[FromQuery] DateTimeOffset startOn,
		[FromQuery] DateTimeOffset endOn,
		[FromQuery] BrandWagerPlayerMinsStatsSort sort,
		[FromQuery] bool isAscending,
		[FromQuery] int pageIndex = 1,
		[FromQuery] int pageSize = 20)
		=> repository.QueryGroupByPlayerAsync(
		brandCode: brand,
		startOn: startOn,
		endOn: endOn,
		sort: sort,
		isAscending: isAscending,
		offset: pageIndex - 1,
		limit: pageSize,
		cancellationToken: HttpContext.RequestAborted);

	[HttpGet("{brand}/query/filter/player/group/players", Name = "GameWager_Stat_Query_Filter_Player_GroupBy_Player")]
	public IAsyncEnumerable<GameWagerStatCloseGroupByPlayer> GameWagerFirstQueryFilterPlayerGroupByPlayerAsync(
		[FromServices] IGameWagerStatCloseRepository repository,
		string brand,
		[FromQuery] DateTimeOffset startOn,
		[FromQuery] DateTimeOffset endOn,
		[FromQuery] string[] playerCodes)
		=> repository.QueryFilterPlayerGroupByPlayerAsync(
		brandCode: brand,
		startOn: startOn,
		endOn: endOn,
		playerCodes: playerCodes,
		cancellationToken: HttpContext.RequestAborted);
}
