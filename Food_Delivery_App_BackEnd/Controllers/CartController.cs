using Food_Delivery_App_BackEnd.ModelDTO.Request;
using Food_Delivery_App_BackEnd.Repositories.IRepositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;

namespace Food_Delivery_App_BackEnd.Controllers
{
    [Route("api/carts")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private IRepositoryCarts repositoryCarts;
        public CartController(IRepositoryCarts repositoryCarts)
        {
            this.repositoryCarts = repositoryCarts;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var accessToken = HttpContext.GetTokenAsync("access_token").Result;
            var token = accessToken?.ToString();
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string username = jwt.Claims.First(c => c.Type == "name").Value;
            return repositoryCarts.GetCartItems(username);
        }

        [HttpGet]
        [Route("cartdetail")]
        public IActionResult GetCartDetail()
        {
            var accessToken = HttpContext.GetTokenAsync("access_token").Result;
            var token = accessToken?.ToString();
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string username = jwt.Claims.First(c => c.Type == "name").Value;
            return repositoryCarts.GetCartDetail(username);
        }


        [HttpPost]
        [Route("add")]
        public IActionResult  AddToCart([FromBody] RequestCart requestCart  )
        {
            var accessToken =   HttpContext.GetTokenAsync("access_token").Result;
            var token =  accessToken?.ToString();
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string username = jwt.Claims.First(c => c.Type == "name").Value;
            return repositoryCarts.AddToCart(username, requestCart);
        }

        [HttpPost]
        [Route("remove")]
        public IActionResult RemoveFromCart([FromBody] RequestCart requestCart)
        {
            var accessToken = HttpContext.GetTokenAsync("access_token").Result;
            var token = accessToken?.ToString();
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string username = jwt.Claims.First(c => c.Type == "name").Value;
            return repositoryCarts.RemoveFromCart(username, requestCart); ;
        }

    }
}
