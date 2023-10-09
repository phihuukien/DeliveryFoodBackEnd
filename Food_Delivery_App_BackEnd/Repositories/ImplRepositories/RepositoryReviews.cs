using Food_Delivery_App_BackEnd.ModelDTO;
using Food_Delivery_App_BackEnd.ModelDTO.Request;
using Food_Delivery_App_BackEnd.ModelDTO.Response;
using Food_Delivery_App_BackEnd.Models.BusinessModels;
using Food_Delivery_App_BackEnd.Models.DataModels;
using Food_Delivery_App_BackEnd.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using MongoDB.Bson;
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

        public IActionResult GetOrderToReview(string username)
        {

            var response = _context.Orders.Aggregate()
                                          .Match(x => x.Username == username && (x.DeliveringStatus == 5))
                                          .Lookup("restaurants", "restaurantId", "_id", "restaurant")
                                          .Sort("{dateCreated:-1}")
                                          .ToList();
            var ordersHistory = new List<ResponseOrders>();
            foreach (var item in response)
            {
                ordersHistory.Add(BsonSerializer.Deserialize<ResponseOrders>(item));
            }

            if (ordersHistory.Count > 0)
            {
                return new JsonResult(new { Message = "Successfully", Status = true, Data = ordersHistory });
            }
            return new JsonResult(new { Message = "Not orders history", Status = false });
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

        public IActionResult GetRating(string restaurantId)
        {
            try
            {
                var response = _context.Orders.Aggregate().Match(x => x.RestaurantId == restaurantId & x.ReviewStatus == 2)
                                                   .Lookup("reviews", "_id", "orderId", "reviews")
                                                   .Project<BsonDocument>("{ reviews: 1}")
                                                   .ToList();
                if (response.Count > 0)
                {
                    var reviews = new List<ResponseRating>();
                    foreach (var item in response)
                    {
                        reviews.Add(BsonSerializer.Deserialize<ResponseRating>(item));
                    }
                    var totalRating = 0;
                    var totalOrderRated = 0;
                    foreach (var item in reviews)
                    {
                        if (item.Reviews.Count > 0)
                        {
                            totalRating += item.Reviews[0].Rate;
                            totalOrderRated++;
                        }
                    }
                    var avgRating = (double)totalRating / totalOrderRated;
                    return new JsonResult(new { Message = "Successfully", Status = true, AvgRating = avgRating, totalOrderRated = reviews.Count() });
                }
                return new JsonResult(new { Message = "Successfully", Status = true, AvgRating = 0, totalOrderRated = 0 });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { Message = ex.Message, Status = false, AvgRating = 0 });
            }

        }

        public IActionResult GetReviewByRestaurant(string restaurantId, string sort_order, string sort_orderBy, int page, string from_date, string to_date)
        {
            try
            {
                var response = _context.Orders.Aggregate().Match(x => x.RestaurantId == restaurantId && x.ReviewStatus == 2)
                                                   .Lookup("reviews", "_id", "orderId", "reviews")
                                                   .Lookup("foods", "reviews.foodId", "_id", "foods")
                                                   .Project<BsonDocument>("{ reviews: 1, foods: 1}")
                                                   .Sort("{dateCreated:-1}")
                                                   .ToList();

                var reviewParent = new List<ResponseRating>();
                if (response.Count > 0)
                {
                    foreach (var item in response)
                    {
                        reviewParent.Add(BsonSerializer.Deserialize<ResponseRating>(item));
                    }
                }
                var reviewChildren = new List<ResponseRestaurantReviews>();


                foreach (var item1 in reviewParent)
                {
                    int f = 0;
                    foreach (var item2 in item1.Reviews)
                    {
                        var obj = new ResponseRestaurantReviews();
                        obj.username = item2.Username;
                        obj.context = item2.Context;
                        obj.Date = item2.Date;
                        obj.Rate = item2.Rate;
                        obj.ReviewImg = item2.ReviewImg;
                        obj.foodName = item1.Foods[f].Name;
                        obj.ReviewId = item2.Id;
                        reviewChildren.Add(obj);
                        f++;
                    }

                }


                if (sort_orderBy == "date" && sort_order == "ASC")
                {
                    reviewChildren = reviewChildren.OrderBy(p => p.Date).ToList();
                }
                else if (sort_orderBy == "date" && sort_order == "DESC")
                {
                    reviewChildren = reviewChildren.OrderByDescending(p => p.Date).ToList();
                }
                else if (sort_orderBy == "rate" && sort_order == "ASC")
                {
                    reviewChildren = reviewChildren.OrderBy(p => p.Rate).ToList();
                }
                else if (sort_orderBy == "rate" && sort_order == "DESC")
                {
                    reviewChildren = reviewChildren.OrderByDescending(p => p.Rate).ToList();
                }

                int page_size = 4;
                int skip = page_size * (page - 1);
                var total_document = reviewChildren.Count();
                var total_Page = total_document % page_size == 0 ? total_document / page_size : total_document / page_size + 1;
                var reviewsList = reviewChildren.Skip(skip).Take(page_size).ToList();

                return new JsonResult(new { Message = "Successfully", Sort_orderBy = sort_orderBy, Sort_order = sort_order, Page = page, Total_Page = total_Page, Status = true, Data = reviewsList });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { Message = ex.Message, Status = false });
            }
        }

        public IActionResult DeleteReview(string reviewId)
        {
            try
            {
                var response = _context.Reviews.DeleteOne(a => a.Id == reviewId);
                return new JsonResult(new { Message = "Delete Successfully", Status = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { Message = ex.Message, Status = false });
            }

        }
    }
}
