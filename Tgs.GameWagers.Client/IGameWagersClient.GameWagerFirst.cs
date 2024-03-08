using Tgs.GameWagers.Common;

namespace Tgs.GameWagers.Client;

public partial interface IGameWagersClient
{
	ValueTask<GameWagerFirstInfo?> GameWagerFirstGetAsync(long wagerId, DateTimeOffset createdOn, CancellationToken cancellationToken = default);

	ValueTask<GameWagerFirstInfo?> GameWagerFirstGetAsync(Guid gameProviderId, int gameId, Guid walletId, string externalWagerId, DateTimeOffset createdOn, CancellationToken cancellationToken = default);
}
