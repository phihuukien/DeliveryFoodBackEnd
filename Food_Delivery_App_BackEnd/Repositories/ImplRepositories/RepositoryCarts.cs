using Food_Delivery_App_BackEnd.ModelDTO;
using Food_Delivery_App_BackEnd.ModelDTO.Request;
using Food_Delivery_App_BackEnd.ModelDTO.Response;
using Food_Delivery_App_BackEnd.Models.BusinessModels;
using Food_Delivery_App_BackEnd.Models.DataModels;
using Food_Delivery_App_BackEnd.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharpCompress.Common;
using System.Collections.Generic;
using System.Linq;

namespace Food_Delivery_App_BackEnd.Repositories.ImplRepositories
{
    public class RepositoryCarts : IRepositoryCarts
    {
        FoodDeliveryAppDbContext _context;
        public RepositoryCarts(FoodDeliveryAppDbContext _context)
        {
            this._context = _context;
        }
        public IActionResult AddToCart(string username, RequestCart requestCart)
        {
            try
            {
                var filter = Builders<Cart>.Filter.And(Builders<Cart>.Filter.Eq("username", username),
                                                       Builders<Cart>.Filter.Eq("foodId", MongoDB.Bson.ObjectId.Parse(requestCart.FoodId)),
                                                       Builders<Cart>.Filter.Eq("restaurantId", MongoDB.Bson.ObjectId.Parse(requestCart.RestaurantId))
                                                       );
                var updates = Builders<Cart>.Update.Inc("count", 1)
                                                    .Inc("price", requestCart.Price);
                var options = new UpdateOptions { IsUpsert = true };
                var result = _context.Cart.UpdateOne(filter, updates, options);
                var cartResponse = GetCartDetail(username); 
                return new JsonResult(new { Status = true, Message = "Item Added to Cart Successfully", Data = cartResponse.Value });
            }
            catch (Exception)
            {
                return new JsonResult(new { Status = false, Message = "Item Added to Cart Failed" });
            }
        }

        public IActionResult RemoveFromCart(string username, RequestCart requestCart)
        {
            try
            {
                var cart = _context.Cart.Find(x => x.Username == username && x.FoodId == requestCart.FoodId).FirstOrDefault();
                var filter = Builders<Cart>.Filter.And(Builders<Cart>.Filter.Eq("username", username),
                                               Builders<Cart>.Filter.Eq("foodId", MongoDB.Bson.ObjectId.Parse(requestCart.FoodId)));
                if (cart.Count == 1)
                {
                    var results = _context.Cart.DeleteOne(filter);
                    var cartResponse = GetCartDetail(username);
                    return new JsonResult(new { Status = true, Message = "Item Removed from Cart Successfully",Data = cartResponse.Value });
                }

                var updates = Builders<Cart>.Update.Inc("count", -1)
                                                   .Inc("price", -requestCart.Price);
                var options = new UpdateOptions { IsUpsert = true };
                var result = _context.Cart.UpdateOne(filter, updates, options);
                var cartsResponse = GetCartDetail(username);
                return new JsonResult(new { Status = true, Message = "Item Removed from Cart Successfully",Data = cartsResponse.Value });

            }
            catch (Exception)
            {
                return new JsonResult(new { Status = false, Message = "Item Removed from Cart Failed" });
            }

        }

        public JsonResult GetCartItems(string username)
        {
            try
            {
                var metaData = new MetaData();
                var response = _context.Cart.Aggregate()
                                            .Match(x => x.Username == username)
                                            .Lookup("restaurants", "restaurantId", "_id", "restaurant")
                                            .Group(BsonDocument.Parse("{ _id: '$restaurantId',username:{'$first':'$username'}, count: { '$sum': 1},price:{'$sum':'$price'},restaurant:{'$first':'$restaurant'}}"))
                                            .Project<BsonDocument>("{ _id: 1, username:1,count: 1, price:1,restaurant:1 }")
                                            .ToList();


                var cartItems = new List<ResponseCarts>();
                foreach (var item in response)
                {
                    cartItems.Add(BsonSerializer.Deserialize<ResponseCarts>(item));
                }

                if (cartItems.Count > 0)
                {
                    return new JsonResult(new
                    {
                        Status = true,
                        Message = "Cart items fetched Successfully",
                        Data = cartItems
                        
                    });
                }
                else
                {
                    return new JsonResult(new { Status = false, Message = "Cart items not found",
                       
                    });
                }

            }
            catch (Exception ex )
            {
                return new JsonResult(new { Status = false, Message = ex.Message });
            }
        }

        public JsonResult GetCartDetail(string username)
        {
            try {
                var response = _context.Cart.Aggregate()
                                            .Match(x => x.Username == username)
                                            .Lookup("foods", "foodId", "_id", "food")
                                            .Project<BsonDocument>("{'restaurantId':0,'price':0,'username':0}")
                                            .ToList();
                var cartItems = new List<ResponseDetailCarts>();
                foreach (var item in response)
                {
                    cartItems.Add(BsonSerializer.Deserialize<ResponseDetailCarts>(item));
                }
              
                if (cartItems.Count > 0)
                {
                    return new JsonResult(new
                    {
                        Status = true,
                        Message = "Cart items fetched Successfully",
                        Data = new
                        {
                            cartItems
                        }

                    });
                }
                else
                {
                    return new JsonResult(new
                    {
                        Status = false,
                        Message = "Cart items not found",

                    });
                }

            }
            catch (Exception ex)
            {
                return new JsonResult(new { Status = false, Message = ex.Message });
            }
        }
    }
}
