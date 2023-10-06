using Food_Delivery_App_BackEnd.ModelDTO;
using Food_Delivery_App_BackEnd.ModelDTO.Request;
using Food_Delivery_App_BackEnd.Repositories.ImplRepositories;
using Food_Delivery_App_BackEnd.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Food_Delivery_App_BackEnd.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController :  ControllerBase
    {
        private IRepositoryOrders repositoryOrders;
        public OrdersController(IRepositoryOrders repositoryOrders)
        {
            this.repositoryOrders = repositoryOrders;
        }
        [HttpPost]
        [Route("cancel-order")]
        [Authorize]
        public IActionResult CancelOrder([FromBody] RequestCancel requestCancel )
        {
            return repositoryOrders.CancelOrder(requestCancel);
        }
        [HttpDelete]
        [Route("delete-order/{id}")]
        [Authorize]
        public IActionResult DeleteOrder(string id)
        {
            return repositoryOrders.DeleteOrder(id);
        }

        [HttpPost]
        [Route("add")]
        [Authorize]
        public IActionResult AddOrder([FromBody] RequestOrder requestOrder)
        {
            return repositoryOrders.Order(requestOrder);
        }
        [HttpGet]
        [Route("getordercoming")]
        [Authorize]
        public IActionResult GetOrderComing()
        {
            if (HttpContext.Request.Headers.TryGetValue("Authorization", out var headerAuth))
            {
                var jwtToken = headerAuth.First().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1];
                var jwt = new JwtSecurityTokenHandler().ReadJwtToken(jwtToken);
                string username = jwt.Claims.First(c => c.Type == "name").Value;
                return repositoryOrders.GetOrderComing(username);
            }
            return BadRequest(new { status = false, Message = "jwt not found" });
        }
        [HttpGet]
        [Route("getorderwaiting")]
        public IActionResult GetOrderWaiting()
        {
             return repositoryOrders.GetOrderWaiting();
        }
      
        [HttpGet]
        [Route("getorderhistory")]
        [Authorize]
        public IActionResult GetOrderHistory()
        {
            if (HttpContext.Request.Headers.TryGetValue("Authorization", out var headerAuth))
            {
                var jwtToken = headerAuth.First().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1];
                var jwt = new JwtSecurityTokenHandler().ReadJwtToken(jwtToken);
                string username = jwt.Claims.First(c => c.Type == "name").Value;
                return repositoryOrders.GetOrderHistory(username);
            }
            return BadRequest(new { status = false, Message = "jwt not found" });
        }
        [HttpGet]
        [Route("update_order_status/{status}/{orderId}")]
        public IActionResult UpdateOdeStatusByShipper( int status, string orderId)
        {
            return repositoryOrders.UpdateDeliveringStatus(status, orderId);
        }
        [HttpGet]
        [Route("get-order-delivering")]
        public IActionResult GetOrderDelivering()
        {
            return repositoryOrders.GetOrderDelivering();
        }
        [HttpGet]
        [Route("get-order-detail/{orderId}")]
        public IActionResult GetOrderDetail(string orderId)
        {
            return repositoryOrders.GetOrderDetailForShiper(orderId);
        }
    }
}
