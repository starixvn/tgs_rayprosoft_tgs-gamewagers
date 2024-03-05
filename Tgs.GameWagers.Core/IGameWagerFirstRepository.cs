using Tgs.GameWagers.Core.Models;

namespace Tgs.GameWagers.Core;

public interface IGameWagerFirstRepository
{
	ValueTask<GameWagerFirstModel?> GetAsync(long wagerId, DateTimeOffset createdOn, CancellationToken cancellationToken = default);

	ValueTask<GameWagerFirstModel?> GetAsync(Guid gameProviderId, int gameId, Guid walletId, string externalWagerId, DateTimeOffset createdOn, CancellationToken cancellationToken = default);
}
