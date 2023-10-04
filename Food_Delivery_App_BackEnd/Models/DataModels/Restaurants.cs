using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace Food_Delivery_App_BackEnd.Models.DataModels
{
    [BsonIgnoreExtraElements]
    public class Restaurants
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;

        
        [BsonElement("name")]
        public string Name { get; set; }
       
        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("tags")]
        public List<string> Tags { get; set; }

        [BsonElement("location")]
        public string Location { get; set; }

        [BsonElement("distance")]
        public Int32 Distance { get; set; }

        [BsonElement("time")]
        public Int32 Times { get; set; }

        [BsonElement("images")]
        public Object? Images { get; set; }

        [BsonElement("username")]
        public string? Username { get; set; }

        [BsonElement("categories")]
        public List<string> Categories { get; set; }

    }
}
