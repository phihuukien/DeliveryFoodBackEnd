using MongoDB.Bson.Serialization.Attributes;

namespace Food_Delivery_App_BackEnd.ModelDTO.Response
{
    public class ResponseId
    {
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        [BsonElement("_id")]
        public string Id { get; set; } = String.Empty;
    }
}
