using Food_Delivery_App_BackEnd.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Food_Delivery_App_BackEnd.Controllers
{
    [Route("api/admin/restaurants")]
    [ApiController]
    [Authorize]
    public class AdminRestaurantController : ControllerBase
    {
        private IRepositoryRestaurants repositoryRestaurants;
        public AdminRestaurantController(IRepositoryRestaurants repositoryRestaurants)
        {
            this.repositoryRestaurants = repositoryRestaurants;
        }
        [HttpGet]
        [Route("")]
        [Authorize(Roles ="ADMIN")]
        public IActionResult Index(int page = 1)
        {
            return repositoryRestaurants.GetAllRestaurantsAdmin(page);
        }

        [HttpGet]
        [Route("{username}")]
        public IActionResult GetRestaurantsByUsername(string username)
        {
            return repositoryRestaurants.GetRestaurantsByUsernamePartner(username);
        }
    }
}
