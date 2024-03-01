using System.Runtime.CompilerServices;
using Tgs.GameWagers.Common;
using Tgs.GameWagers.Core.Models;

namespace Tgs.GameWagers.Core;

public interface IGameWagerFirstRepository
{
	IAsyncEnumerable<GameWagerFirstBrandGameStatsInfo> BrandGameStatsQueryAsync(string brandCode, DateTimeOffset externalClosedStartOn, DateTimeOffset externalClosedEndOn, CancellationToken cancellationToken = default);

	IAsyncEnumerable<GameWagerFirstBrandPlayerStatsInfo> BrandPlayerStatsQueryAsync(string brandCode, DateTimeOffset externalClosedStartOn, DateTimeOffset externalClosedEndOn, CancellationToken cancellationToken = default);

	IAsyncEnumerable<GameWagerFirstGameStatsInfo> GameStatsQueryAsync(string gameProviderCode, DateTimeOffset externalClosedStartOn, DateTimeOffset externalClosedEndOn, CancellationToken cancellationToken = default);

	ValueTask<GameWagerFirstModel?> GetAsync(long wagerId, DateTimeOffset createdOn, CancellationToken cancellationToken = default);

	ValueTask<GameWagerFirstModel?> GetAsync(Guid gameProviderId, int gameId, Guid walletId, string externalWagerId, DateTimeOffset createdOn, CancellationToken cancellationToken = default);
}
