using Tgs.GameWagers.Core.Models;

namespace Tgs.GameWagers.Core;

public interface IGameWagerPlayerAccumRepository
{
	public ValueTask<GameWagerPlayerAccumModel?> GetNewestAsync(string playerCode, CancellationToken cancellationToken = default);
}
