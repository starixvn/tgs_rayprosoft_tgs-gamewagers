namespace Tgs.GameWagers.Core.Models;
public record GameWagerPlayerAccumModel(
	string PlayerCode,
	DateTimeOffset AccumOn,
	int Count,
	decimal Bet,
	decimal ValidBet,
	decimal Settle,
	decimal Commission,
	decimal Turnover,
	decimal WinLoss);
