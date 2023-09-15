﻿using Food_Delivery_App_BackEnd.ModelDTO;
using Food_Delivery_App_BackEnd.Models.DataModels;
using Microsoft.AspNetCore.Mvc;

namespace Food_Delivery_App_BackEnd.Repositories.IRepositories
{
    public interface IRepositoryUser
    {
        public IActionResult Register(Users user);
        public IActionResult Login(RequestLogin user);
        public List<Users> GetAll();
        public JsonResult GetUserByUsername(string username);
        public IActionResult CheckUserExist(string type, string value);
        public IActionResult RefreshToken(string token, string username);

        public Task<IActionResult> Upload(UploadFile obj);
       



    }
}
