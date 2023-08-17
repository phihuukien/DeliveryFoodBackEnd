using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Food_Delivery_App_BackEnd.Models.DataModels;

namespace Food_Delivery_App_BackEnd.ModelDTO.Response
{
    public class ResponseCarts
    {


        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string Id { get; set; } = string.Empty;

        [BsonElement("username")]
        public string Username { get; set; }

        [BsonElement("count")]
        public int Count { get; set; }

        [BsonElement("price")]
        public  float Price { get; set; }

        [BsonElement("restaurant")]
        public List<Restaurants>? Restaurant { get; set; }
    }
}
