using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Food_Delivery_App_BackEnd.ModelDTO.Request
{
    public class RequestCategory
    {
        public string RestaurantId { get; set; } = String.Empty;
        public string Name { get; set; } = String.Empty;
        public int Status { get; set; }
    }
}
