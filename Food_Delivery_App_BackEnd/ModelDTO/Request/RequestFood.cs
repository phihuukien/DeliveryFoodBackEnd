using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Food_Delivery_App_BackEnd.Models.DataModels
{

    public class RequestFood
    {
        public string Id { get; set; } = String.Empty;
        public string RestaurantId { get; set; } = String.Empty;
       
        public string Name { get; set; } = String.Empty;

        public Int32 Price { get; set; }

        public IFormFile? Image { get; set; }

        public string Category { get; set; } = String.Empty;

        public string Description { get; set; } = String.Empty;

        public int Status { get; set; } = 1;

        public string Ingredients { get; set; } = String.Empty;
    }
}
