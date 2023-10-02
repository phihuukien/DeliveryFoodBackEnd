using Food_Delivery_App_BackEnd.Models.DataModels;
using Food_Delivery_App_BackEnd.Repositories.ImplRepositories;
using Food_Delivery_App_BackEnd.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Food_Delivery_App_BackEnd.Controllers
{
    [Route("api/tags")]
    [ApiController]
    [Authorize]
    public class TagsController : ControllerBase
    {
        private IRepositoryTags repositoryTags;
        public TagsController(IRepositoryTags repositoryTags)
        {
            this.repositoryTags = repositoryTags;
        }

        [HttpGet]
        [Route("getAll")]
        public IActionResult GetAllTags()
        {
            return repositoryTags.GetAllTags();
        }
    }
}
