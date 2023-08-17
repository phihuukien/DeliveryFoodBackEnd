using MongoDB.Bson.Serialization.Attributes;

namespace Food_Delivery_App_BackEnd.ModelDTO
{
    public class RequestOrderDetail
    {
        public string FoodId { get; set; } = String.Empty;
        public int Count { get; set; }
    }
}
