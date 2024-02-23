using Microsoft.AspNetCore.Mvc;
using Tgs.GameWagers.Common;
using Tgs.GameWagers.Core.ApplicationServices;
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
}
