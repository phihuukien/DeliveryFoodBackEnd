using Food_Delivery_App_BackEnd.ModelDTO;
using Food_Delivery_App_BackEnd.Models.BusinessModels;
using Food_Delivery_App_BackEnd.Models.DataModels;
using Food_Delivery_App_BackEnd.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Food_Delivery_App_BackEnd.Repositories.ImplRepositories
{
    public class RepositoryBookMarks : IRepositoryBookMarks
    {
        FoodDeliveryAppDbContext _context;
        public RepositoryBookMarks(FoodDeliveryAppDbContext _context)
        {
            this._context = _context;
        }

        public JsonResult AddBookmark(string username, string restaurantId)
        {
            try
            {
                var bookMarkExist = _context.BookMarks.Find(x => x.RestaurantId == restaurantId && x.Username == username).FirstOrDefault();

                var filter = Builders<BookMarks>.Filter.And(Builders<BookMarks>.Filter.Eq("username", username),
                        Builders<BookMarks>.Filter.Eq("restaurantId", MongoDB.Bson.ObjectId.Parse(restaurantId)));
                var updates = Builders<BookMarks>.Update
                                                         .Set("username", username);
                var options = new UpdateOptions { IsUpsert = true };
                var result = _context.BookMarks.UpdateOne(filter, updates, options);
               
                var bookmarks = GetBookmarks(username).Value;
                return new JsonResult(new { Status = true, Message = "Bookmark added Successfully", Data = bookmarks });
             
            }
            catch (Exception ex)
            {
                return new JsonResult(new { Status = false, message = ex.Message });
            }
        }

        public JsonResult GetBookmarks(string username)
        {
            try
            {
                var response = _context.BookMarks.Aggregate()
                                                   .Match(x => x.Username == username)
                                                   .Lookup("restaurants", "restaurantId", "_id", "restaurant").ToList();

                var bookmarks = new List<ResponseBookMarks>();
                foreach (var item in response)
                {
                    bookmarks.Add(BsonSerializer.Deserialize<ResponseBookMarks>(item));
                }

                if (bookmarks.Count > 0)
                {
                    return new JsonResult(new
                    {
                        Status = true,
                        Message = "Bookmarks fetched Successfully",
                        Data = bookmarks
                    });
                }
                else
                {
                    return new JsonResult(new
                    {
                        Status = false,
                        Message = "Bookmarks not found",

                    });
                }

            }
            catch (Exception)
            {
                return new JsonResult(new { Status = false, Message = "Bookmarks fetching Failed" });
            }
        }

        public JsonResult RemoveBookmark(string username, string restaurantId)
        {
            try
            {
               
                    _context.BookMarks.DeleteOne(x => x.Username == username && x.RestaurantId == restaurantId);
                    var bookMarks = GetBookmarks(username).Value;
                    return new JsonResult(new { Status = true, Message = "Bookmark Removed Successfully", Data = bookMarks });

            }
            catch (Exception)
            {
                return new JsonResult(new { Status = false, Message = "Bookmark Removed Failed" });
            }
        }
    }
}
