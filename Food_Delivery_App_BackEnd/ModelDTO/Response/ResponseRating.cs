using Food_Delivery_App_BackEnd.Models.DataModels;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Food_Delivery_App_BackEnd.ModelDTO.Response
{
    public class ResponseRating
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string? Id { get; set; }

        [BsonElement("reviews")]
        public List<Reviews> Reviews { get; set; }

        [BsonElement("foods")]
        public List<Foods> Foods { get; set; }
    }
}
