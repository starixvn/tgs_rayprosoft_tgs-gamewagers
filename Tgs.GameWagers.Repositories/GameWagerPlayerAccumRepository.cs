using Microsoft.EntityFrameworkCore;
using Tgs.GameWagers.Core;
using Tgs.GameWagers.Core.Models;
using Tgs.GameWagers.Repositories.Entities;

namespace Tgs.GameWagers.Repositories;

internal class GameWagerPlayerAccumRepository(TgsContext context) : IGameWagerPlayerAccumRepository
{
	public async ValueTask<GameWagerPlayerAccumModel?> GetNewestAsync(string playerCode, CancellationToken cancellationToken = default)
		=> await context.GameWagerPlayerAccums
		.Where(predicate: accum => accum.PlayerCode == playerCode)
		.OrderByDescending(keySelector: accum => accum.AccumOn)
		.Select(selector: accum => new GameWagerPlayerAccumModel(
			accum.PlayerCode,
			accum.AccumOn,
			accum.Count,
			accum.Bet,
			accum.ValidBet,
			accum.Settle,
			accum.Commission,
			accum.Turnover,
			accum.WinLoss))
		.FirstOrDefaultAsync(
			cancellationToken: cancellationToken)
		.ConfigureAwait(false);
}
