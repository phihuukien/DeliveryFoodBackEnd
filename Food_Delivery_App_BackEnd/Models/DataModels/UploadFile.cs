using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace Food_Delivery_App_BackEnd.Models.DataModels
{
    public class UploadFile
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;

        [Required]
        [BsonElement("username")]
        public string Username { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }

        [BsonElement("phone")]
        public string Phone { get; set; }

        [BsonElement("status")]
        public int Status { get; set; } = 1;



        [BsonElement("creationtime")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Creationtime { get; set; } = DateTime.Now;

        public IFormFile? Image { get; set; }
    }
}
