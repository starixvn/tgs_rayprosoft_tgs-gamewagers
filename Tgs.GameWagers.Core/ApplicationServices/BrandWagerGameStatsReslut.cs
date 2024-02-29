namespace Tgs.GameWagers.Core.ApplicationServices;

public record BrandWagerGameStatsReslut(
	Guid GameProviderId,
	string GameProviderCode,
	int GameId,
	Guid ResellerId,
	string ResellerCode,
	Guid LicenseeId,
	string LicenseeCode,
	Guid BrandId,
	string BrandCode,
	string CurrencyCode,
	int Count,
	decimal Bet,
	decimal ValidBet,
	decimal Settle,
	decimal Commission,
	decimal Turnover,
	decimal WinLoss);
