using Tgs.GameWagers.Common;

namespace Tgs.GameWagers.Client;

public partial interface IGameWagersClient
{
	ValueTask<GameBetsEventSourceInfo?> GameBetsEventSourceGetAsync(Guid id, DateTimeOffset createdOn, CancellationToken cancellationToken = default);

	IAsyncEnumerable<GameBetsEventSourceInfo> GameBetsEventSourceQueryAllAsync(Guid gameProviderId, Guid id, int limit = 100, CancellationToken cancellationToken = default);

	IAsyncEnumerable<GameBetsEventSourceInfo> GameBetsEventSourceQueryAllAsync(Guid gameProviderId, DateTimeOffset fromOn, DateTimeOffset toOn, int page = 1, int pageSize = 100, CancellationToken cancellationToken = default);

	IAsyncEnumerable<GameBetsEventSourceInfo> GameBetsEventSourceQuerySettlementsAsync(Guid gameProviderId, Guid id, int limit = 100, CancellationToken cancellationToken = default);

	IAsyncEnumerable<GameBetsEventSourceInfo> GameBetsEventSourceQuerySettlementsAsync(Guid gameProviderId, DateTimeOffset timepoint, int limit = 100, CancellationToken cancellationToken = default);

	IAsyncEnumerable<GameBetsEventSourceInfo> GameBetsEventSourceQuerySettlementsAsync(Guid gameProviderId, DateTimeOffset fromOn, DateTimeOffset toOn, int page = 1, int pageSize = 100, CancellationToken cancellationToken = default);
}
