using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Food_Delivery_App_BackEnd.Models.DataModels
{

    public class RequestFood
    {
        public string? Id { get; set; }
        public string? RestaurantId { get; set; }
       
        public string? Name { get; set; }

        public Int32 Price { get; set; }

        public IFormFile? Image { get; set; }

        public string? Category { get; set; }

        public string? Description { get; set; }

        public int Status { get; set; } = 1;

        public string? Ingredients { get; set; }
    }
}
