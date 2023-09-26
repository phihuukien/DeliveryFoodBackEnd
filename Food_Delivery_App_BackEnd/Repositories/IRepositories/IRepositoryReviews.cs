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
    }
}
