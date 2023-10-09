using Food_Delivery_App_BackEnd.Models.DataModels;
using Microsoft.AspNetCore.Mvc;

namespace Food_Delivery_App_BackEnd.Repositories.IRepositories
{
    public interface IRepositoryRestaurants
    {
        public IActionResult GetAllRestaurants();
        public IActionResult GetAllRestaurantsByRate();
        public IActionResult GetOneRestaurantById(string id);
        public IActionResult GetRestaurantById(string id);
        public IActionResult GetRestaurantsByTag(string tagName);
        public IActionResult GetAllRestaurantsAdmin(int page);
        public IActionResult GetRestaurantsByUsernamePartner(string username);
        public IActionResult PagingRestaurantsByUsername(string username, int limit, string? textSearch, int page);
        public IActionResult AddRestaurant(ResquestRestaurant restaurants);
        public IActionResult UpdateRestaurant(ResquestRestaurant restaurants);
    }
}
