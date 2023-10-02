using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using Food_Delivery_App_BackEnd.Util;

namespace Food_Delivery_App_BackEnd.Models.DataModels
{
    
    [BsonIgnoreExtraElements]
    public class Orders
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;

        [BsonElement("orderCode")]
        public string OrderCode { get; set; } = String.Empty;

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("restaurantId")]
        public string RestaurantId { get; set; } = String.Empty;

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
        public int Status { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        [BsonElement("dateCreated")]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

   
    }
}
