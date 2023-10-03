using Food_Delivery_App_BackEnd.Models.DataModels;
using Microsoft.AspNetCore.Mvc;

namespace Food_Delivery_App_BackEnd.Repositories.IRepositories
{
    public interface IRepositoryRestaurants
    {
        public IActionResult GetAllRestaurants();
        public IActionResult GetOneRestaurantById(string id);

        public IActionResult GetAllRestaurantsAdmin(int page);
        public IActionResult GetRestaurantsByUsernamePartner(string username);
        public IActionResult PagingRestaurantsByUsername(string username, int limit, string? textSearch, int page);

        public IActionResult AddRestaurant(Restaurants restaurants);

        public IActionResult UpdateRestaurant(Restaurants restaurants);
    }
}
