using Food_Delivery_App_BackEnd.Models.DataModels;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Food_Delivery_App_BackEnd.ModelDTO
{
    public class ResponseFoods
    {

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string Id { get; set; } = string.Empty;

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("restaurantId")]
        public string RestaurantId { get; set; } = String.Empty;

        [BsonElement("name")]
        public string Name { get; set; } = String.Empty;

        [BsonElement("price")]
        public Int32 Price { get; set; }

        [BsonElement("image")]
        public string Image { get; set; } = String.Empty;

        [BsonElement("category")]
        public string Category { get; set; } = String.Empty;

        [BsonElement("description")]
        public string Description { get; set; } = String.Empty;

        [BsonElement("ingredients")]
        public string Ingredients { get; set; } = String.Empty;

        [BsonElement("restaurant")]
        public List<Restaurants>? Restaurant { get; set; }
    }
}