using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Mechanix.API.Core.Data.Models;

namespace Mechanix.API.Core.Data
{
    public class DbConnection : IDbConnection
    {
        private readonly IConfiguration _configuration;
        private readonly IMongoDatabase _database;
        private readonly string _connectionStringID = "MongoDB";

        public string DatabaseName { get; private set; }
        public string CarCollectionName { get; private set; } = "cars";
        public string PartCollectionName { get; private set; } = "parts";

        public MongoClient Client { get; private set; }
        public IMongoCollection<Car> CarCollection { get; private set; }
        public IMongoCollection<Part> PartCollection { get; private set; }

        public DbConnection(IConfiguration configuration)
        {
            _configuration = configuration;
            Client = new MongoClient(_configuration.GetConnectionString(_connectionStringID));
            DatabaseName = _configuration["DatabaseName"];
            _database = Client.GetDatabase(DatabaseName);

            CarCollection = _database.GetCollection<Car>(CarCollectionName);
            PartCollection = _database.GetCollection<Part>(PartCollectionName);
        }
    }
}
