using Food_Delivery_App_BackEnd.Models.DataModels;
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
        [Route("restaurant/{username}")]
        public IActionResult GetRestaurantsByUsernamePartner(string username)
        {
            return repositoryRestaurants.GetRestaurantsByUsernamePartner(username);
        }
        [HttpPost]
        [Route("add-restaurant")]
        public IActionResult AddRestaurant([FromBody] Restaurants restaurant)
        {
            return repositoryRestaurants.AddRestaurant(restaurant);
        }
    }
}
