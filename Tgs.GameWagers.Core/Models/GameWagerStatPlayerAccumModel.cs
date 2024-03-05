namespace Tgs.GameWagers.Core.Models;

public record GameWagerStatPlayerAccumModel(
	string PlayerCode,
	int Count,
	decimal Bet,
	decimal ValidBet,
	decimal Settle,
	decimal Commission,
	decimal Turnover,
	decimal WinLoss);
