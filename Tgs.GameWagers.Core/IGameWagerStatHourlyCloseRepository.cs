using Tgs.GameWagers.Core.Models;

namespace Tgs.GameWagers.Core;

public interface IGameWagerStatHourlyCloseRepository
{
	ValueTask<GameWagerStatPlayerAccum?> GetPlayerAccumAsync(string playerCode, DateTimeOffset startOn, DateTimeOffset endOn, CancellationToken cancellationToken = default);

	ValueTask<GameWagerStatPlayerAccum?> GetPlayerAccumAsync(string playerCode, DateOnly dateOn, TimeOnly endTimeOn, CancellationToken cancellationToken = default);
}
