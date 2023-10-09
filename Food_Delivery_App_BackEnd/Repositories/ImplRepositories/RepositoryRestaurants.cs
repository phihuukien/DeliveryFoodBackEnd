using Food_Delivery_App_BackEnd.ModelDTO;
using Food_Delivery_App_BackEnd.ModelDTO.Request;
using Food_Delivery_App_BackEnd.ModelDTO.Response;
using Food_Delivery_App_BackEnd.Models.BusinessModels;
using Food_Delivery_App_BackEnd.Models.DataModels;
using Food_Delivery_App_BackEnd.Repositories.IRepositories;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Core.Bindings;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace Food_Delivery_App_BackEnd.Repositories.ImplRepositories
{
    public class RepositoryRestaurants : IRepositoryRestaurants
    {
        FoodDeliveryAppDbContext _context;

        public RepositoryRestaurants(FoodDeliveryAppDbContext _context)
        {
            this._context = _context;

        }

        public IActionResult AddRestaurant(ResquestRestaurant restaurants)
        {
            try
            {
                //string input = "Hello,World,How,Are,You";
                //char separator = ',';
                //string[] result = input.Split(separator);
                string[] result = restaurants.Tags[0].Split(',');
                var d = result.ToList();
                var logo = "";
                var poster = "";
                var cover = "";
                //anh logo
                if (restaurants.Logo != null && restaurants.Logo.Length > 0)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "logo", restaurants.Logo.FileName);
                    using (var stream = System.IO.File.Create(path))
                    {
                        restaurants.Logo.CopyTo(stream);
                    }
                    logo = System.IO.Path.GetFileNameWithoutExtension(restaurants.Logo.FileName);
                }
                else
                {
                    logo = "";

                }



                if (restaurants.Poster != null && restaurants.Poster.Length > 0)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "poster", restaurants.Poster.FileName);
                    using (var stream = System.IO.File.Create(path))
                    {
                        restaurants.Poster.CopyTo(stream);
                    }
                    poster = System.IO.Path.GetFileNameWithoutExtension(restaurants.Poster.FileName);
                }
                else
                {
                    poster = "";

                }


                if (restaurants.Cover != null && restaurants.Cover.Length > 0)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "gallery", "square", restaurants.Cover.FileName);
                    using (var stream = System.IO.File.Create(path))
                    {
                        restaurants.Cover.CopyTo(stream);
                    }
                    cover = System.IO.Path.GetFileNameWithoutExtension(restaurants.Cover.FileName);
                }
                else
                {
                    cover = "";

                }



                var img = new ImageR
                {
                    Logo = logo,
                    Poster = poster,
                    Cover = cover
                };

                var r = new Restaurants {Images=img, Name = restaurants.Name , Distance = restaurants.Distance,Tags= d, Location=restaurants.Location,Times=restaurants.Times,Type=restaurants.Type,Username=restaurants.Username};
                _context.Restaurants.InsertOne(r);
                return new JsonResult(new { status = true, Message = "Restaurants add successfully" });

            }
            catch (Exception EX)
            {

                return new JsonResult(new { Status = false, Message = "Restaurants add failed", Error = "Restaurants add failed : " + EX.Message });
            }
        }

        public IActionResult GetAllRestaurants()
        {
            try
            {
                var restaurants = _context.Restaurants.Find(Restaurants => true).ToList();
                if (restaurants != null && restaurants.Count() > 0)
                {
                    return new JsonResult(new { Status = true, Message = "Restaurants found successfully", Data = restaurants });
                }
                else
                {
                    return new JsonResult(new { Status = false, Message = "No restaurants found" });
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

        public IActionResult GetAllRestaurantsAdmin(int page)
        {
            int page_size = 2;
            int skip = page_size * (page - 1);
            var total_document = _context.Restaurants.CountDocuments(FilterDefinition<Restaurants>.Empty);
            var total_Page = total_document % page_size == 0 ? total_document / page_size : total_document / page_size + 1;
            var data = _context.Restaurants.Find(Restaurants => true).Skip(skip).Limit(page_size).ToList();
            return new JsonResult(new { Status = true, Page = page, Total_Page = total_Page, Data = data });
        }

        public IActionResult GetAllRestaurantsByRate()
        {
            try
            {
                var restaurantsIds = _context.Orders.Find(x => x.ReviewStatus == 2).ToList();
                var response2 = _context.Orders.Aggregate().Match(x=> x.ReviewStatus == 2)
                                                  .Group(BsonDocument.Parse("{ _id: '$restaurantId'}"))
                                                  .ToList();
                var resString = new List<ResponseId>();
                foreach (var item2 in response2)
                {
                    resString.Add(BsonSerializer.Deserialize<ResponseId>(item2));
                }
                var responseRestaurants = new List<ResponseRestaurantsByRate>();
                foreach (var item in resString)
                {
                    var response = _context.Orders.Aggregate().Match(x => x.RestaurantId == item.Id & x.ReviewStatus == 2)
                                                  .Lookup("restaurants", "restaurantId", "_id", "restaurant")
                                                  .Lookup("reviews", "_id", "orderId", "reviews")
                                                  .Project<BsonDocument>("{ reviews: 1,restaurant:1}")
                                                  .ToList();
                    if (response.Count > 0)
                    {
                        var reviews = new List<ResponseRateRes>();
                        foreach (var item2 in response)
                        {
                            reviews.Add(BsonSerializer.Deserialize<ResponseRateRes>(item2));
                        }
                        var totalRating = 0;
                        var totalOrderRated = 0;
                        foreach (var item2 in reviews)
                        {
                            if (item2.Reviews.Count > 0)
                            {
                                totalRating += item2.Reviews[0].Rate;
                                totalOrderRated++;
                            }
                        }
                        var avgRating = (double)totalRating / totalOrderRated;
                        var resByRate = new ResponseRestaurantsByRate();
                        resByRate.Id = reviews[0].Restaurant[0].Id;
                        resByRate.Name = reviews[0].Restaurant[0].Name;
                        resByRate.Image = reviews[0].Restaurant[0].Images;
                        resByRate.Rate = avgRating;
                        resByRate.Tags = reviews[0].Restaurant[0].Tags;
                        resByRate.TotalOrderRated = totalOrderRated;
                        responseRestaurants.Add(resByRate);
                    }
                }
                var resultData = responseRestaurants.Where(x => x.Rate > 4.0).ToList();
                if (resultData.Count<0)
                {
                    resultData = responseRestaurants.Where(x => x.Rate > 3.0).ToList();
                    if (resultData.Count <0)
                    {
                        resultData = responseRestaurants.Where(x => x.Rate > 2.0).ToList();
                    }
                }

                return new JsonResult(new { Message = "Successfully", Status = true, Data = resultData });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { Message = ex.Message, Status = false, });
            }
        }

        public IActionResult GetOneRestaurantById(string id)
        {
            try
            {
                var response = _context.Restaurants.Aggregate().Match(x => x.Id == id).Lookup("foods", "_id", "restaurantId", "foods")
                      .Project<BsonDocument>("{ username: 0 }").FirstOrDefault();


                var restaurant = BsonSerializer.Deserialize<ResponseRestaurant>(response);

                if (restaurant != null)
                {
                    return new JsonResult(new { Status = true, Message = "Restaurant found successfully", Data = restaurant });
                }
                else
                {
                    return new JsonResult(new { Status = false, Message = "No restaurant found" });
                }


            }
            catch (Exception ex)
            {
                return new JsonResult(new { Status = false, Message = "Restaurant finding failed", error = "Restaurant finding failed : " + ex.Message });

            }
        }

        public IActionResult GetRestaurantsByTag(string tagName)
        {
            try
            {
                var response = Builders<Restaurants>.Filter.In("tags", new[] { tagName });
                var result = _context.Restaurants.Find(response).ToList();
                if (result.Count > 0)
                {
                    return new JsonResult(new { Status = true, Message = "Restaurant found successfully", Data = result });
                }
                else
                {
                    return new JsonResult(new { Status = false, Message = "No restaurant found" });
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new { Status = false, Message = "Restaurant finding failed", error = "Restaurant finding failed : " + ex.Message });

            }
        }

        public IActionResult GetRestaurantById(string id)
        {
            try
            {
                var restaurant = _context.Restaurants.Find(r => r.Id == id).FirstOrDefault();

                if (restaurant != null)
                {
                    return new JsonResult(new { Status = true, Message = "Restaurant found successfully", Data = restaurant });
                }
                else
                {
                    return new JsonResult(new { Status = false, Message = "No restaurant found" });
                }


            }
            catch (Exception ex)
            {
                return new JsonResult(new { Status = false, Message = "Restaurant finding failed", error = "Restaurant finding failed : " + ex.Message });

            }
        }

        public IActionResult GetRestaurantsByUsernamePartner(string username)
        {
            var restaurants = _context.Restaurants.Find(x => x.Username == username).ToList();
            if (restaurants.Count > 0)
            {
                return new JsonResult(new { Status = true, Message = " get restaurants successfly", Data = restaurants });
            }

            return new JsonResult(new { Status = false, Message = "No restaurant found" });
        }

        public IActionResult PagingRestaurantsByUsername(string username, int limit, string? textSearch, int page)
        {
            var filter = Builders<Restaurants>.Filter.Where(x => x.Username == username);

            if (!string.IsNullOrEmpty(textSearch))
            {
                var regexFilter = Builders<Restaurants>.Filter.Regex("Name", new BsonRegularExpression(new Regex(textSearch, RegexOptions.IgnoreCase)));
                filter = Builders<Restaurants>.Filter.And(filter, regexFilter);
            }

            var find = _context.Restaurants.Find(filter);

            int skip = limit * (page - 1);
            var total_document = find.CountDocuments();
            var total_Page = total_document % limit == 0 ? total_document / limit : total_document / limit + 1;
            var restaurants = find.Skip(skip).Limit(limit).ToList();
            if (restaurants.Count > 0)
            {
                return new JsonResult(new { Status = true, Message = " get restaurants successfly", total_Page = total_Page, Data = restaurants });
            }

            return new JsonResult(new { Status = false, Message = "No restaurant found" });
        }

        public IActionResult UpdateRestaurant(ResquestRestaurant restaurants)
        {
            try
            {
                var findRes = _context.Restaurants.Find(x => x.Id == restaurants.Id).FirstOrDefault();
                if(findRes == null)
                {
                    return new JsonResult(new { status = true, Message = "Restaurant not found" });
                }
                string[] tags = restaurants.Tags[0].Split(',');
                string[] cates = restaurants.Categories[0].Split(',');
                var logo = "";
                var poster = "";
                var cover = "";
                //anh logo
                if (restaurants.Logo != null && restaurants.Logo.Length > 0)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "logo", restaurants.Logo.FileName);
                    using (var stream = System.IO.File.Create(path))
                    {
                        restaurants.Logo.CopyTo(stream);
                    }
                    logo = System.IO.Path.GetFileNameWithoutExtension(restaurants.Logo.FileName);
                }
                else
                {
                    logo = findRes.Images.Logo;

                }



                if (restaurants.Poster != null && restaurants.Poster.Length > 0)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "poster", restaurants.Poster.FileName);
                    using (var stream = System.IO.File.Create(path))
                    {
                        restaurants.Poster.CopyTo(stream);
                    }
                    poster = System.IO.Path.GetFileNameWithoutExtension(restaurants.Poster.FileName);
                }
                else
                {
                    poster = findRes.Images.Poster;

                }


                if (restaurants.Cover != null && restaurants.Cover.Length > 0)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "gallery", "square", restaurants.Cover.FileName);
                    using (var stream = System.IO.File.Create(path))
                    {
                        restaurants.Cover.CopyTo(stream);
                    }
                    cover = System.IO.Path.GetFileNameWithoutExtension(restaurants.Cover.FileName);
                }
                else
                {
                    cover = findRes.Images.Cover;

                }

                var img = new ImageR
                {
                    Logo = logo,
                    Poster = poster,
                    Cover = cover
                };
                var filter = Builders<Restaurants>.Filter.Eq(r => r.Id, restaurants.Id);
                var update = Builders<Restaurants>.Update.Set("name", restaurants.Name)
                    .Set("images", img).Set("type", restaurants.Type)
                    .Set("location", restaurants.Location).Set("distance", restaurants.Distance)
                    .Set("times", restaurants.Times).Set("tags", tags.ToList()).Set("categories", cates.ToList());
                _context.Restaurants.UpdateOne(filter,update);
                return new JsonResult(new { status = true, Message = "Restaurants update successfully" });

            }
            catch (Exception EX)
            {

                return new JsonResult(new { Status = false, Message = "Restaurants update failed", Error = "Restaurants update failed : " + EX.Message });
            }
        }

     
    }
}
