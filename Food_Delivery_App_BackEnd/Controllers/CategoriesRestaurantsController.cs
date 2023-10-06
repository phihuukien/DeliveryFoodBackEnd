using Food_Delivery_App_BackEnd.Models.DataModels;
using Food_Delivery_App_BackEnd.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace Food_Delivery_App_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesRestaurantsController : ControllerBase
    {
        private IRepositoryCategoriesRestaurants Repository;

        public CategoriesRestaurantsController(IRepositoryCategoriesRestaurants RepositoryCategoriesRestaurants)
        {

            this.Repository = RepositoryCategoriesRestaurants;
        }

        [HttpGet]
        [Route("getAllCatRes/{restaurantId}")]
        public IActionResult Index(string restaurantId)
        {
            return Repository.GetAllByStatus(restaurantId);
        }

        [HttpGet]
        [Route("getAllCat/{restaurantId}")]
        public IActionResult GetAll(string restaurantId)
        {
            return Repository.GetAll(restaurantId);
        }
    }
}
