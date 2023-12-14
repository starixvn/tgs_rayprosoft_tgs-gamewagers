using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Tgs.GameWagers;

internal class MongoDBConnectionManager
{
    public IMongoClient MongoClient { get; }

    public MongoDBConnectionManager(
        IOptions<MongoDBOptions> optionsAccessor)
    {
        ArgumentNullException.ThrowIfNull(optionsAccessor);

        var options = optionsAccessor.Value;

        var mongoUrl = new MongoUrl(options.ConnectionString);
        MongoClient = new MongoClient(mongoUrl);
    }
}
