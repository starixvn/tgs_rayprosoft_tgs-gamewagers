using Microsoft.AspNetCore.Mvc;
using Tgs.GameWagers.Common;
using Tgs.GameWagers.Core;

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
}
