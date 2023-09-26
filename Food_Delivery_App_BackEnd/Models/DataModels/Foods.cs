using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Food_Delivery_App_BackEnd.Models.DataModels
{

    [BsonIgnoreExtraElements]
    public class Foods
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;

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


        [BsonElement("status")]
        public int Status { get; set; } = 1;


        [BsonElement("ingredients")]
        public string Ingredients { get; set; } = String.Empty;
    }
}
