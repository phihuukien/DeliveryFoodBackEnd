using Microsoft.AspNetCore.Mvc;

namespace Food_Delivery_App_BackEnd.Repositories.IRepositories
{
    public interface IRepositoryFoods
    {
        public JsonResult GetOneFoodById(string foodId);
        public JsonResult GetRestaurantsByNameFood(string name);
    }
}
