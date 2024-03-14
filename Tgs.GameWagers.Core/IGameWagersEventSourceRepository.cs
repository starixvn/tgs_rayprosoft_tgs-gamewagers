using Tgs.GameWagers.Core.Models;

namespace Tgs.GameWagers.Core;

public interface IGameWagersEventSourceRepository
{
	IAsyncEnumerable<GameWagersEventSourceModel> QuerySettlementsAsync(Guid gameProviderId, Guid id, int limit = 100, CancellationToken cancellationToken = default);

	IAsyncEnumerable<GameWagersEventSourceModel> QueryAllAsync(Guid gameProviderId, Guid id, int limit = 100, CancellationToken cancellationToken = default);

	IAsyncEnumerable<GameWagersEventSourceModel> QuerySettlementsAsync(Guid gameProviderId, DateTimeOffset fromOn, DateTimeOffset toOn, int offset = 0, int limit = 100, CancellationToken cancellationToken = default);

	IAsyncEnumerable<GameWagersEventSourceModel> QuerySettlementsAsync(Guid gameProviderId, DateTimeOffset timepoint, int limit = 100, CancellationToken cancellationToken = default);
}
