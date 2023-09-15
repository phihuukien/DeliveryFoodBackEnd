using Food_Delivery_App_BackEnd.ModelDTO;
using Food_Delivery_App_BackEnd.Models.BusinessModels;
using Food_Delivery_App_BackEnd.Models.DataModels;
using Food_Delivery_App_BackEnd.Repositories.IRepositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Food_Delivery_App_BackEnd.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IRepositoryUser repositoryUser;
        private static IWebHostEnvironment _webHostEnvironment;
       
        public UserController(IRepositoryUser repositoryUser, IWebHostEnvironment webHostEnvironment)
        {
            this.repositoryUser = repositoryUser;
            _webHostEnvironment = webHostEnvironment;
           
        }

        // GET: api/<UserController>
        [HttpGet]
        public IEnumerable<Users> Get()
        {
            return repositoryUser.GetAll().AsEnumerable();
        }

        [HttpGet]
        [Route("refreshToken")]
        public async Task<IActionResult> RefreshToken( )
        {
     
            if (HttpContext.Request.Headers.TryGetValue("Authorization", out var headerAuth))
            {
                var jwtToken = headerAuth.First().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1];
                var jwt = new JwtSecurityTokenHandler().ReadJwtToken(jwtToken);
                string username = jwt.Claims.First(c => c.Type == "name").Value;
                return repositoryUser.RefreshToken(jwtToken, username);
            }
            return new JsonResult(new { status = false, Message = "jwt not found" });
        }


        [HttpGet]
        [Route("get-user")]
        [Authorize]
        public IActionResult GetUserByUsername()
        {
            var accessToken = HttpContext.GetTokenAsync("access_token").Result;
            var token = accessToken?.ToString();
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(accessToken?.ToString());
            string username = jwt.Claims.First(c => c.Type == "name").Value;
            return repositoryUser.GetUserByUsername(username);

        }

        // POST api/<UserController>
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] Users user)
        {
                var response = repositoryUser.Register(user);
                return new JsonResult(response);
                
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] RequestLogin user)
        {
            var response = repositoryUser.Login(user);
            return new JsonResult(response);

        }

        [HttpGet("exist/{type}={value}")]
        public IActionResult CheckUserExist(string type, string value)
        {
            var response = repositoryUser.CheckUserExist(type, value);
            return new JsonResult(response);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> Upload([FromForm] UploadFile obj)
        { 

            return await repositoryUser.Upload(obj);

        }

      
    }
}
