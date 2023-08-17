using Food_Delivery_App_BackEnd.ModelDTO;
using Food_Delivery_App_BackEnd.ModelDTO.Request;
using Food_Delivery_App_BackEnd.ModelDTO.Response;
using Food_Delivery_App_BackEnd.Models.BusinessModels;
using Food_Delivery_App_BackEnd.Models.DataModels;
using Food_Delivery_App_BackEnd.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Food_Delivery_App_BackEnd.Repositories.ImplRepositories
{
    public class RepositoryOrders : IRepositoryOrders
    {
        private FoodDeliveryAppDbContext _context;
        public RepositoryOrders(FoodDeliveryAppDbContext _context)
        {
            this._context = _context;
        }

        public IActionResult GetOrderComing(string username)
        {
            try
            {
                var response = _context.Orders.Aggregate()
                                         .Match(x => x.Username == username && x.Status != 4)
                                         .Lookup("restaurants", "restaurantId", "_id", "restaurant")
                                         .Sort("{dateCreated:-1}")
                                         .ToList();
                var orders = new List<ResponseOrders>();
                foreach (var item in response)
                {
                    orders.Add(BsonSerializer.Deserialize<ResponseOrders>(item));
                }

                if (orders.Count > 0)
                {
                    return new JsonResult(new { Message = "Successfully", Status = true, Data = orders });
                }
                return new JsonResult(new { Message = "Not orders coming", Status = false });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { message = ex.Message, Status = false });
            }
           
        }

        public IActionResult GetOrderHistory(string username)
        {

            var response = _context.Orders.Aggregate()
                                          .Match(x => x.Username == username && x.Status == 4)
                                          .Lookup("restaurants", "restaurantId", "_id", "restaurant")
                                          .Sort("{dateCreated:-1}")
                                          .ToList();
            var ordersHistory = new List<ResponseOrders>();
            foreach (var item in response)
            {
                ordersHistory.Add(BsonSerializer.Deserialize<ResponseOrders>(item));
            }

            if (ordersHistory.Count > 0)
            {
                return new JsonResult(new { Message = "Successfully", Status = true, Data = ordersHistory });
            }
            return new JsonResult(new { Message = "Not orders history", Status = false });
        }

        public IActionResult Order(RequestOrder requestOrder)
        {

            try
            {
                Orders order = new Orders();
                order.DeliveryAddress = requestOrder.DeliveryAddress;
                order.PhoneAddress = requestOrder.PhoneAddress;
                order.RestaurantId = requestOrder.RestaurantId;
                order.Quantity = requestOrder.Quantity;
                order.PriceTotal = requestOrder.PriceTotal;
                order.PaymentMothod = requestOrder.PaymentMothod;
                order.Username = requestOrder.Username;
                order.Note = requestOrder.Note;
                _context.Orders.InsertOneAsync(order);
                List<OrderDetails> orderDetails = new List<OrderDetails>();
                foreach (var item in requestOrder.CartOrder)
                {
                    OrderDetails orderDetail = new OrderDetails();
                    orderDetail.OrderId = order.Id;
                    orderDetail.FoodId = item.FoodId;
                    orderDetail.Quantity = item.Count;
                    orderDetails.Add(orderDetail);
                }
                _context.OrderDetails.InsertManyAsync(orderDetails);
                var filter = Builders<Cart>.Filter.And(Builders<Cart>.Filter.Eq("username", requestOrder.Username),
                                                       Builders<Cart>.Filter.Eq("restaurantId", MongoDB.Bson.ObjectId.Parse(requestOrder.RestaurantId)));
    
                _context.Cart.DeleteManyAsync(filter);
                return new JsonResult(new { Message = "Order Successfully" ,Status=true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { Message = ex.Message , Status = false });
            }
        }
    }
}
