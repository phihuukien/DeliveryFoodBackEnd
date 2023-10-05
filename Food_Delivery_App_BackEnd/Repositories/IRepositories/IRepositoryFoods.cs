using Food_Delivery_App_BackEnd.ModelDTO;
using Food_Delivery_App_BackEnd.Models.DataModels;
using Microsoft.AspNetCore.Mvc;

namespace Food_Delivery_App_BackEnd.Repositories.IRepositories
{
    public interface IRepositoryFoods
    {
        public JsonResult GetOneFoodById(string foodId);
        public JsonResult GetAllFoodByRestaurantId(string restaurantId,int page, int limit, string? textsearch);
        public JsonResult GetRestaurantsByNameFood(string name);
        public IActionResult AddFood(RequestFood food);
        public IActionResult UpdateFood(RequestFood food);
        public IActionResult DelelteFood(String id);
    }
}