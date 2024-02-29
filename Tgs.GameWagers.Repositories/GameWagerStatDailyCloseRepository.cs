using Microsoft.EntityFrameworkCore;
using Tgs.GameWagers.Core;
using Tgs.GameWagers.Core.Models;
using Tgs.GameWagers.Repositories.Entities;

namespace Tgs.GameWagers.Repositories;

internal class GameWagerStatDailyCloseRepository(
	TgsContext context) : IGameWagerStatDailyCloseRepository
{
	public async ValueTask<GameWagerStatPlayerAccum?> GetPlayerAccumAsync(string playerCode, DateTimeOffset endOn, CancellationToken cancellationToken = default)
		=> await context.GameWagerStatDailyCloses
		.Where(predicate: stat => stat.StatOn < endOn &&
		stat.PlayerCode == playerCode)
		.GroupBy(keySelector: stat => stat.PlayerCode)
		.Select(selector: stat => new GameWagerStatPlayerAccum(
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
}
