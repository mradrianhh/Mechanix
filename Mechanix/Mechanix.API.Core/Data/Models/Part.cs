using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Mechanix.API.Core.Data.Models
{
    public class Part
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Producer { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public Car Car { get; set; }
    }
}
