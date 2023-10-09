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

        public IActionResult AddFood(RequestFood food)
        {
            try
            {
                var image = "";

                if (food.Image != null && food.Image.Length > 0)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "gallery", "square", food.Image.FileName);
                    using (var stream = System.IO.File.Create(path))
                    {
                        food.Image.CopyTo(stream);
                    }
                    image = System.IO.Path.GetFileNameWithoutExtension(food.Image.FileName);
                }
                else
                {
                    image = "";

                }
                var f = new Foods { Image = image, Category = food.Category, RestaurantId = food.RestaurantId, Ingredients = food.Ingredients, Name = food.Name, Price = food.Price, Description = food.Description };
                var update = Builders<Foods>.Update.Set("image", image)
                   .Set("name", food.Name).Set("price", food.Price).Set("category", food.Category).Set("description", food.Description).Set("ingredients", food.Ingredients).Set("restaurantId", food.RestaurantId);
                // xử lý ảnh
                _context.Foods.InsertOne(f);
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
                _context.Foods.DeleteOne(x => x.Id == id);
                return new JsonResult(new { status = true, Message = "Delete food successfully" });

            }
            catch (Exception EX)
            {

                return new JsonResult(new { Status = false, Message = "Delete food failed", Error = "Delete food failed : " + EX.Message });
            }
        }

        public JsonResult GetAllFoodByRestaurantId(string restaurantId, int page, int limit, string? textsearch)
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
                            .Group(BsonDocument.Parse("{ _id: '$restaurantId',restaurant:{'$first':'$restaurant'}}"))
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

        public IActionResult UpdateFood(RequestFood food)
        {
            try
            {
                var findfood = _context.Foods.Find(x => x.Id == food.Id).FirstOrDefault();
                if (findfood == null)
                {
                    return new JsonResult(new { status = true, Message = "Food Null" });
                }
                   var image = findfood?.Image;

                if (food.Image != null && food.Image.Length > 0)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "gallery", "square", food.Image.FileName);
                    using (var stream = System.IO.File.Create(path))
                    {
                        food.Image.CopyTo(stream);
                    }
                    image = System.IO.Path.GetFileNameWithoutExtension(food.Image.FileName);
                }

                var filter = Builders<Foods>.Filter.Eq(f => f.Id, food.Id);
                var update = Builders<Foods>.Update.Set("image", image).Set("status",food.Status)
                   .Set("name", food.Name).Set("price", food.Price).Set("category", food.Category).Set("description", food.Description).Set("ingredients", food.Ingredients);
                // xử lý ảnh
                _context.Foods.UpdateOne(filter,update);
                return new JsonResult(new { status = true, Message = "Food update successfully" });

            }
            catch (Exception EX)
            {

                return new JsonResult(new { Status = false, Message = "Foods update failed", Error = "Foods add failed : " + EX.Message });
            }
        }
    }
}