using Food_Delivery_App_BackEnd.ModelDTO;
using Food_Delivery_App_BackEnd.ModelDTO.Request;
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
                        restaurants.Poster.CopyTo(stream);
                    }
                    cover = System.IO.Path.GetFileNameWithoutExtension(restaurants.Poster.FileName);
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

        public IActionResult GetAllRestaurantsAdmin(int page)
        {
            int page_size = 2;
            int skip = page_size * (page - 1);
            var total_document = _context.Restaurants.CountDocuments(FilterDefinition<Restaurants>.Empty);
            var total_Page = total_document % page_size == 0 ? total_document / page_size : total_document / page_size + 1;
            var data = _context.Restaurants.Find(Restaurants => true).Skip(skip).Limit(page_size).ToList();
            return new JsonResult(new { Status = true, Page = page, Total_Page = total_Page ,Data=data });
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
                return new JsonResult(new { Status = true, Message = " get restaurants successfly" ,Data = restaurants});
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
                        restaurants.Poster.CopyTo(stream);
                    }
                    cover = System.IO.Path.GetFileNameWithoutExtension(restaurants.Poster.FileName);
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
                var update = Builders<Restaurants>.Update.Set("name", restaurants.Name).Set("logo", restaurants.Logo)
                    .Set("cover", restaurants.Cover).Set("poster", restaurants.Poster).Set("type", restaurants.Type)
                    .Set("location", restaurants.Location).Set("distance", restaurants.Distance)
                    .Set("times", restaurants.Times).Set("Tags", result);
                _context.Restaurants.UpdateOne(filter,update);
                return new JsonResult(new { status = true, Message = "Restaurants add successfully" });

            }
            catch (Exception EX)
            {

                return new JsonResult(new { Status = false, Message = "Restaurants add failed", Error = "Restaurants add failed : " + EX.Message });
            }
        }

    }
}
