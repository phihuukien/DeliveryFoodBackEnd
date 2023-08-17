using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Food_Delivery_App_BackEnd.ModelDTO
{
    public class RequestOrder
    {

        public int Quantity { get; set; }
        public string DeliveryAddress { get; set; } = String.Empty;
        public string PhoneAddress { get; set; } = String.Empty;
        public string Username { get; set; } = String.Empty;
        public string RestaurantId { get; set; } = String.Empty;
        public string Note { get; set; } = String.Empty;
        public int PaymentMothod { get; set; }
        public float PriceTotal { get; set; }
        public List<RequestOrderDetail>? CartOrder { get; set; }
    }
}
