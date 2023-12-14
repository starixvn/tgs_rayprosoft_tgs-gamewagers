using Tgs.GameWagers.Common;
using Tgs.GameWagers.Core.Models;

namespace Tgs.GameWagers.Core;

public interface IBrandWagerPlayerMinsStatsRepository
{
    IAsyncEnumerable<BrandWagerPlayerStats> QueryAsync(string brandCode, DateTimeOffset startOn, DateTimeOffset endOn, BrandWagerPlayerMinsStatsSort sort, bool isAscending = true, CancellationToken cancellationToken = default);

    IAsyncEnumerable<BrandWagerPlayerStatsInfo> PlayerCodesQueryAsync(string brandCode, DateTimeOffset startOn, DateTimeOffset endOn, IEnumerable<string> playerCodes, CancellationToken cancellationToken = default);
}
