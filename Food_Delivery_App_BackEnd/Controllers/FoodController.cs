using Food_Delivery_App_BackEnd.Models.DataModels;
using Food_Delivery_App_BackEnd.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace Food_Delivery_App_BackEnd.Controllers
{
    [Route("api/food")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private IRepositoryFoods repositoryFoods;
        public FoodController(IRepositoryFoods repositoryFoods)
        {
            this.repositoryFoods = repositoryFoods;
        }

        [HttpGet]
        [Route("getAllFoods/{restaurantId}")]
        public IActionResult GetAllFoodByRestaurantId(string restaurantId,int page, int limit, string? textsearch)
        {
            return repositoryFoods.GetAllFoodByRestaurantId(restaurantId,page,limit,textsearch);
        }

        [HttpGet]
        [Route("{foodId}")]
        public IActionResult GetOneFoodById(string foodId)
        {
            return repositoryFoods.GetOneFoodById(foodId);
        }

        [HttpGet]
        [Route("nameFood/{name}")]
        public IActionResult GetRestaurantsByNameFood(string name)
        {
            return repositoryFoods.GetRestaurantsByNameFood(name);
        }


        [HttpPost]
        [Route("addFood")]
        public IActionResult AddFood([FromBody] Foods food)
        {
            return repositoryFoods.AddFood(food);
        }

        [HttpDelete]
        [Route("deleteFood/{id}")]
        public IActionResult DeleteFood(String id)
        {
            return repositoryFoods.DelelteFood(id);
        }
    }
}