using Food_Delivery_App_BackEnd.Models.DataModels;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Food_Delivery_App_BackEnd.ModelDTO
{
    public class ResponseRestaurant
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
        public string Distance { get; set; }

        [BsonElement("time")]
        public string Times { get; set; }

        [BsonElement("images")]
        public Object Images { get; set; }


        [BsonElement("categories")]
        public List<string> Categories { get; set; }

        [BsonElement("foods")]
        public List<Foods> Foods { get; set; }
    }
}
