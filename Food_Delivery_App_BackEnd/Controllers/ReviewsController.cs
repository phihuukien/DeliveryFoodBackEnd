using Food_Delivery_App_BackEnd.ModelDTO;
using Food_Delivery_App_BackEnd.ModelDTO.Request;
using Food_Delivery_App_BackEnd.ModelDTO.Response;
using Food_Delivery_App_BackEnd.Models.BusinessModels;
using Food_Delivery_App_BackEnd.Models.DataModels;
using Food_Delivery_App_BackEnd.Repositories.ImplRepositories;
using Food_Delivery_App_BackEnd.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Food_Delivery_App_BackEnd.Controllers
{
    [Route("api/reviews")]
    [ApiController]
    [Authorize]
    public class ReviewsController : ControllerBase
    {
        private IRepositoryReviews repositoryReviews;
        FoodDeliveryAppDbContext _context;
        public ReviewsController(IRepositoryReviews repositoryReviews, FoodDeliveryAppDbContext context)
        {
            this.repositoryReviews = repositoryReviews;
            _context = context;
        }

        [HttpPost]
        [Route("add")]
        public IActionResult AddReview([FromForm] RequestReview requestReview)
        {

            return repositoryReviews.AddReview(requestReview);
        }

        [HttpGet]
        [Route("get/{foodId}")]
        public IActionResult GetReview(string foodId)
        {
            return repositoryReviews.GetReview(foodId);

        }
        [HttpGet]
        [Route("item/{orderId}")]
        public IActionResult GetOrderDetail(string orderId)
        {
            return repositoryReviews.GetOrderDetail(orderId);
        }
    }
}
