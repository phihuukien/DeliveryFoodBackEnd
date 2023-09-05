using Food_Delivery_App_BackEnd.ModelDTO;
using Food_Delivery_App_BackEnd.ModelDTO.Response;
using Food_Delivery_App_BackEnd.Models.BusinessModels;
using Food_Delivery_App_BackEnd.Models.DataModels;
using Food_Delivery_App_BackEnd.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Net.Http.Headers;

namespace Food_Delivery_App_BackEnd.Repositories.ImplRepositories
{
    public class RepositoryFoods : IRepositoryFoods
    {
        FoodDeliveryAppDbContext _context;
        public RepositoryFoods(FoodDeliveryAppDbContext _context)
        {
            this._context = _context;

        }
        public  JsonResult GetOneFoodById(string foodId)
        {
            try {
                
                var food = _context.Foods.Find(x => x.Id == foodId).FirstOrDefault();
                if (food != null)
                {
                    return new JsonResult(new { Status = true, Message = "Food found successfully", Data = food });
                }
                else
                {

                    return new JsonResult(new { Status = false, Message = "No Food found", Data = food });
                }

            }
            catch (Exception EX)
            {
              
                return new JsonResult(new { Status = false, Message = "Food finding failed", Error = "Food finding failed : "+ EX.Message });
            }
           
        }

        public JsonResult GetRestaurantsByNameFood(string name)
        {
            try {
                name = name.ToLower();
                // var food = _context.Foods.Find(x => x.Name.Contains(name)).ToList();
                var response = _context.Foods.Aggregate()
                            .Match(x => x.Name.ToLower().Contains(name))
                            .Lookup("restaurants", "restaurantId", "_id", "restaurant")
                           // .Group(BsonDocument.Parse("{ _id: '$restaurantId',restaurant:{'$first':'$restaurant'}}"))
                            .ToList();
                var foodItems = new List<ResponseFoods>();
                foreach (var item in response)
                {
                    foodItems.Add(BsonSerializer.Deserialize<ResponseFoods>(item));
                }
                if (foodItems != null)
                {
                    return new JsonResult(new { Status = true, Message = "Food found successfully", Data = foodItems });
                }
                else
                {

                    return new JsonResult(new { Status = false, Message = "No Food found", Data = foodItems });
                }

            }
            catch (Exception EX)
            {

                return new JsonResult(new { Status = false, Message = "Food finding failed", Error = "Food finding failed : " + EX.Message });
            }
        }
    }
}
