using Food_Delivery_App_BackEnd.ModelDTO;
using Food_Delivery_App_BackEnd.ModelDTO.Request;
using Food_Delivery_App_BackEnd.ModelDTO.Response;
using Food_Delivery_App_BackEnd.Models.BusinessModels;
using Food_Delivery_App_BackEnd.Models.DataModels;
using Food_Delivery_App_BackEnd.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.IO;

namespace Food_Delivery_App_BackEnd.Repositories.ImplRepositories
{
    public class RepositoryReviews : IRepositoryReviews
    {
        FoodDeliveryAppDbContext _context;
        public RepositoryReviews(FoodDeliveryAppDbContext _context)
        {
            this._context = _context;
        }

        public IActionResult AddReview(RequestReview requestReview)
        {
            try
            {
                List<Reviews> reviews = new List<Reviews>();
                if (requestReview.FoodId?.Count() > 0)
                {
                    int k = 0;
                    List<string> imgName = new List<string>();
                    foreach (var item in requestReview.FoodId)
                    {
                        Reviews review = new Reviews();
                        review.Context = requestReview.Context;
                        review.Username = requestReview.Username;
                        review.Rate = requestReview.Rate;
                        review.OrderId = requestReview.OrderId;
                        if (requestReview?.ReviewImg?.Count() > 0 && k < requestReview.ReviewImg.Count())
                        {
                            foreach (var file in requestReview.ReviewImg)
                            {
                                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "reviews", file.FileName);
                                using (var stream = System.IO.File.Create(path))
                                {
                                    file.CopyTo(stream);
                                }
                                var imgPath = file.FileName;
                                imgName.Add(imgPath);
                                k++;
                            }
                        }
                        review.ReviewImg = imgName;
                        review.FoodId = item;
                        reviews.Add(review);

                    }

                    _context.Reviews.InsertMany(reviews);
                }
                var order = _context.Orders.Find(x => x.Id == requestReview.OrderId).FirstOrDefault();
                if (order != null)
                {
                    var filter = Builders<Orders>.Filter.Eq(x => x.Id, requestReview.OrderId);
                    var update = Builders<Orders>.Update.Set(x => x.ReviewStatus, 2);
                    _context.Orders.UpdateOne(filter, update);
                }


                return new JsonResult(new { Message = "Review Successfully", Status = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { Message = ex.Message, Status = false });
            }
        }

        public IActionResult GetOrderDetail(string orderId)
        {
            try
            {
                var orderFoodId = _context.OrderDetails.Find(x => x.OrderId == orderId).ToList();
                var foodIdList = new List<string>();
                foreach (var item in orderFoodId)
                {
                    foodIdList.Add(item.FoodId);
                }
                var responseOrderDetail = _context.OrderDetails.Aggregate()
                                       .Match(x => x.OrderId == orderId)
                                       .Lookup("foods", "foodId", "_id", "foods")
                                       .ToList();
                var ordersDetail = new List<ResponseOrderDetail>();
                foreach (var item in responseOrderDetail)
                {
                    ordersDetail.Add(BsonSerializer.Deserialize<ResponseOrderDetail>(item));
                }

                return new JsonResult(new { Message = "Success ", Status = true, OrdersDetail = ordersDetail, FoodIdList = foodIdList });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { Message = ex.Message, Status = false });
            }
        }

        public IActionResult GetReview(string foodId)
        {
            var response = _context.Reviews.Aggregate().Match(x => x.FoodId == foodId)
                                                       .Lookup("reviews", "foodId", "_id", "foods")
                                                       .Sort("{created_at:-1}")
                                                       .ToList();
            var reviews = new List<ResponseReview>();
            foreach (var item in response)
            {
                reviews.Add(BsonSerializer.Deserialize<ResponseReview>(item));
            }

            if (reviews.Count > 0)
            {
                return new JsonResult(new { Message = "Successfully", Status = true, Data = reviews });
            }
            return new JsonResult(new { Message = "No review found", Status = false });
        }

        public IActionResult EditReview(RequestReview requestReview)
        {
            throw new NotImplementedException();
        }

    }
}
