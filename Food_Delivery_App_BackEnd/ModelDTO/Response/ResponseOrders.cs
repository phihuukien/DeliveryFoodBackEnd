using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Food_Delivery_App_BackEnd.Models.DataModels;
using System.ComponentModel.DataAnnotations;

namespace Food_Delivery_App_BackEnd.ModelDTO.Response
{
    public class ResponseOrders
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string Id { get; set; } = String.Empty;

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("restaurantId")]
        public string RestaurantId { get; set; } = String.Empty;

        [BsonElement("orderCode")]
        public string OrderCode { get; set; } = String.Empty;

        [BsonElement("quantity")]
        public int Quantity { get; set; }

        [BsonElement("deliveryAddress")]
        public string DeliveryAddress { get; set; } = String.Empty;

        [BsonElement("phoneAddress")]
        public string PhoneAddress { get; set; } = String.Empty;


        [BsonElement("username")]
        public string Username { get; set; } = String.Empty;

        [BsonElement("note")]
        public string Note { get; set; } = String.Empty;

        [BsonElement("paymentMothod")]
        public int PaymentMothod { get; set; }

        [BsonElement("priceTotal")]
        public float PriceTotal { get; set; }

        [BsonElement("deliveringStatus")]
        public int DeliveringStatus { get; set; }

        [BsonElement("status")]
        public int Status { get; set; } = 1;

        [BsonElement("dateCreated")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        [BsonElement("restaurant")]
        public List<Restaurants>? Restaurant { get; set; }

        [BsonElement("user")]
        public List<Users>? User { get; set; }


        [BsonElement("reviewStatus")]
        public int ReviewStatus { get; set; } = 1;

        }
}
