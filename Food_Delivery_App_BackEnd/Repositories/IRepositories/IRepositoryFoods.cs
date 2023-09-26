using Food_Delivery_App_BackEnd.ModelDTO;
using Food_Delivery_App_BackEnd.Models.DataModels;
using Microsoft.AspNetCore.Mvc;

namespace Food_Delivery_App_BackEnd.Repositories.IRepositories
{
    public interface IRepositoryFoods
    {
        public JsonResult GetOneFoodById(string foodId);
        public JsonResult GetAllFoodByRestaurantId(string restaurantId);
        public JsonResult GetRestaurantsByNameFood(string name);
        public IActionResult AddFood(Foods food);
        public IActionResult UpdateFood(Foods food);
        public IActionResult DelelteFood(String id);
    }
}