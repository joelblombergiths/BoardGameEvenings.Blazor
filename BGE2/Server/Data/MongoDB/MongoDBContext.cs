using MongoDB.Driver;

namespace BGE2.Server.Data.MongoDB;

public class MongoDBContext<EventModel>
{
    public readonly IMongoCollection<EventModel> Collection;
    public MongoDBContext(string connectionString, string collectionName, string databaseName)
    {
        MongoClientSettings settings = MongoClientSettings.FromConnectionString(connectionString);
        settings.ServerApi = new(ServerApiVersion.V1);
        MongoClient client = new(settings);
        IMongoDatabase db = client.GetDatabase(databaseName);
        Collection = db.GetCollection<EventModel>(collectionName, new() { AssignIdOnInsert = true });
    }
}
