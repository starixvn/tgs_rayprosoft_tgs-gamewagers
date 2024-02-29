using Tgs.GameWagers.Common;

namespace Tgs.GameWagers.Core.ApplicationServices;

public record BrandWagerGameStatsQuery(
	string BrandCode,
	DateTimeOffset StartOn,
	DateTimeOffset EndOn,
	BrandWagerGameMinsStatsSort Sort,
	bool IsAscending = true,
	int Page = 1,
	int PageSize = 1000);
