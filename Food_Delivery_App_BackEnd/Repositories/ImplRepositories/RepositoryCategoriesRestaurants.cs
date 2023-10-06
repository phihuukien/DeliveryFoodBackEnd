using Food_Delivery_App_BackEnd.Models.BusinessModels;
using Food_Delivery_App_BackEnd.Models.DataModels;
using Food_Delivery_App_BackEnd.Repositories.IRepositories;
using Food_Delivery_App_BackEnd.Util;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Food_Delivery_App_BackEnd.Repositories.ImplRepositories
{
    public class RepositoryCategoriesRestaurants : IRepositoryCategoriesRestaurants
    {
        FoodDeliveryAppDbContext _context;
        public RepositoryCategoriesRestaurants(FoodDeliveryAppDbContext _context)
        {
            this._context = _context;

        }

        public IActionResult GetAll(string RestaurantsId)
        {

            try
            {
                var category = _context.CategoriesRestaurants.Find(x => x.RestaurantId == RestaurantsId).ToList();
                if (category != null && category.Count() > 0)
                {
                    return new JsonResult(new { Status = true, Message = "CategoriesRestaurants found successfully", Data = category });
                }
                else
                {
                    return new JsonResult(new { Status = false, Message = "No CategoriesRestaurants found" });
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    status = false,
                    message = "CategoriesRestaurants finding failed",
                    error = "CategoriesRestaurants finding failed: " + ex.Message
                });
            }
        }

        public IActionResult GetAllByStatus(String RestaurantId)
        {
            try
            {
                var tags = _context.CategoriesRestaurants.Find(x => x.Status == 1 && x.RestaurantId==RestaurantId).ToList();
                if (tags != null && tags.Count() > 0)
                {
                    return new JsonResult(new { Status = true, Message = "CategoriesRestaurants found successfully", Data = tags });
                }
                else
                {
                    return new JsonResult(new { Status = false, Message = "No CategoriesRestaurants found" });
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    status = false,
                    message = "CategoriesRestaurants finding failed",
                    error = "CategoriesRestaurants finding failed: " + ex.Message
                });
            }
        }
    }
}
