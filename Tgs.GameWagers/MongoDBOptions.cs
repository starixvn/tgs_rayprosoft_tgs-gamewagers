namespace Tgs.GameWagers;

public record MongoDBOptions
{
    public string ConnectionString { get; set; } = "mongodb://localhost";
}
