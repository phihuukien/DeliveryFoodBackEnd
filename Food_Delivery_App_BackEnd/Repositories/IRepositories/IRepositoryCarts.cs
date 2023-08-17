using Food_Delivery_App_BackEnd.ModelDTO.Request;
using Microsoft.AspNetCore.Mvc;

namespace Food_Delivery_App_BackEnd.Repositories.IRepositories
{
    public interface IRepositoryCarts
    {
        public IActionResult AddToCart(string username, RequestCart requestCart);
        public IActionResult RemoveFromCart(string username, RequestCart requestCart);
        public JsonResult GetCartItems(string username);
        public JsonResult GetCartDetail(string username);
    }
}
