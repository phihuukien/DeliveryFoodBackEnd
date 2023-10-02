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
        public ReviewsController(IRepositoryReviews repositoryReviews)
        {
            this.repositoryReviews = repositoryReviews;
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

        [HttpGet]
        [Route("rating/{restaurantId}")]
        public IActionResult GetRating(string restaurantId)
        {
            return repositoryReviews.GetRating(restaurantId);
        }

        [HttpGet]
        [Route("getAll/{restaurantId}")]
        public IActionResult GetReviewByRestaurant(string restaurantId, string? sort_order, string? sort_orderBy, int page, string? from_date, string? to_date)
        {
            return repositoryReviews.GetReviewByRestaurant(restaurantId, sort_order, sort_orderBy, page, from_date, to_date);
        }

        [HttpDelete]
        [Route("delete/{reviewId}")]
        public IActionResult DeleteReview(string reviewId)
        {
            return repositoryReviews.DeleteReview(reviewId);
        }
    }
}
