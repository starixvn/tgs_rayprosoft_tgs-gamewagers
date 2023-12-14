using Tgs.GameWagers.Common;

namespace Tgs.GameWagers.Core;

public interface IGameWagerFirstRepository
{
    IAsyncEnumerable<GameWagerFirstBrandGameStatsInfo> BrandGameStatsQueryAsync(string brandCode, DateTimeOffset externalClosedStartOn, DateTimeOffset externalClosedEndOn, CancellationToken cancellationToken = default);

    IAsyncEnumerable<GameWagerFirstBrandPlayerStatsInfo> BrandPlayerStatsQueryAsync(string brandCode, DateTimeOffset externalClosedStartOn, DateTimeOffset externalClosedEndOn, CancellationToken cancellationToken = default);

    IAsyncEnumerable<GameWagerFirstGameStatsInfo> GameStatsQueryAsync(string gameProviderCode, DateTimeOffset externalClosedStartOn, DateTimeOffset externalClosedEndOn, CancellationToken cancellationToken = default);
}
