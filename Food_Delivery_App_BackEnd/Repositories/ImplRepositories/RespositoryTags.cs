using Food_Delivery_App_BackEnd.ModelDTO;
using Food_Delivery_App_BackEnd.Models.BusinessModels;
using Food_Delivery_App_BackEnd.Models.DataModels;
using Food_Delivery_App_BackEnd.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Core.Bindings;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace Food_Delivery_App_BackEnd.Repositories.ImplRepositories
{
    public class RespositoryTags : IRepositoryTags
    {
        FoodDeliveryAppDbContext _context;
        public RespositoryTags(FoodDeliveryAppDbContext _context)
        {
            this._context = _context;

        }
        public IActionResult GetAll()
        {
            try
            {
                var tags = _context.Tags.Find(Tag=>true).ToList();
                if (tags != null && tags.Count() > 0)
                {
                    return new JsonResult(new { Status = true, Message = "Tags found successfully", Data = tags });
                }
                else
                {
                    return new JsonResult(new { Status = false, Message = "No Tags found" });
                }
            }catch (Exception ex)
            {
                return new JsonResult(new
                {
                    status = false,
                    message = "Tags finding failed",
                    error = "Tags finding failed: " + ex.Message
                });
            }

        }
    }
}
