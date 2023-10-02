using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Food_Delivery_App_BackEnd.Models.DataModels
{
    public class Tags
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("status")]
        public int Status { get; set; }
        [BsonElement("image")]
        public string Image { get; set; }
    }
}
