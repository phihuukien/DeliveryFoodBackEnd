using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Food_Delivery_App_BackEnd.Models.DataModels
{
    [BsonIgnoreExtraElements]
    public class Users
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
        public string Phone { get; set; } = String.Empty;

        [BsonElement("status")]
        public int Status { get; set; } = 1;

        [BsonElement("role")]
        public string Role { get; set; } = String.Empty;

        [BsonElement("avatar")]
        public string Avatar { get; set; } = String.Empty;

        [BsonElement("creationtime")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Creationtime { get; set; } = DateTime.Now;
    }
}
