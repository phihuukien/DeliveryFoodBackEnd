using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Food_Delivery_App_BackEnd.Models.DataModels
{
    [BsonIgnoreExtraElements]
    public class Reviews
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;

        [BsonElement("context")]
        public string Context { get; set; }

        [BsonElement("username")]
        public string Username { get; set; }

        [BsonElement("rate")]
        public int Rate { get; set; }

        [BsonElement("created_at")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Date { get; set; } = DateTime.Now;

        [BsonElement("foodId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string FoodId { get; set; }
        
        [BsonElement("orderId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string OrderId { get; set; }

        [BsonElement("reviewImg")]
        public List<string> ReviewImg { get; set; }
    }
}
