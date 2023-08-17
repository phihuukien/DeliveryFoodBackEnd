using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Food_Delivery_App_BackEnd.Models.DataModels
{
    [BsonIgnoreExtraElements]
    public class Cart
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("foodId")]
        public string FoodId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("restaurantId")]
        public string? RestaurantId { get; set; }

        [BsonElement("price")]
        public float? Price { get; set; }


        [BsonElement("username")]
        public string Username { get; set; }

        [BsonElement("count")]
        public Int32 Count { get; set; }


    }
}
