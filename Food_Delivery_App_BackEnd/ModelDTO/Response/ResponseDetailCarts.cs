using Food_Delivery_App_BackEnd.Models.DataModels;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Food_Delivery_App_BackEnd.ModelDTO.Response
{
    public class ResponseDetailCarts
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string Id { get; set; } = string.Empty;

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("foodId")]
        public string FoodId { get; set; } = string.Empty;

        [BsonElement("count")]
        public int Count { get; set; }

        [BsonElement("food")]
        public List<Foods>? Food { get; set; }
    }
}
