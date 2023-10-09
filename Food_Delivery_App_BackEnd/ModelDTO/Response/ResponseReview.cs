using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Food_Delivery_App_BackEnd.Models.DataModels;

namespace Food_Delivery_App_BackEnd.ModelDTO.Response
{
    public class ResponseReview
    {


        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string Id { get; set; } = string.Empty;

        [BsonElement("context")]
        public string Context { get; set; }

        [BsonElement("username")]
        public string Username { get; set; }

        [BsonElement("rate")]
        public int Rate { get; set; }

        [BsonElement("created_at")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Date { get; set; } = DateTime.Now;

        [BsonElement("foods")]
        public List<Foods> Food { get; set; } 
        
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("foodId")]

        public string FoodId { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("orderId")]

        public string OrderId { get; set; }
        [BsonElement("reviewImg")]

        public List<string> ReviewImg { get; set; }
    }
}
