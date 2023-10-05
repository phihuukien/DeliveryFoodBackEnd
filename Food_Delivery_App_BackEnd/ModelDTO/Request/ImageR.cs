using MongoDB.Bson.Serialization.Attributes;

namespace Food_Delivery_App_BackEnd.ModelDTO.Request
{
  
        public class ImageR
        {
        [BsonElement("logo")]
        public string? Logo { get; set; }
        [BsonElement("poster")]
        public string? Poster { get; set; }
        [BsonElement("cover")]
        public string? Cover { get; set; }
        }
    
}
