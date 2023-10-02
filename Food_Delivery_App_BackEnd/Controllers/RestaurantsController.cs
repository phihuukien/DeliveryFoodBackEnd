using Food_Delivery_App_BackEnd.Repositories.IRepositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;

namespace Food_Delivery_App_BackEnd.Controllers
{
    [Route("api/restaurants")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private IRepositoryRestaurants repositoryRestaurants;
       
        public RestaurantsController(IRepositoryRestaurants repositoryRestaurants)
        {
           
            this.repositoryRestaurants = repositoryRestaurants;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return repositoryRestaurants.GetAllRestaurants();
        }

        [HttpGet]
        [Route("{restaurantId}")]
        public IActionResult GetOneRestaurantById(string restaurantId)
        {
            return repositoryRestaurants.GetOneRestaurantById(restaurantId);
        }

        [HttpGet]
        [Route("tag/{tagName}")]
        public IActionResult GetRestaurantsByTag(string tagName)
        {
            return repositoryRestaurants.GetRestaurantsByTag(tagName);
        }
    }
}
