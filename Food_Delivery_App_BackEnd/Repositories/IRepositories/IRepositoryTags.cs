using Food_Delivery_App_BackEnd.Models.DataModels;
using Microsoft.AspNetCore.Mvc;

namespace Food_Delivery_App_BackEnd.Repositories.IRepositories
{
    public interface IRepositoryTags
    {
        public IActionResult GetAll();
        public IActionResult GetAllTags();
    }
}
