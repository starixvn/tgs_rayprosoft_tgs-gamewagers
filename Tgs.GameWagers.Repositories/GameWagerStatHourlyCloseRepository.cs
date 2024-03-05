using Microsoft.EntityFrameworkCore;
using Tgs.GameWagers.Core;
using Tgs.GameWagers.Core.Models;
using Tgs.GameWagers.Repositories.Entities;

namespace Tgs.GameWagers.Repositories;

internal class GameWagerStatHourlyCloseRepository(
	TgsContext context) : IGameWagerStatHourlyCloseRepository
{
	public async ValueTask<GameWagerStatPlayerAccumModel?> GetPlayerAccumAsync(string playerCode, DateTimeOffset startOn, DateTimeOffset endOn, CancellationToken cancellationToken = default)
		=> await context.GameWagerStatHourlyCloses
		.Where(predicate: stat => stat.StatOn >= startOn.UtcDateTime &&
		stat.StatOn < endOn.UtcDateTime &&
		stat.PlayerCode == playerCode)
		.GroupBy(keySelector: stat => stat.PlayerCode)
		.Select(selector: stat => new GameWagerStatPlayerAccumModel(
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

	public async ValueTask<GameWagerStatPlayerAccumModel?> GetPlayerAccumAsync(string playerCode, DateOnly dateOn, TimeOnly endTimeOn, CancellationToken cancellationToken = default)
		=> await context.GameWagerStatHourlyCloses
		.Where(predicate: stat => stat.StatDayOn == dateOn &&
		stat.StatHourOn < endTimeOn &&
		stat.PlayerCode == playerCode)
		.GroupBy(keySelector: stat => stat.PlayerCode)
		.Select(selector: stat => new GameWagerStatPlayerAccumModel(
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
