using Food_Delivery_App_BackEnd.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Food_Delivery_App_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private IRepositoryTags repositoryTags;

        public TagsController(IRepositoryTags repositoryTags)
        {

            this.repositoryTags = repositoryTags;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return repositoryTags.GetAll();
        }

        // GET api/<TagsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<TagsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TagsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TagsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
