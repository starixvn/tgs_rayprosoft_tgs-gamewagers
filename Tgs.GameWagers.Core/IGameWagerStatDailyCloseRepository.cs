using Tgs.GameWagers.Core.Models;

namespace Tgs.GameWagers.Core;

public interface IGameWagerStatDailyCloseRepository
{
	ValueTask<GameWagerStatPlayerAccumModel?> GetPlayerAccumAsync(string playerCode, DateOnly endDayOn, CancellationToken cancellationToken = default);
}
