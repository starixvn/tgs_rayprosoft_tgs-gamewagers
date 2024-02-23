namespace Tgs.GameWagers.Common;

public record GameWagerStatPlayerAccumInfo(
    string PlayerCode,
     int Count,
     decimal Bet,
     decimal ValidBet,
     decimal Settle,
     decimal Commission,
     decimal Turnover,
     decimal WinLoss);
