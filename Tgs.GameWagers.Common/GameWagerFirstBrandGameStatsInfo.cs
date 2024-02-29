namespace Tgs.GameWagers.Common;

public record GameWagerFirstBrandGameStatsInfo
{
	public Guid GameProviderId { get; set; }
	public string GameProviderCode { get; set; } = default!;
	public int GameId { get; set; }
	public Guid ResellerId { get; set; }
	public string ResellerCode { get; set; } = default!;
	public Guid LicenseeId { get; set; }
	public string LicenseeCode { get; set; } = default!;
	public Guid BrandId { get; set; }
	public string BrandCode { get; set; } = default!;
	public string CurrencyCode { get; set; } = default!;
	public int Count { get; set; }
	public decimal Bet { get; set; }
	public decimal ValidBet { get; set; }
	public decimal Settle { get; set; }
	public decimal Commission { get; set; }
	public decimal Turnover { get; set; }
	public decimal WinLoss { get; set; }
}
