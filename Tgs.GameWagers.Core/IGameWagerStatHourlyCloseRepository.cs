using Tgs.GameWagers.Core.Models;

namespace Tgs.GameWagers.Core;

public interface IGameWagerStatHourlyCloseRepository
{
	ValueTask<GameWagerStatPlayerAccumModel?> GetPlayerAccumAsync(string playerCode, DateTimeOffset startOn, DateTimeOffset endOn, CancellationToken cancellationToken = default);

	ValueTask<GameWagerStatPlayerAccumModel?> GetPlayerAccumAsync(string playerCode, DateOnly dateOn, TimeOnly endTimeOn, CancellationToken cancellationToken = default);
}
