using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Food_Delivery_App_BackEnd.Models.DataModels;

namespace Food_Delivery_App_BackEnd.ModelDTO.Response
{
    public class ResponseOrderDetail
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("orderId")]
        public string OrderId { get; set; } = String.Empty;

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("foodId")]
        public string FoodId { get; set; } = String.Empty;

        [BsonElement("quantity")]
        public int Quantity { get; set; }

        [BsonElement("foods")]
        public List<Foods>? Foods { get; set; }
    }
}
