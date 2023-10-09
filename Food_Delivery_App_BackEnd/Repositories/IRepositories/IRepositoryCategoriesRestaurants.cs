using Food_Delivery_App_BackEnd.ModelDTO.Request;
using Microsoft.AspNetCore.Mvc;

namespace Food_Delivery_App_BackEnd.Repositories.IRepositories
{
    public interface IRepositoryCategoriesRestaurants
    {
        public IActionResult GetAllByStatus(String RestaurantsId);
        public IActionResult InsertCategory(RequestCategory requestCategory);
        public IActionResult GetCategoryById(string id);
        public IActionResult UpdateCategory(RequestUpdateCategory requestCategory);
        public IActionResult DeleteCategory(string id);
        public IActionResult GetAll(String RestaurantsId);
    }
}
