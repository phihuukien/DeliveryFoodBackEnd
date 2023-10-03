using Food_Delivery_App_BackEnd.Models.DataModels;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Food_Delivery_App_BackEnd.ModelDTO.Response
{
    public class ResponseRateRes
    {
         [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string? Id { get; set; }

        [BsonElement("reviews")]
        public List<Reviews> Reviews { get; set; }

        [BsonElement("restaurant")]
        public List<Restaurants> Restaurant { get; set; }
    }
}
