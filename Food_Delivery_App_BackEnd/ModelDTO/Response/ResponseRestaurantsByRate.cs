using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Food_Delivery_App_BackEnd.ModelDTO.Response
{
    public class ResponseRestaurantsByRate
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Object Image { get; set; }
        public double Rate { get; set; }
        public int TotalOrderRated { get; set; }
        public List<String> Tags { get; set; }
    }
}
