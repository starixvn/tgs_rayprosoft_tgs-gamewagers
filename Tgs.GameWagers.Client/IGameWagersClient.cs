using Tgs.GameWagers.Common;

namespace Tgs.GameWagers.Client;

public interface IGameWagersClient
{
    IAsyncEnumerable<BrandWagerGameStatsInfo> BrandWagerGameStatsQueryAsync(string brandCode, DateTimeOffset startOn, DateTimeOffset endOn, BrandWagerGameMinsStatsSort sort, bool isAscending = true, int page = 1, int pageSize = 1000, CancellationToken cancellationToken = default);

    IAsyncEnumerable<BrandWagerPlayerStatsInfo> BrandWagerPlayerStatsQueryAsync(string brandCode, DateTimeOffset startOn, DateTimeOffset endOn, BrandWagerPlayerMinsStatsSort sort, bool isAscending = true, int page = 1, int pageSize = 1000, CancellationToken cancellationToken = default);

    IAsyncEnumerable<BrandWagerPlayerStatsInfo> BrandWagerPlayerStatsPlayerQueryAsync(string brandCode, DateTimeOffset startOn, DateTimeOffset endOn, IEnumerable<string> playerCodes, CancellationToken cancellationToken = default);

    ValueTask<GameWagerStatPlayerAccumInfo> GameWagerStatGetPlayerAccumAsync(string playerCode, CancellationToken cancellationToken = default);
}
