using Tgs.GameWagers.Core.Models;

namespace Tgs.GameWagers.Core;

public interface IGameWagerStatDailyCloseRepository
{
	ValueTask<GameWagerStatPlayerAccum?> GetPlayerAccumAsync(string playerCode, DateTimeOffset endOn, CancellationToken cancellationToken = default);
}
