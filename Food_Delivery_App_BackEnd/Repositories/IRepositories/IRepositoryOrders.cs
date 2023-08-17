
using Food_Delivery_App_BackEnd.ModelDTO;
using Microsoft.AspNetCore.Mvc;

namespace Food_Delivery_App_BackEnd.Repositories.IRepositories
{
    public interface IRepositoryOrders
    {
        public IActionResult Order(RequestOrder requestOrder);
        public IActionResult GetOrderComing(string username);
        public IActionResult GetOrderHistory(string username);
    }
}
