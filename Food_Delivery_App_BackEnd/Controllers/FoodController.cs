using Food_Delivery_App_BackEnd.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        [Route("{foodId}")]
        public IActionResult GetOneFoodById(string foodId)
        {
            return repositoryFoods.GetOneFoodById(foodId);
        }
    }
}
