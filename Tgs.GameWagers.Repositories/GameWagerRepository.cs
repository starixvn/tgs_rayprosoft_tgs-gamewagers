using Tgs.GameWagers.Core;
using Tgs.GameWagers.Repositories.Entities;

namespace Tgs.GameWagers.Repositories;

internal class GameWagerRepository(TgsContext context) : IGameWagerRepository
{
    public IAsyncEnumerable<Core.Models.GameWager> QueryAsync(string brandCode, DateTimeOffset createdStartOn, DateTimeOffset createdEndOn, CancellationToken cancellationToken = default) => throw new NotImplementedException();
}
