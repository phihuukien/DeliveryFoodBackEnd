using Food_Delivery_App_BackEnd.Models.BusinessModels;
using Food_Delivery_App_BackEnd.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Food_Delivery_App_BackEnd.Repositories.ImplRepositories
{
    public class RepositoryFoods : IRepositoryFoods
    {
        FoodDeliveryAppDbContext _context;
        public RepositoryFoods(FoodDeliveryAppDbContext _context)
        {
            this._context = _context;

        }
        public  JsonResult GetOneFoodById(string foodId)
        {
            try {
                
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
              
                return new JsonResult(new { Status = false, Message = "Food finding failed", Error = "Food finding failed : "+ EX.Message });
            }
           
        }
    }
}
