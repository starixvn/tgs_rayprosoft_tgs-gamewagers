namespace Tgs.GameWagers.Common;

public record WagerGameMinsStatsInfo(
	Guid GameProviderId,
	string GameProviderCode,
	int GameId,
	string CurrencyCode,
	int Count,
	decimal Bet,
	decimal ValidBet,
	decimal Settle,
	decimal Commission,
	decimal Turnover,
	decimal WinLoss);
