using Tgs.GameWagers.Common;

namespace Tgs.GameWagers.Core.ApplicationServices;

public record BrandWagerPlayerStatsQuery(
	string BrandCode,
	DateTimeOffset StartOn,
	DateTimeOffset EndOn,
	BrandWagerPlayerMinsStatsSort Sort,
	bool IsAscending = true,
	int Page = 1,
	int PageSize = 1000);
