using Microsoft.AspNetCore.Mvc;

namespace Food_Delivery_App_BackEnd.Repositories.IRepositories
{
    public interface IRepositoryCategoriesRestaurants
    {
        public IActionResult GetAllByStatus(String RestaurantsId);
        public IActionResult GetAll(String RestaurantsId);
    }
}
