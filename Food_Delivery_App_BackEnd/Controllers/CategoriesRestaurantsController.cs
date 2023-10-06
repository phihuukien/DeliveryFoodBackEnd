using Food_Delivery_App_BackEnd.ModelDTO.Request;
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

        [HttpPost]
        [Route("insert-category")]
        public IActionResult InsertCategory([FromBody] RequestCategory requestCategory)
        {
            return Repository.InsertCategory(requestCategory);
        }
        [HttpPost]
        [Route("update-category")]
        public IActionResult UpdateCategory([FromBody] RequestUpdateCategory requestCategory)
        {
            return Repository.UpdateCategory(requestCategory);
        }

        [HttpGet]
        [Route("get-cate-by-id/{id}")]
        public IActionResult GetCategoryById(string id)
        {
            return Repository.GetCategoryById(id);
        }
    }
}
