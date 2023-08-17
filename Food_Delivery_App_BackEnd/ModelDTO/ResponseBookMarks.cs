using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Food_Delivery_App_BackEnd.Models.DataModels;

namespace Food_Delivery_App_BackEnd.ModelDTO
{
    public class ResponseBookMarks
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("restaurantId")]
        public string RestaurantId { get; set; } = String.Empty;


        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("foodId")]
        public string FoodId { get; set; } = String.Empty;

        [BsonElement("username")]
        public string Username { get; set; }

        [BsonElement("restaurant")]
        public List<Restaurants> Restaurant { get; set; }
    }
}
