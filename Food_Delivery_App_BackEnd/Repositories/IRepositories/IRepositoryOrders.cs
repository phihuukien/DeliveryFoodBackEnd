
using Food_Delivery_App_BackEnd.ModelDTO;
using Microsoft.AspNetCore.Mvc;

namespace Food_Delivery_App_BackEnd.Repositories.IRepositories
{
    public interface IRepositoryOrders
    {
        public IActionResult Order(RequestOrder requestOrder);
        public IActionResult GetOrderComing(string username);
        public IActionResult GetOrderHistory(string username);

        public IActionResult GetOrderPending(string restaurantId);
        public IActionResult GetOrderToday(string restaurantId);
        public IActionResult GetOrderAll(string restaurantId, 
                                         string? searchString, 
                                         string? sort_order,
                                         string? sort_orderBy, 
                                         int page,
                                         string? from_date,
                                         string? to_date,
                                         int? order_status);

        public IActionResult UpdateOrderStatus(string restaurantId, int status ,string orderId);
        public IActionResult GetOrderDetail(string restaurantId, string orderId);
    }
}
