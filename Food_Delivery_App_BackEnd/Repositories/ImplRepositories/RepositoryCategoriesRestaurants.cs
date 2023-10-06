using Food_Delivery_App_BackEnd.ModelDTO.Request;
using Food_Delivery_App_BackEnd.Models.BusinessModels;
using Food_Delivery_App_BackEnd.Models.DataModels;
using Food_Delivery_App_BackEnd.Repositories.IRepositories;
using Food_Delivery_App_BackEnd.Util;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Linq;

namespace Food_Delivery_App_BackEnd.Repositories.ImplRepositories
{
    public class RepositoryCategoriesRestaurants : IRepositoryCategoriesRestaurants
    {
        FoodDeliveryAppDbContext _context;
        public RepositoryCategoriesRestaurants(FoodDeliveryAppDbContext _context)
        {
            this._context = _context;

        }

        public IActionResult DeleteCategory(string id)
        {
            throw new NotImplementedException();
        }

        public IActionResult GetAllByStatus(String RestaurantId)
        {
            try
            {
                var tags = _context.CategoriesRestaurants.Find(x => x.RestaurantId == RestaurantId).ToList();
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

        public IActionResult GetCategoryById(string id)
        {

            try
            {
                var cateCheck = _context.CategoriesRestaurants.Find(x => x.Id == id).FirstOrDefault();
                if (cateCheck != null)
                {
                  
                    return new JsonResult(new { Status = true, Message = "SUCCESS" ,Data= cateCheck });
                }
                else
                {
                    return new JsonResult(new { Status = true, Message = "khong ton tai category" });
                }

            }
            catch (Exception ex)
            {
                return new JsonResult(new { Status = false, Message = ex.Message });
            }
        }

        public IActionResult InsertCategory(RequestCategory requestCategory)
        {
            try
            {
                var cateCheck = _context.CategoriesRestaurants.Find(x => x.Name == requestCategory.Name && x.RestaurantId == requestCategory.RestaurantId).FirstOrDefault();
                if (cateCheck == null)
                {
                    var category = new CategoriesRestaurants();
                    category.Status = requestCategory.Status;
                    category.Name = requestCategory.Name;
                    category.RestaurantId = requestCategory.RestaurantId;
                    if (requestCategory.Status == 1)
                    {
                        var restaurant = _context.Restaurants.Find(x => x.Id == requestCategory.RestaurantId).FirstOrDefault();
                        var catego = restaurant.Categories;
                        catego.Add(requestCategory.Name);
                        var filter = Builders<Restaurants>.Filter.Where(x => x.Id == requestCategory.RestaurantId);
                        var update = Builders<Restaurants>.Update
                            .Set(x => x.Categories, catego);
                        _context.Restaurants.UpdateOne(filter, update);
                       
                    }
                    _context.CategoriesRestaurants.InsertOne(category);
                    return new JsonResult(new { Status = true, Message = "SUCCESS" });
                }
                else
                {
                    return new JsonResult(new { Status = true, Message = "ton tai category" });
                }

            }
            catch (Exception ex)
            {
                return new JsonResult(new { Status = false, Message = ex.Message });
            }

        }

        public IActionResult UpdateCategory(RequestUpdateCategory requestCategory)
        {
            try
            {

                var nameCheck = _context.CategoriesRestaurants.Find(x => x.Id != requestCategory.Id && x.RestaurantId == requestCategory.RestaurantId
                                                                           && x.Name == requestCategory.Name).FirstOrDefault();
              
                var cateCheck = _context.CategoriesRestaurants.Find(x => x.Id == requestCategory.Id).FirstOrDefault();
                if (cateCheck != null && nameCheck == null)
                {
                    if (requestCategory.Status == 1)
                    {
                        var restaurant = _context.Restaurants.Find(x => x.Id == requestCategory.RestaurantId).FirstOrDefault();
                        var catego = restaurant.Categories.ToList();
                        var ca = catego.IndexOf(requestCategory.UpdateName);
                        if(ca > 0)
                        {
                            catego[ca] = requestCategory.Name;
                        }
                        else
                        {
                            catego.Add(requestCategory.Name);
                        }
                        
                        var filter = Builders<Restaurants>.Filter.Where(x => x.Id == requestCategory.RestaurantId);
                        var update = Builders<Restaurants>.Update
                            .Set(x => x.Categories, catego);
                        _context.Restaurants.UpdateOne(filter, update);
                    }
                    else if (requestCategory.Status == 2)
                    {
                        var restaurant = _context.Restaurants.Find(x => x.Id == requestCategory.RestaurantId).FirstOrDefault();
                        var catego = restaurant.Categories.ToList();
                        catego.Remove(requestCategory.UpdateName);

                        var filter = Builders<Restaurants>.Filter.Where(x => x.Id == requestCategory.RestaurantId);
                        var update = Builders<Restaurants>.Update
                            .Set(x => x.Categories, catego);
                        _context.Restaurants.UpdateOne(filter, update);
                      
                    }
                    var filterUp = Builders<CategoriesRestaurants>.Filter.Where(x => x.Id == requestCategory.Id);
                    var updateCate = Builders<CategoriesRestaurants>.Update
                        .Set(x => x.Name, requestCategory.Name)
                         .Set(x => x.Status, requestCategory.Status)
                         .Set(x => x.RestaurantId, requestCategory.RestaurantId);
                    _context.CategoriesRestaurants.UpdateOne(filterUp, updateCate);
                    return new JsonResult(new { Status = true, Message = "SUCCESS" });
                }
                else
                {
                    return new JsonResult(new { Status = true, Message = "khong ton tai category" });
                }

            }
            catch (Exception ex)
            {
                return new JsonResult(new { Status = false, Message = ex.Message });
            }
        }
    }
}
