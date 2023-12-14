using Tgs.GameWagers.Common;
using Tgs.GameWagers.Core.Models;

namespace Tgs.GameWagers.Core;

public interface IBrandWagerGameMinsStatsRepository
{
    IAsyncEnumerable<BrandWagerGameMinsStats> QueryAsync(string brandCode, DateTimeOffset startOn, DateTimeOffset endOn, BrandWagerGameMinsStatsSort sort, bool isAscending = true, CancellationToken cancellationToken = default);
}
