using Tgs.GameWagers.Core.Models;

namespace Tgs.GameWagers.Core;

public interface IGameBetEventSourceRepository
{
	ValueTask<GameBetEventSourceModel?> GetAsync(Guid id, DateTimeOffset createdOn, CancellationToken cancellationToken = default);

	IAsyncEnumerable<GameBetEventSourceModel> QueryAllAsync(Guid gameProviderId, Guid id, int limit = 100, CancellationToken cancellationToken = default);

	IAsyncEnumerable<GameBetEventSourceModel> QueryAllAsync(Guid gameProviderId, DateTimeOffset fromOn, DateTimeOffset toOn, int offset = 0, int limit = 100, CancellationToken cancellationToken = default);

	IAsyncEnumerable<GameBetEventSourceModel> QuerySettlementsAsync(Guid gameProviderId, Guid id, int limit = 100, CancellationToken cancellationToken = default);

	IAsyncEnumerable<GameBetEventSourceModel> QuerySettlementsAsync(Guid gameProviderId, DateTimeOffset fromOn, DateTimeOffset toOn, int offset = 0, int limit = 100, CancellationToken cancellationToken = default);

	IAsyncEnumerable<GameBetEventSourceModel> QuerySettlementsAsync(Guid gameProviderId, DateTimeOffset timepoint, int limit = 100, CancellationToken cancellationToken = default);
}
