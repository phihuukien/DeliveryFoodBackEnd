using Food_Delivery_App_BackEnd.ModelDTO;
using Food_Delivery_App_BackEnd.Models.BusinessModels;
using Food_Delivery_App_BackEnd.Models.DataModels;
using Food_Delivery_App_BackEnd.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Text.RegularExpressions;

namespace Food_Delivery_App_BackEnd.Repositories.ImplRepositories
{
    public class RepositoryFoods : IRepositoryFoods
    {
        FoodDeliveryAppDbContext _context;
        public RepositoryFoods(FoodDeliveryAppDbContext _context)
        {
            this._context = _context;

        }

        public IActionResult AddFood(Foods food)
        {
            try
            {
                _context.Foods.InsertOne(food);
                return new JsonResult(new { status = true, Message = "Food add successfully" });

            }
            catch (Exception EX)
            {

                return new JsonResult(new { Status = false, Message = "Foods add failed", Error = "Foods add failed : " + EX.Message });
            }
        }

        public IActionResult DelelteFood(String id)
        {
            try
            {
                _context.Foods.DeleteOne(id);
                return new JsonResult(new { status = true, Message = "Delete food successfully" });

            }
            catch (Exception EX)
            {

                return new JsonResult(new { Status = false, Message = "Delete food failed", Error = "Delete food failed : " + EX.Message });
            }
        }

        public JsonResult GetAllFoodByRestaurantId(string restaurantId,int page,int limit, string? textsearch)
        {
            try
            {
                var filter = Builders<Foods>.Filter.Where(x => x.RestaurantId == restaurantId);
                if (!string.IsNullOrEmpty(textsearch))
                {
                    var regexFilter = Builders<Foods>.Filter.Regex("name", new BsonRegularExpression(new Regex(textsearch, RegexOptions.IgnoreCase)));
                    filter = Builders<Foods>.Filter.And(filter, regexFilter);
                }

                var find = _context.Foods.Find(filter);
                
                int skip = limit * (page - 1);
                var total_document = find.CountDocuments();
                var total_Page = total_document % limit == 0 ? total_document / limit : total_document / limit + 1;
                var foods = find.Skip(skip).Limit(limit).ToList();
                if (foods != null)
                {
                    return new JsonResult(new { Status = true, Message = "Food found successfully", Total_Page = total_Page, Data = foods });
                }
                else
                {

                    return new JsonResult(new { Status = false, Message = "No Food found", Data = foods });
                }

            }
            catch (Exception EX)
            {

                return new JsonResult(new { Status = false, Message = "Food finding failed", Error = "Food finding failed : " + EX.Message });
            }
        }

        public JsonResult GetOneFoodById(string foodId)
        {
            try
            {
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

                return new JsonResult(new { Status = false, Message = "Food finding failed", Error = "Food finding failed : " + EX.Message });
            }

        }

        public JsonResult GetRestaurantsByNameFood(string name)
        {
            try
            {
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

        public IActionResult UpdateFood(Foods food)
        {
            throw new NotImplementedException();
        }
    }
}