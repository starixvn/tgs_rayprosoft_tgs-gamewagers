using System.Runtime.CompilerServices;
using Tgs.ServiceUtility;

namespace Tgs.GameWagers.Core.ApplicationServices;

internal class BrandWagerGameStatsQueryService(IBrandWagerGameMinsStatsRepository repository) : IQueryService<BrandWagerGameStatsQuery, BrandWagerGameStatsReslut>
{
    public async IAsyncEnumerable<BrandWagerGameStatsReslut> QueryAsync(BrandWagerGameStatsQuery query, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var brandWagerGameMinsStats = await repository
            .QueryAsync(
            query.BrandCode,
            query.StartOn,
            query.EndOn,
            query.Sort,
            query.IsAscending,
            cancellationToken)
            .ToArrayAsync(
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (brandWagerGameMinsStats.Length == 0)
        {
            yield break;
        }

        var brandWagerGameStatsResluts = brandWagerGameMinsStats
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(x => new BrandWagerGameStatsReslut(
                GameProviderId: x.GameProviderId,
                GameProviderCode: x.GameProviderCode,
                GameId: x.GameId,
                ResellerId: x.ResellerId,
                ResellerCode: x.ResellerCode,
                LicenseeId: x.LicenseeId,
                LicenseeCode: x.LicenseeCode,
                BrandId: x.BrandId,
                BrandCode: x.BrandCode,
                CurrencyCode: x.CurrencyCode,
                Count: x.Count,
                Bet: x.Bet,
                ValidBet: x.ValidBet,
                Settle: x.Settle,
                Commission: x.Commission,
                Turnover: x.Turnover,
                WinLoss: x.WinLoss));

        foreach (var brandWagerGameStatsReslut in brandWagerGameStatsResluts)
        {
            yield return brandWagerGameStatsReslut;
        }
    }
}
