using Food_Delivery_App_BackEnd.ModelDTO;
using Food_Delivery_App_BackEnd.ModelDTO.Request;
using Food_Delivery_App_BackEnd.ModelDTO.Response;
using Food_Delivery_App_BackEnd.Models.BusinessModels;
using Food_Delivery_App_BackEnd.Models.DataModels;
using Food_Delivery_App_BackEnd.Repositories.IRepositories;
using Food_Delivery_App_BackEnd.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using static MongoDB.Driver.WriteConcern;
using static NuGet.Packaging.PackagingConstants;

namespace Food_Delivery_App_BackEnd.Repositories.ImplRepositories
{
    public class RepositoryOrders : IRepositoryOrders
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private FoodDeliveryAppDbContext _context;
        public RepositoryOrders(FoodDeliveryAppDbContext _context, IHubContext<ChatHub> hubContext)
        {
            this._context = _context;
            _hubContext = hubContext;
        }

        public IActionResult CancelDetail()
        {
             _hubContext.Clients.All.SendAsync("ReceiveMessageFromMobile", "kien");
            return new JsonResult(new { Message = "ff", Status = false }); ;
        }

        public IActionResult GetOrderAll(string restaurantId,
                                         string? searchString,
                                         string? sort_order,
                                         string? sort_orderBy,
                                         int page,
                                         string? from_date,
                                         string? to_date,
                                         int? order_status)
        {
            try
            {
                var filter = Builders<Orders>.Filter.Where(x => x.RestaurantId == restaurantId);

                if (!string.IsNullOrEmpty(searchString))
                {
                    var regexFilter = Builders<Orders>.Filter.Regex("orderCode", new BsonRegularExpression(new Regex(searchString, RegexOptions.IgnoreCase)));
                    filter = Builders<Orders>.Filter.And(filter, regexFilter);
                }

                if (order_status > 0)
                {
                    var statusFilter = Builders<Orders>.Filter.Where(x => x.Status == order_status);
                    filter = Builders<Orders>.Filter.And(filter, statusFilter);
                }

                if (!string.IsNullOrEmpty(from_date) && !string.IsNullOrEmpty(to_date))
                {
                    var dateFilter = Builders<Orders>.Filter.And(
                        Builders<Orders>.Filter.Gte(x => x.DateCreated, DateTime.Parse(from_date)),
                        Builders<Orders>.Filter.Lte(x => x.DateCreated, DateTime.Parse(to_date).AddDays(1))
                    );
                    filter = Builders<Orders>.Filter.And(filter, dateFilter);
                }

                var find = _context.Orders.Find(filter);

                if (sort_orderBy == "date" && sort_order == "ASC")
                {
                    find = find.SortBy(p => p.DateCreated);
                }
                else if (sort_orderBy == "date" && sort_order == "DESC")
                {
                    find = find.SortByDescending(p => p.DateCreated);
                }

                if (sort_orderBy == "total" && sort_order == "ASC")
                {
                    find = find.SortBy(p => p.PriceTotal);
                }
                else if (sort_orderBy == "total" && sort_order == "DESC")
                {
                    find = find.SortByDescending(p => p.PriceTotal);
                }

                int page_size = 2;
                int skip = page_size * (page - 1);
                var total_document = find.CountDocuments();
                var total_Page = total_document % page_size == 0 ? total_document / page_size : total_document / page_size + 1;
                var orders = find.Skip(skip).Limit(page_size).ToList();
                return new JsonResult(new { Message = "Successfully", Sort_orderBy = sort_orderBy, Sort_order = sort_order, Page = page, Total_Page = total_Page, Status = true, Data = orders });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { Message = ex.Message, Status = false });
            }


        }

        public IActionResult GetOrderComing(string username)
        {
            try
            {
                var response = _context.Orders.Aggregate()
                                         .Match(x => x.Username == username && x.DeliveringStatus != 5)
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

        public IActionResult GetOrderDelivering()
        {
            try
            {
                var response = _context.Orders.Aggregate()
                                         .Match(x => x.DeliveringStatus == 2 || x.DeliveringStatus == 3 || x.DeliveringStatus == 4)
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
                return new JsonResult(new { Message = "Not orders waiting", Status = false });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { message = ex.Message, Status = false });
            }
        }

        public IActionResult GetOrderDetail(string restaurantId, string orderId)
        {
            try
            {
                var responseOrder = _context.Orders.Aggregate()
                                        .Match(x => x.RestaurantId == restaurantId && x.Id == orderId)
                                        .Lookup("restaurants", "restaurantId", "_id", "restaurant")
                                          .Lookup("users", "username", "username", "user")
                                        .FirstOrDefault();
                var responseOrderDetail = _context.OrderDetails.Aggregate()
                                       .Match(x => x.OrderId == orderId)
                                        .Lookup("foods", "foodId", "_id", "foods")
                                       .ToList();
                var ordersDetail = new List<ResponseOrderDetail>();
                var order = BsonSerializer.Deserialize<ResponseOrders>(responseOrder);
                foreach (var item in responseOrderDetail)
                {
                    ordersDetail.Add(BsonSerializer.Deserialize<ResponseOrderDetail>(item));
                }
                if (order != null && ordersDetail.Count > 0)
                {
                    return new JsonResult(new { Message = "Successfully", Status = true, Data = order, DataOrderDetail = ordersDetail });
                }
                return new JsonResult(new { Message = "not found order", Status = false });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { Message = ex.Message, Status = false });
            }
        }

        public IActionResult GetOrderDetailForShiper(string orderId)
        {
            try
            {
                var responseOrder = _context.Orders.Aggregate()
                                       .Match(x =>  x.Id == orderId)
                                       .Lookup("restaurants", "restaurantId", "_id", "restaurant")
                                         .Lookup("users", "username", "username", "user")
                                       .FirstOrDefault();
                var order = BsonSerializer.Deserialize<ResponseOrders>(responseOrder);
                var responseOrderDetail = _context.OrderDetails.Aggregate()
                                       .Match(x => x.OrderId == orderId)
                                        .Lookup("foods", "foodId", "_id", "foods")
                                       .ToList();
                var ordersDetail = new List<ResponseOrderDetail>();
                foreach (var item in responseOrderDetail)
                {
                    ordersDetail.Add(BsonSerializer.Deserialize<ResponseOrderDetail>(item));
                }
                if (order != null && ordersDetail.Count > 0)
                {
                    return new JsonResult(new { Message = "Successfully", Status = true, Data = order, DataOrderDetail = ordersDetail });
                }
                return new JsonResult(new { Message = "not found order", Status = false });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { Message = ex.Message, Status = false });
            }
        }

        public IActionResult GetOrderHistory(string username)
        {

            var response = _context.Orders.Aggregate()
                                          .Match(x => x.Username == username && x.DeliveringStatus == 5)
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

        public IActionResult GetOrderPending(string restaurantId)
        {
            var orderPending = _context.Orders.Find(x => x.Status == 1 && x.RestaurantId == restaurantId).ToList();

            return new JsonResult(new { Message = "Successfly", Status = true, Data = orderPending });
        }

        public IActionResult GetOrderToday(string restaurantId)
        {
            var foodSoldOut = _context.Orders.Find(x => x.Status == (int)StatusFood.SOLDOUT).ToList().Count();
            var orderToday = _context.Orders.Find(x => x.RestaurantId == restaurantId && x.DateCreated.Day == DateTime.Now.Day).ToList();
            var orderPending = orderToday.Where(x => x.Status == (int)Status.PENDING).ToList();
            var orderWaiting = orderToday.Where(x => x.Status == (int)Status.WAITING).ToList();
            var orderCancel = orderToday.Where(x => x.Status == (int)Status.Cancel).ToList();
            var earnings = orderToday.Sum(x => x.PriceTotal);
            var orderFinish = orderToday.Where(x => x.Status == (int)Status.Finish).Count();

            var responseDashboard = new ResponseDashboard();
            responseDashboard.Earnings = (int)earnings;
            responseDashboard.TotalOrderToday = orderToday.Count();
            responseDashboard.OrderPending = orderPending;
            responseDashboard.FoodSoldOut = foodSoldOut;
            responseDashboard.OrderCancel = orderCancel.Count();
            responseDashboard.OrderFinish = orderFinish;
            responseDashboard.Message = "Successfly";
            responseDashboard.Status = true;

            return new JsonResult(responseDashboard);
        }

        public IActionResult GetOrderWaiting()
        {
            try
            {
                var response = _context.Orders.Aggregate()
                                         .Match(x => x.Status == 2)
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
                return new JsonResult(new { Message = "Not orders waiting", Status = false });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { message = ex.Message, Status = false });
            }
        }

        public IActionResult GetOrderWaitingPartner(string restaurantId)
        {
            var orderWaiting = _context.Orders.Find(x => (x.Status == 2 || x.Status == 3) && x.RestaurantId == restaurantId).ToList();

            return new JsonResult(new { Message = "Successfly", Status = true, Data = orderWaiting });
        }

        public IActionResult Order(RequestOrder requestOrder)
        {

            try
            {
                DateTime thisDay = DateTime.Today;
                var date = DateTime.Now.ToString("MMddyyyyHHmmss");
                string strrandom = Guid.NewGuid().ToString("N").Substring(0,5);
                Orders order = new Orders();
                order.DeliveryAddress = requestOrder.DeliveryAddress;
                order.PhoneAddress = requestOrder.PhoneAddress;
                order.RestaurantId = requestOrder.RestaurantId;
                order.Quantity = requestOrder.Quantity;
                order.PriceTotal = requestOrder.PriceTotal;
                order.PaymentMothod = requestOrder.PaymentMothod;
                order.Username = requestOrder.Username;
                order.Note = requestOrder.Note;
                order.OrderCode = date + "-"+ strrandom;
                order.Status = (int)Status.PENDING;
                order.DeliveringStatus = (int)Status.PENDING;
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
                _hubContext.Clients.All.SendAsync("ReceiveMessageFromMobile", "web nhan");
                return new JsonResult(new { Message = "Order Successfully", Status = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { Message = ex.Message, Status = false });
            }
        }

        public IActionResult UpdateDeliveringStatus(int status, string orderId)
        {
            try
            {
                var order = _context.Orders.Find(x => x.Id == orderId).FirstOrDefault();
                if (order != null)
                {
                    if (status == 2)
                    {
                        var filter = Builders<Orders>.Filter
                       .Eq(order => order.Id, orderId);
                        var update = Builders<Orders>.Update
                            .Set(Orders => Orders.DeliveringStatus, status)
                            .Set(Order => Order.Status, 3);
                        _context.Orders.UpdateOne(filter, update);
                        _hubContext.Clients.All.SendAsync("ReceiveMessageFromMobile", "web nhan");
                        _hubContext.Clients.All.SendAsync("ReceiveMessageFromWeb", "mobile nhan");
                        _hubContext.Clients.All.SendAsync("SendStutusToMobileForShipper", "mobile nhan");
                        return new JsonResult(new { Message = "Successfly", Status = true });
                    }
                    else
                    {
                        var filter = Builders<Orders>.Filter
                        .Eq(order => order.Id, orderId);
                        var update = Builders<Orders>.Update
                            .Set(Orders => Orders.DeliveringStatus, status);
                        _context.Orders.UpdateOne(filter, update);
                        _hubContext.Clients.All.SendAsync("SendStutusToMobileForShipper", "mobile nhan");
                        return new JsonResult(new { Message = "Successfly", Status = true });
                    }

                    
                }
                return new JsonResult(new { Message = "ex.Message", Status = false });

            }
            catch (Exception ex)
            {
                return new JsonResult(new { Message = ex.Message, Status = false });
            }
        }

        public IActionResult UpdateOrderStatus(string restaurantId, int status, string orderId)
        {
            try
            {
                var order = _context.Orders.Find(x => x.RestaurantId == restaurantId && x.Id == orderId).FirstOrDefault();
                if (order != null)
                {

                    var filter = Builders<Orders>.Filter
                        .Eq(order => order.Id, orderId);
                    var update = Builders<Orders>.Update
                        .Set(Orders => Orders.Status, status);

                    _context.Orders.UpdateOne(filter, update);
                    _hubContext.Clients.All.SendAsync("ReceiveMessageFromWeb", "order");
                    return new JsonResult(new { Message = "Successfly", Status = true });
                }
                return new JsonResult(new { Message = "failed", Status = false });

            }
            catch (Exception ex)
            {
                return new JsonResult(new { Message = ex.Message, Status = false });
            } 
        }
    }
}
