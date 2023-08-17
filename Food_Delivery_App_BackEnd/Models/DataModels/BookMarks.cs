using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Food_Delivery_App_BackEnd.Models.DataModels
{
    [BsonIgnoreExtraElements]
    public class BookMarks
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("restaurantId")]
        public string RestaurantId { get; set; } = String.Empty;

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("foodId")]
        public string FoodId { get; set; } = string.Empty;


        [BsonElement("username")]
        public string Username { get; set; }

       
    }
}
