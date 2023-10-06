using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using Food_Delivery_App_BackEnd.ModelDTO.Request;

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
        public string? Distance { get; set; }

        [BsonElement("time")]
        public string? Times { get; set; }

        [BsonElement("images")]
        public ImageR Images { get; set; }

        [BsonElement("username")]
        public string? Username { get; set; }

        [BsonElement("categories")]
        public List<string> Categories { get; set; }

    }

}
