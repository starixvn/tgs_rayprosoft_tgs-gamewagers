using System.Runtime.CompilerServices;
using MongoDB.Bson;
using MongoDB.Driver;
using Tgs.GameWagers.Common;
using Tgs.GameWagers.Core;
using Tgs.GameWagers.Core.Models;
using Tgs.GameWagers.Domains;

namespace Tgs.GameWagers.Repositories;

internal class BrandWagerGameMinsStatsRepository(IGameWagersMongoConnectionSettings connectionSettings) : IBrandWagerGameMinsStatsRepository
{
    public async IAsyncEnumerable<BrandWagerGameMinsStats> QueryAsync(
        string brandCode,
        DateTimeOffset startOn,
        DateTimeOffset endOn,
        BrandWagerGameMinsStatsSort sort,
        bool isAscending = true,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var collection = connectionSettings.GetBrandWagerGameMinsStatsCollection(brandCode);

        var aggregate = await collection
            .Aggregate()
            .Match(
             Builders<BrandWagerGameMinsStatsModel>.Filter.Eq(x => x.BrandCode, brandCode)
             & Builders<BrandWagerGameMinsStatsModel>.Filter.Gte(x => x.StatisticsOn, startOn.UtcDateTime)
             & Builders<BrandWagerGameMinsStatsModel>.Filter.Lt(x => x.StatisticsOn, endOn.UtcDateTime))
            .Group(
             id: doc => new
             {
                 doc.GameProviderId,
                 doc.GameProviderCode,
                 doc.GameId,
                 doc.ResellerId,
                 doc.ResellerCode,
                 doc.LicenseeId,
                 doc.LicenseeCode,
                 doc.BrandId,
                 doc.BrandCode,
                 doc.CurrencyCode,
             },
             group: group => new BrandWagerGameMinsStats(
                 group.Key.GameProviderId,
                 group.Key.GameProviderCode,
                 group.Key.GameId,
                 group.Key.ResellerId,
                 group.Key.ResellerCode,
                 group.Key.LicenseeId,
                 group.Key.LicenseeCode,
                 group.Key.BrandId,
                 group.Key.BrandCode,
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
            cancellationToken: cancellationToken)
			.ConfigureAwait(false);

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
