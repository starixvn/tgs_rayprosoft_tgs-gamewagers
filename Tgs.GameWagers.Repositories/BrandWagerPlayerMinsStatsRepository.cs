using System.Runtime.CompilerServices;
using MongoDB.Bson;
using MongoDB.Driver;
using Tgs.GameWagers.Common;
using Tgs.GameWagers.Core;
using Tgs.GameWagers.Core.Models;
using Tgs.GameWagers.Domains;

namespace Tgs.GameWagers.Repositories;

internal class BrandWagerPlayerMinsStatsRepository(IGameWagersMongoConnectionSettings connectionSettings) : IBrandWagerPlayerMinsStatsRepository
{
    public async IAsyncEnumerable<BrandWagerPlayerStats> QueryAsync(
        string brandCode,
        DateTimeOffset startOn,
        DateTimeOffset endOn,
        BrandWagerPlayerMinsStatsSort sort,
        bool isAscending = true,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)

    {
        var collection = connectionSettings.GetBrandWagerPlayerMinsStatsCollection(brandCode);

        var aggregate = await collection
            .Aggregate()
            .Match(
             Builders<BrandWagerPlayerMinsStatsModel>.Filter.Eq(x => x.BrandCode, brandCode)
             & Builders<BrandWagerPlayerMinsStatsModel>.Filter.Gte(x => x.StatisticsOn, startOn.UtcDateTime)
             & Builders<BrandWagerPlayerMinsStatsModel>.Filter.Lt(x => x.StatisticsOn, endOn.UtcDateTime))
            .Group(
             id: doc => new
             {
                 doc.ResellerId,
                 doc.ResellerCode,
                 doc.LicenseeId,
                 doc.LicenseeCode,
                 doc.BrandId,
                 doc.BrandCode,
                 doc.PlayerCode,
                 doc.Username,
                 doc.CurrencyCode,
             },
             group: group => new BrandWagerPlayerStats(

                 group.Key.ResellerId,
                 group.Key.ResellerCode,
                 group.Key.LicenseeId,
                 group.Key.LicenseeCode,
                 group.Key.BrandId,
                 group.Key.BrandCode,
                 group.Key.PlayerCode,
                 group.Key.Username,
                 group.Key.CurrencyCode,
                 group.Sum(s => s.Count),
                 group.Sum(s => s.Bet),
                 group.Sum(s => s.ValidBet),
                 group.Sum(s => s.Settle),
                 group.Sum(s => s.Commission),
                 group.Sum(s => s.Turnover),
                 group.Sum(s => s.WinLoss)))
            .Sort(new BsonDocument(sort.ToString(), isAscending ? 1 : -1))
            .ToCursorAsync(
            cancellationToken: cancellationToken);

        while (await aggregate
                   .MoveNextAsync(
                   cancellationToken: cancellationToken)
                   .ConfigureAwait(false))
        {
            foreach (var current in aggregate.Current)
            {
                yield return current;
            }
        }
    }

    public async IAsyncEnumerable<BrandWagerPlayerStatsInfo> PlayerCodesQueryAsync(
        string brandCode,
        DateTimeOffset startOn,
        DateTimeOffset endOn,
        IEnumerable<string> playerCodes,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)

    {
        var collection = connectionSettings.GetBrandWagerPlayerMinsStatsCollection(brandCode);

        var aggregate = await collection
            .Aggregate()
            .Match(
             Builders<BrandWagerPlayerMinsStatsModel>.Filter.Eq(x => x.BrandCode, brandCode)
             & Builders<BrandWagerPlayerMinsStatsModel>.Filter.Gte(x => x.StatisticsOn, startOn.UtcDateTime)
             & Builders<BrandWagerPlayerMinsStatsModel>.Filter.Lt(x => x.StatisticsOn, endOn.UtcDateTime)
             & Builders<BrandWagerPlayerMinsStatsModel>.Filter.In(x => x.PlayerCode, playerCodes))
            .Group(
             id: doc => new
             {
                 doc.ResellerId,
                 doc.ResellerCode,
                 doc.LicenseeId,
                 doc.LicenseeCode,
                 doc.BrandId,
                 doc.BrandCode,
                 doc.PlayerCode,
                 doc.Username,
                 doc.CurrencyCode,
             },
             group: group => new BrandWagerPlayerStatsInfo(
                 group.Key.ResellerId,
                 group.Key.ResellerCode,
                 group.Key.LicenseeId,
                 group.Key.LicenseeCode,
                 group.Key.BrandId,
                 group.Key.BrandCode,
                 group.Key.PlayerCode,
                 group.Key.Username,
                 group.Key.CurrencyCode,
                 group.Sum(s => s.Count),
                 group.Sum(s => s.Bet),
                 group.Sum(s => s.ValidBet),
                 group.Sum(s => s.Settle),
                 group.Sum(s => s.Commission),
                 group.Sum(s => s.Turnover),
                 group.Sum(s => s.WinLoss)))
            .ToCursorAsync(
            cancellationToken: cancellationToken);

        while (await aggregate
                   .MoveNextAsync(
                   cancellationToken: cancellationToken)
                   .ConfigureAwait(false))
        {
            foreach (var current in aggregate.Current)
            {
                yield return current;
            }
        }
    }
}
