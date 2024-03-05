using Tgs.GameWagers.Common;
using Tgs.GameWagers.Common.Enums;
using Tgs.GameWagers.Core.Models;

namespace Tgs.GameWagers.Core;

public interface IGameWagerStatCloseRepository
{
	ValueTask<GameWagerStatPlayerAccumModel?> GetPlayerAccumAsync(string playerCode, DateTimeOffset startOn, DateTimeOffset endOn, CancellationToken cancellationToken = default);

	IAsyncEnumerable<GameWagerStatCloseGroupByGameModel> QueryGroupByGameAsync(string brandCode, DateTimeOffset startOn, DateTimeOffset endOn, BrandWagerGameMinsStatsSort sort, bool isAscending = true, int offset = 0, int limit = 20, CancellationToken cancellationToken = default);

	IAsyncEnumerable<GameWagerStatCloseGroupByPlayerModel> QueryGroupByPlayerAsync(string brandCode, DateTimeOffset startOn, DateTimeOffset endOn, BrandWagerPlayerMinsStatsSort sort, bool isAscending = true, int offset = 0, int limit = 20, CancellationToken cancellationToken = default);

	IAsyncEnumerable<GameWagerStatCloseGroupByPlayerModel> QueryFilterPlayerGroupByPlayerAsync(string brandCode, DateTimeOffset startOn, DateTimeOffset endOn, IEnumerable<string> playerCodes, CancellationToken cancellationToken = default);
}
