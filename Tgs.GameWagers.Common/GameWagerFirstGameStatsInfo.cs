namespace Tgs.GameWagers.Common;

public record GameWagerFirstGameStatsInfo
{
	public Guid GameProviderId { get; set; }
	public string GameProviderCode { get; set; } = default!;
	public int GameId { get; set; }
	public string CurrencyCode { get; set; } = default!;
	public int Count { get; set; }
	public decimal Bet { get; set; }
	public decimal ValidBet { get; set; }
	public decimal Settle { get; set; }
	public decimal Commission { get; set; }
	public decimal Turnover { get; set; }
	public decimal WinLoss { get; set; }
}
