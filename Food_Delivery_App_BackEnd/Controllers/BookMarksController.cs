using Food_Delivery_App_BackEnd.Models.DataModels;
using Food_Delivery_App_BackEnd.Repositories.IRepositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Food_Delivery_App_BackEnd.Controllers
{
    [Route("api/bookmarks")]
    [ApiController]
    [Authorize]
    public class BookMarksController : ControllerBase
    {
        private IRepositoryBookMarks repositoryBookMarks;
    
       
        public BookMarksController(IRepositoryBookMarks repositoryBookMarks)
        {
            this.repositoryBookMarks = repositoryBookMarks;
       
        }
        [HttpGet]
        public IActionResult Index()
        {
            var accessToken = HttpContext.GetTokenAsync("access_token").Result;
            var token = accessToken?.ToString();
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string username = jwt.Claims.First(c => c.Type == "name").Value;
            return repositoryBookMarks.GetBookmarks(username);
        }
        [HttpPost]
        [Route("addBookMart/{restaurantId}")]
        public IActionResult AddBookmark(string restaurantId)
        {
          
            var accessToken = HttpContext.GetTokenAsync("access_token").Result;
            var token = accessToken?.ToString();
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string username = jwt.Claims.First(c => c.Type == "name").Value;
            return repositoryBookMarks.AddBookmark(username, restaurantId);
        }
        [HttpDelete]
        [Route("removeBookMart/{restaurantId}")]
        public IActionResult RemoveBookmark(string restaurantId)
        {
            var accessToken = HttpContext.GetTokenAsync("access_token").Result;
            var token = accessToken?.ToString();
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string username = jwt.Claims.First(c => c.Type == "name").Value;
            return repositoryBookMarks.RemoveBookmark(username, restaurantId);
        }
    }
}
