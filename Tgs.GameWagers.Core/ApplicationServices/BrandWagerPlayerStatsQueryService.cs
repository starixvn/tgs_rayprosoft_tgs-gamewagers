using System.Runtime.CompilerServices;
using Tgs.ServiceUtility;

namespace Tgs.GameWagers.Core.ApplicationServices;

internal class BrandWagerPlayerStatsQueryService(IBrandWagerPlayerMinsStatsRepository repository) : IQueryService<BrandWagerPlayerStatsQuery, BrandWagerPlayerStatsReslut>
{
    public async IAsyncEnumerable<BrandWagerPlayerStatsReslut> QueryAsync(BrandWagerPlayerStatsQuery query, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var brandWagerPlayerStats = await repository
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

        if (brandWagerPlayerStats.Length == 0)
        {
            yield break;
        }

        var brandWagerPlayerStatsResluts = brandWagerPlayerStats
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(x => new BrandWagerPlayerStatsReslut(
                ResellerId: x.ResellerId,
                ResellerCode: x.ResellerCode,
                LicenseeId: x.LicenseeId,
                LicenseeCode: x.LicenseeCode,
                BrandId: x.BrandId,
                BrandCode: x.BrandCode,
                PlayerCode: x.PlayerCode,
                Username: x.Username,
                CurrencyCode: x.CurrencyCode,
                Count: x.Count,
                Bet: x.Bet,
                ValidBet: x.ValidBet,
                Settle: x.Settle,
                Commission: x.Commission,
                Turnover: x.Turnover,
                WinLoss: x.WinLoss));

        foreach (var brandWagerPlayerStatsReslut in brandWagerPlayerStatsResluts)
        {
            yield return brandWagerPlayerStatsReslut;
        }
    }
}
