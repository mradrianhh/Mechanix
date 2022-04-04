using Mechanix.API.Core.Data.Models;
using MongoDB.Driver;

namespace Mechanix.API.Core.Data
{
    public interface IDbConnection
    {
        IMongoCollection<Car> CarCollection { get; }
        string CarCollectionName { get; }
        MongoClient Client { get; }
        string DatabaseName { get; }
        IMongoCollection<Part> PartCollection { get; }
        string PartCollectionName { get; }
    }
}