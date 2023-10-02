using Food_Delivery_App_BackEnd.Models.BusinessModels;
using Food_Delivery_App_BackEnd.Models.DataModels;
using Food_Delivery_App_BackEnd.Repositories.IRepositories;
using Food_Delivery_App_BackEnd.Util;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Food_Delivery_App_BackEnd.Repositories.ImplRepositories
{
    public class RepositoryTags : IRepositoryTags
    {
        FoodDeliveryAppDbContext _context;
        public RepositoryTags(FoodDeliveryAppDbContext _context)
        {
            this._context = _context;
        }
        public IActionResult GetAllTags()
        {
            try
            {
                var response = _context.Tags.Find(x => x.Status == 2).ToList();
                if (response.Count > 0)
                {
                    return new JsonResult(new { Message = "Successfully", Status = true, Data = response });
                }
                else
                {
                    return new JsonResult(new { Message = "No tags found", Status = false });
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new { Message = ex.Message, Status = false });
            }
        }
    }
}
