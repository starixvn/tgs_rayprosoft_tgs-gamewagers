using Tgs.GameWagers.Common;
using Tgs.GameWagers.Core.Models;

namespace Tgs.GameWagers.Core;

public interface IGameWagerStatCloseRepository
{
	ValueTask<GameWagerStatPlayerAccum?> GetPlayerAccumAsync(string playerCode, DateTimeOffset startOn, DateTimeOffset endOn, CancellationToken cancellationToken = default);

	IAsyncEnumerable<GameWagerStatCloseGroupByGame> QueryGroupByGameAsync(string brandCode, DateTimeOffset startOn, DateTimeOffset endOn, BrandWagerGameMinsStatsSort sort, bool isAscending = true, int offset = 0, int limit = 20, CancellationToken cancellationToken = default);

	IAsyncEnumerable<GameWagerStatCloseGroupByPlayer> QueryGroupByPlayerAsync(string brandCode, DateTimeOffset startOn, DateTimeOffset endOn, BrandWagerPlayerMinsStatsSort sort, bool isAscending = true, int offset = 0, int limit = 20, CancellationToken cancellationToken = default);

	IAsyncEnumerable<GameWagerStatCloseGroupByPlayer> QueryFilterPlayerGroupByPlayerAsync(string brandCode, DateTimeOffset startOn, DateTimeOffset endOn, IEnumerable<string> playerCodes, CancellationToken cancellationToken = default);
}
