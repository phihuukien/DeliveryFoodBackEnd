using Food_Delivery_App_BackEnd.ModelDTO;
using Food_Delivery_App_BackEnd.Repositories.ImplRepositories;
using Food_Delivery_App_BackEnd.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Food_Delivery_App_BackEnd.Controllers
{
    [Route("api/orders")]
    [ApiController]
    [Authorize]
    public class OrdersController :  ControllerBase
    {
        private IRepositoryOrders repositoryOrders;
        public OrdersController(IRepositoryOrders repositoryOrders)
        {
            this.repositoryOrders = repositoryOrders;
        }

        [HttpPost]
        [Route("add")]
        public IActionResult AddOrder([FromBody] RequestOrder requestOrder)
        {
            return repositoryOrders.Order(requestOrder);
        }
        [HttpGet]
        [Route("getordercoming")]
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
        [Route("getorderhistory")]
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
    }
}
