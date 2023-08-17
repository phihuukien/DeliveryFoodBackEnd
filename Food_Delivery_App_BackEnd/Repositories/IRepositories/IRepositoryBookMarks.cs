using Microsoft.AspNetCore.Mvc;
namespace Food_Delivery_App_BackEnd.Repositories.IRepositories
{
    public interface IRepositoryBookMarks
    {
        public JsonResult GetBookmarks(string username);
        public JsonResult AddBookmark(string username, string restaurantId);

        public JsonResult RemoveBookmark(string username, string restaurantId);

    }
}
