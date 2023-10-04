using Food_Delivery_App_BackEnd.ModelDTO;
using Food_Delivery_App_BackEnd.Models.BusinessModels;
using Food_Delivery_App_BackEnd.Models.DataModels;
using Food_Delivery_App_BackEnd.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Food_Delivery_App_BackEnd.Controllers
{
    [Route("api/admin-auth")]
    [ApiController]
    public class AdminAuthController : ControllerBase
    {
        private IRepositoryUser repositoryUser;
        public AdminAuthController(IRepositoryUser repositoryUser)
        {
            this.repositoryUser = repositoryUser;
        }

        [HttpPost("login")]
        public IActionResult Login(RequestLogin user)
        {
            return repositoryUser.AdminLogin(user);
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] Users user)
        {
            var response = repositoryUser.PartnerRegister(user);
            return new JsonResult(response);

        }


        // POST api/<AdminAuthController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<AdminAuthController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AdminAuthController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
