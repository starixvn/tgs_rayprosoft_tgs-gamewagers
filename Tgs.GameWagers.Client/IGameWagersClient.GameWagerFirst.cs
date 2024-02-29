using Tgs.GameWagers.Common;

namespace Tgs.GameWagers.Client;

public partial interface IGameWagersClient
{
	ValueTask<GameWagerFirstInfo?> GetAsync(long wagerId, DateTimeOffset createdOn, CancellationToken cancellationToken = default);

	ValueTask<GameWagerFirstInfo?> GetAsync(Guid gameProviderId, int gameId, Guid walletId, string externalWagerId, DateTimeOffset createdOn, CancellationToken cancellationToken = default);
}
