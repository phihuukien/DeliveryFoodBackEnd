using Food_Delivery_App_BackEnd.Repositories.ImplRepositories;
using Food_Delivery_App_BackEnd.Repositories.IRepositories;
using Food_Delivery_App_BackEnd.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.IdentityModel.Tokens.Jwt;

namespace Food_Delivery_App_BackEnd.Controllers
{
    [Route("api/partner_order/")]
    [ApiController]
    public class PartnerOrdersController : ControllerBase
    {
      
        private IRepositoryOrders repositoryOrders;
        public PartnerOrdersController(IRepositoryOrders repositoryOrders)
        {
            this.repositoryOrders = repositoryOrders;
        
        }
        [HttpGet]
        [Route("test")]
        public async Task<IActionResult> testSigNal()
        {

            return repositoryOrders.CancelDetail();
        }
        [HttpGet]
        [Route("get-order-pending/{restaurantId}")]
        public IActionResult GetOrderPending(string restaurantId)
        {
            return repositoryOrders.GetOrderPending(restaurantId);
        }
        [HttpGet]
        [Route("get-order-waiting/{restaurantId}")]
        public IActionResult GetOrderWaiting(string restaurantId)
        {
            return repositoryOrders.GetOrderWaitingPartner(restaurantId);
        }

        [HttpGet]
        [Route("getOrder_today/{restaurantId}")]
        public IActionResult GetOrderToday(string restaurantId)
        {
            return repositoryOrders.GetOrderToday(restaurantId);
        }
        [HttpGet]
        [Route("getOrder_all/{restaurantId}")]
        public IActionResult GetOrderAll(string restaurantId,
                                         string? sort_orderBy, 
                                         string? sort_order , 
                                         string? searchString,
                                         string? from_date,
                                         string? to_date,
                                         int? order_status,
                                         int page = 1
                                        )
        {
            return repositoryOrders.GetOrderAll(restaurantId, searchString, sort_order, sort_orderBy, page, from_date, to_date, order_status);
        }
        [HttpGet]
        [Route("update_order_status/{restaurantId}/{status}/{orderId}")]
        public IActionResult UpdateOdeStatus(string restaurantId,int status,string orderId)
        {
            return repositoryOrders.UpdateOrderStatus(restaurantId,status,orderId);
        }

        [HttpGet]
        [Route("get_order_detail/{restaurantId}/{orderId}")]
        public IActionResult GetOrderDetail(string restaurantId, string orderId)
        {
            return repositoryOrders.GetOrderDetail(restaurantId, orderId);
        }
    }
}
