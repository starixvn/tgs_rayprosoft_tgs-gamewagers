using Amazon.Runtime;
using Microsoft.AspNetCore.Mvc;
using Tgs.GameWagers.Common;
using Tgs.GameWagers.Core;
using Tgs.GameWagers.Core.Models;

namespace Tgs.GameWagers.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GameWagerFirstController : ControllerBase
{
	[HttpGet("{brand}/brand/game/stats", Name = "GameWagerFirst_Brand_Game_Stats_Query")]
	public IAsyncEnumerable<GameWagerFirstBrandGameStatsInfo> BrandGameStatsQueryAsync(
		[FromServices] IGameWagerFirstRepository repository,
		string brand,
		[FromQuery] DateTimeOffset startOn,
		[FromQuery] DateTimeOffset endOn)
		=> repository
		.BrandGameStatsQueryAsync(
		brandCode: brand,
		externalClosedStartOn: startOn,
		externalClosedEndOn: endOn,
		cancellationToken: HttpContext.RequestAborted);

	[HttpGet("{brand}/brand/player/stats", Name = "GameWagerFirst_Brand_Player_Stats_Query")]
	public IAsyncEnumerable<GameWagerFirstBrandPlayerStatsInfo> BrandPlayerStatsQueryAsync(
		[FromServices] IGameWagerFirstRepository repository,
		string brand,
		[FromQuery] DateTimeOffset startOn,
		[FromQuery] DateTimeOffset endOn)
		=> repository
		.BrandPlayerStatsQueryAsync(
		brandCode: brand,
		externalClosedStartOn: startOn,
		externalClosedEndOn: endOn,
		cancellationToken: HttpContext.RequestAborted);

	[HttpGet("{gameProviderCode}/game/stats", Name = "GameWagerFirst_Game_Stats_Query")]
	public IAsyncEnumerable<GameWagerFirstGameStatsInfo> GameStatsQueryAsync(
		[FromServices] IGameWagerFirstRepository repository,
		string gameProviderCode,
		[FromQuery] DateTimeOffset startOn,
		[FromQuery] DateTimeOffset endOn)
		=> repository
		.GameStatsQueryAsync(
		gameProviderCode: gameProviderCode,
		externalClosedStartOn: startOn,
		externalClosedEndOn: endOn,
		cancellationToken: HttpContext.RequestAborted);

	[HttpGet("{brand}/query/group/game", Name = "GameWagerFirst_Query_GroupBy_Game")]
	public IAsyncEnumerable<GameWagerFirstGroupByGame> GameWagerFirstQueryGroupByGameAsync(
		[FromServices] IGameWagerFirstRepository repository,
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

	[HttpGet("{brand}/query/group/player", Name = "GameWagerFirst_Query_GroupBy_player")]
	public IAsyncEnumerable<GameWagerFirstGroupByPlayer> GameWagerFirstQueryGroupByPlayerAsync(
		[FromServices] IGameWagerFirstRepository repository,
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

	[HttpGet("{brand}/query/filter/player/group/players", Name = "GameWagerFirst_Query_Filter_Player_GroupBy_Player")]
	public IAsyncEnumerable<GameWagerFirstGroupByPlayer> GameWagerFirstQueryFilterPlayerGroupByPlayerAsync(
		[FromServices] IGameWagerFirstRepository repository,
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
