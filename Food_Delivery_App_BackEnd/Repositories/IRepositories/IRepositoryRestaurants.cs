using Food_Delivery_App_BackEnd.Models.DataModels;
using Microsoft.AspNetCore.Mvc;

namespace Food_Delivery_App_BackEnd.Repositories.IRepositories
{
    public interface IRepositoryRestaurants
    {
        public IActionResult GetAllRestaurants();
        public IActionResult GetOneRestaurantById(string id);
    }
}
