using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Food_Delivery_App_BackEnd.ModelDTO.Request
{
    public class RequestReview
    {
        public string Context { get; set; } = String.Empty;
        public string Username { get; set; } = String.Empty;
        public int Rate { get; set; }

        public List<string>? FoodId { get; set; }

        public List<IFormFile>? ReviewImg { get; set; }
        public string OrderId { get; set; }
    }
}
