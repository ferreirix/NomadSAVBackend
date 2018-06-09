using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Nomadvantage.sav
{
    [BsonIgnoreExtraElements]
    public class Ticket
    {
        public Ticket()
        {
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("subtitle")]        
        public string Subtitle { get; set; }

        [BsonElement("avatar_url")]                
        public string AvatarUrl { get; set; }

        [BsonElement("age")]                        
        public int Age { get; set; }
    }
}