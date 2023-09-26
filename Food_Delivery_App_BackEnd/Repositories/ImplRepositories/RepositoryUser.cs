using Food_Delivery_App_BackEnd.ModelDTO;
using Food_Delivery_App_BackEnd.Models.BusinessModels;
using Food_Delivery_App_BackEnd.Models.DataModels;
using Food_Delivery_App_BackEnd.Repositories.IRepositories;
using Food_Delivery_App_BackEnd.Util;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Food_Delivery_App_BackEnd.Repositories.ImplRepositories
{
    public class RepositoryUser : IRepositoryUser
    {
        FoodDeliveryAppDbContext _context;
        utilities utilities;
        public RepositoryUser(FoodDeliveryAppDbContext _context, utilities utilities)
        {
            this._context = _context;
            this.utilities = utilities;
        }

        public IActionResult AdminLogin(RequestLogin user)
        {
            try
            {
                if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
                {
                    return new JsonResult(new { status = false, Message = "Please fill up all the fields" });
                }
                else
                {
                    var userExist = _context.Users.Find(x => x.Username == user.Username).FirstOrDefault();
                    if (userExist != null && (userExist.Role == "ADMIN" || userExist.Role == "PARTNER") )
                    {
                        if (BCrypt.Net.BCrypt.Verify(user.Password, userExist.Password))
                        {

                            var token = utilities.CreateToken(userExist);
                            return new JsonResult(new { status = true, Message = "User login successful", accessToken = token, });
                        }
                        else
                        {
                            return new JsonResult(new { status = false, Message = "Incorrect username or password" });
                        }
                    }
                    else
                    {
                        return new JsonResult(new { status = false, Message = "Incorrect username or password" });
                    }

                }

            }
            catch (Exception)
            {

                return new JsonResult(new { status = false, Message = "User login failed" });

            }
        }

        public IActionResult CheckUserExist(string type, string value)
        {
            string message = "This user is not taken";
            bool status = true;
            if (type == "username")
            {
                var usernameExist = _context.Users.Find(x => x.Username == value).FirstOrDefault();
                if (usernameExist != null)
                {
                    message = "This username is taken";
                    status = false;
                }
            }

            if (type == "email")
            {
                var emailExist = _context.Users.Find(x => x.Email == value).FirstOrDefault();
                if (emailExist != null)
                {
                    message = "This email is taken";
                    status = false;
                }
            }


            return new JsonResult(new { Status = status, Message = message });
        }

        public List<Users> GetAll()
        {
            return _context.Users.Find(User => true).ToList();
        }


        public JsonResult GetUserByUsername(string username)
        {

            try
            {
                var user = _context.Users.Find(x => x.Username == username).FirstOrDefault();
                if (user !=  null)
                {
                    return new JsonResult(new { Status = true, Message = "User found successfully",Data = user });
                }
                else
                {
                    return new JsonResult(new { Status = false, Message = "No user found" });
                }
                
            }
            catch (Exception Ex)
            {
                 return new JsonResult(new { Status = false, Message = "User finding failed" , Error = "User finding failed: "+ Ex.Message });
            }

        }

        public IActionResult Login(RequestLogin user)
        {


            try
            {
                if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
                {
                    return new JsonResult(new { status = false, Message = "Please fill up all the fields" });
                }
                else
                {
                    var userExist = _context.Users.Find(x => x.Username == user.Username).FirstOrDefault();
                    if (userExist != null)
                    {
                        if (BCrypt.Net.BCrypt.Verify(user.Password, userExist.Password))
                        {

                            var token = utilities.CreateToken(userExist);
                            return new JsonResult(new { status = true, Message = "User login successful", accessToken = token });
                        }
                        else
                        {
                            return new JsonResult(new { status = false, Message = "Incorrect password" });
                        }
                    }
                    else
                    {
                        return new JsonResult(new { status = false, Message = "No user found" });
                    }

                }

            }
            catch (Exception)
            {

                return new JsonResult(new { status = false, Message = "User login failed" });

            }
        }

        public IActionResult PartnerRegister(Users user)
        {
            try
            {
                if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password) || string.IsNullOrEmpty(user.Email))
                {
                    return new JsonResult(new { status = false, Message = "Please fill up all the fields" });
                }
                else 
                {
                    var userExist = _context.Users.Find(x => x.Username == user.Username || x.Email == user.Email).FirstOrDefault();
                    if (userExist == null)
                    {
                        string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
                        user.Password = passwordHash;
                        user.Role = "PARTNER";
                        _context.Users.InsertOne(user);
                        return new JsonResult(new { status = true, Message = "User registered successfully" });
                    }
                    return new JsonResult(new { status = false, Message = "User already exist" });

                }


            }
            catch (Exception ex)
            {
                return new JsonResult(new { status = false, Message = ex.Message });

            }
        }

        public IActionResult RefreshToken(string token,string username)
        {
        
            JwtSecurityToken jwtSecurityToken;
            try
            {
                jwtSecurityToken = new JwtSecurityToken(token);
                var expired = jwtSecurityToken.ValidTo > DateTime.UtcNow;

                if (!expired)
                {
                    var user = _context.Users.Find(x => x.Username == username).FirstOrDefault();
                    var newAccessToken = utilities.CreateToken(user);
                    return new JsonResult(new { accessToken = newAccessToken, status = true, Message= "Token refresh successful" });
                }
                return new JsonResult(new { Message = "Invalid Token", status = false });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { token = ex.Message });
            }

         
        }


      
        public IActionResult Register(Users user)
        {
            try
            {
                if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password) || string.IsNullOrEmpty(user.Email))
                {
                    return new JsonResult(new { status = false, Message = "Please fill up all the fields" });
                }
                else
                {
                    string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
                    user.Password = passwordHash;
                    user.Role = "USER";
                    _context.Users.InsertOne(user);
                    return new JsonResult(new { status = true, Message = "User registered successfully" });
                }


            }
            catch (Exception ex)
            {
                var existCode = ex.Message.Contains("Code : 11000");
                var existUsername = ex.Message.Contains("username_1 dup key");
                string message = "User registered failed";
                if ((existCode && existUsername) == true)
                {
                    message = "Username already exist";
                }
                return new JsonResult(new { status = false, Message = message });

            }

        }
    }
}
