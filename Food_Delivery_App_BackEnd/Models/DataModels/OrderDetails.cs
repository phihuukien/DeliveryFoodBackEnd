using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Food_Delivery_App_BackEnd.Models.DataModels
{
    [BsonIgnoreExtraElements]
    public class OrderDetails
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("orderId")]
        public string OrderId { get; set; } = String.Empty;

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("foodId")]
        public string FoodId { get; set; } = String.Empty;

        [BsonElement("quantity")]
        public int Quantity { get; set; }

    }
}
