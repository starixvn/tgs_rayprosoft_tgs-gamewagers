using Tgs.GameWagers.Core.Models;

namespace Tgs.GameWagers.Core;

public interface IGameWagerRepository
{
    IAsyncEnumerable<GameWager> QueryAsync(string brandCode, DateTimeOffset createdStartOn, DateTimeOffset createdEndOn, CancellationToken cancellationToken = default);
}
