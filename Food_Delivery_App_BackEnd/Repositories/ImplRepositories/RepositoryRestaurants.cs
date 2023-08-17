using Food_Delivery_App_BackEnd.ModelDTO;
using Food_Delivery_App_BackEnd.Models.BusinessModels;
using Food_Delivery_App_BackEnd.Models.DataModels;
using Food_Delivery_App_BackEnd.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Core.Bindings;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Food_Delivery_App_BackEnd.Repositories.ImplRepositories
{
    public class RepositoryRestaurants : IRepositoryRestaurants
    {
        FoodDeliveryAppDbContext _context;
        
        public RepositoryRestaurants(FoodDeliveryAppDbContext _context)
        {
            this._context = _context;

        }
        public IActionResult GetAllRestaurants()
        {
            try
            {
                var restaurants = _context.Restaurants.Find(Restaurants => true).ToList();
                if (restaurants !=null  && restaurants.Count()>0)
                {
                    return new JsonResult(new { Status = true, Message = "Restaurants found successfully", Data = restaurants });
                }
                else
                {
                    return new JsonResult(new { Status = false, Message = "No restaurants found"});
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    status = false,
                    message = "Restaurant finding failed",
                    error = "Restaurant finding failed: " + ex.Message
                });
            }


        }

        public IActionResult GetOneRestaurantById(string id)
        {
            try
            {
                var response = _context.Restaurants.Aggregate().Match(x => x.Id == id).Lookup("foods", "_id", "restaurantId", "foods").FirstOrDefault();

             
                var restaurant = BsonSerializer.Deserialize<ResponseRestaurant>(response);
                
                if (restaurant != null)
                {
                    return new JsonResult(new { Status = true, Message = "Restaurant found successfully", Data= restaurant });
                }
                else
                {
                    return new JsonResult(new { Status = false, Message = "No restaurant found"});
                }
              

            }
            catch (Exception ex)
            {
                return new JsonResult(new { Status = false, Message = "Restaurant finding failed", error= "Restaurant finding failed : "+ ex.Message});

            }
        }
    }
}
