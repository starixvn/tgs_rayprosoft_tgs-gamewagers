using Tgs.GameWagers.Core.Models;

namespace Tgs.GameWagers.Core;

public interface IGameWagerRepository
{
	ValueTask<GameWagerModel?> GetAsync(long wagerId, DateTimeOffset createdOn, CancellationToken cancellationToken = default);

	ValueTask<GameWagerModel?> GetAsync(Guid gameProviderId, int gameId, Guid walletId, string externalWagerId, DateTimeOffset createdOn, CancellationToken cancellationToken = default);
}
