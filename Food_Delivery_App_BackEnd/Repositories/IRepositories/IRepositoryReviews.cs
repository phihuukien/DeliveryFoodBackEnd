using Food_Delivery_App_BackEnd.ModelDTO;
using Food_Delivery_App_BackEnd.ModelDTO.Request;
using Microsoft.AspNetCore.Mvc;
namespace Food_Delivery_App_BackEnd.Repositories.IRepositories
{
    public interface IRepositoryReviews
    {
        public IActionResult GetReview(string foodId);
        public IActionResult AddReview(RequestReview requestReview);
        public IActionResult EditReview(RequestReview requestReview);
        public IActionResult GetOrderDetail(string orderId);
        public IActionResult GetRating(string restaurantId);
        public IActionResult GetReviewByRestaurant(string restaurantId, string? sort_order, string? sort_orderBy, int page, string? from_date, string? to_date);
        public IActionResult DeleteReview(string reviewId);
        public IActionResult GetOrderToReview(string username);
    }
}
