using Tgs.GameWagers.Common;

namespace Tgs.GameWagers.Client;

public partial interface IGameWagersClient
{
	IAsyncEnumerable<GameWagersEventSourceInfo> GameWagersEventSourceQueryAllAsync(Guid gameProviderId, Guid id, int limit = 100, CancellationToken cancellationToken = default);

	IAsyncEnumerable<GameWagersEventSourceInfo> GameWagersEventSourceQuerySettlementsAsync(Guid gameProviderId, Guid id, int limit = 100, CancellationToken cancellationToken = default);

	IAsyncEnumerable<GameWagersEventSourceInfo> GameWagersEventSourceQuerySettlementsAsync(Guid gameProviderId, DateTimeOffset timepoint, int limit = 100, CancellationToken cancellationToken = default);

	IAsyncEnumerable<GameWagersEventSourceInfo> GameWagersEventSourceQuerySettlementsAsync(Guid gameProviderId, DateTimeOffset fromOn, DateTimeOffset toOn, int page = 1, int pageSize = 100, CancellationToken cancellationToken = default);
}
