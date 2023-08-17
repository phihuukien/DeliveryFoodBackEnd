using Food_Delivery_App_BackEnd.ModelDTO;
using Food_Delivery_App_BackEnd.Models.BusinessModels;
using Food_Delivery_App_BackEnd.Models.DataModels;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Food_Delivery_App_BackEnd.Util
{
    public class utilities
    {
        IConfiguration config;
        FoodDeliveryAppDbContext _context;
        public utilities(FoodDeliveryAppDbContext _context, IConfiguration config)
        {
            this._context = _context;
            this.config = config;
        }
     
      
        public string CreateToken(Users user)
        {
            var key = config["Jwt:Key"];
            //mã hóa ky
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            //ký vào key đã mã hóa
            var signingCredential = new SigningCredentials(signingKey,
            SecurityAlgorithms.HmacSha256);
            //tạo claims chứa thông tin người dùng (nếu cần)
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name,user.Username),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.MobilePhone,user.Phone),
            });
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddDays(2),
                SigningCredentials = signingCredential,
                Issuer = config["Jwt:Issuer"],
                Audience = config["Jwt:Audience"],
            };
            var token = new JwtSecurityTokenHandler().CreateToken(tokenDescriptor);

            //sinh ra chuỗi token với các thông số ở trên
            var AccessToken = new JwtSecurityTokenHandler().WriteToken(token);
            //trả về kết quả cho client username và chuỗi token eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiVVNFUiAxMSIsImVtYWlsIjoiVVNFUjFAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbW9iaWxlcGhvbmUiOiIwMTMxNDE0MTEiLCJuYmYiOjE2OTExNjA1MDQsImV4cCI6MTY5MTE2MDUwNiwiaWF0IjoxNjkxMTYwNTA0LCJpc3MiOiJKV1RBdXRoZW50aWNhdGlvblNlcnZlciIsImF1ZCI6IkpXVFNlcnZpY2VDbGllbnQifQ.4CedDsZGm79iiSBDwcvI_r2AAJ6BIGFQOdrOVoDdcxU
            //eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.
            //eyJuYW1lIjoiVVNFUiAxMSIsImVtYWlsIjoiVVNFUjFAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3
            //MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbW9iaWxlcGhvbmUiOiIwMTMxNDE0MTEiLCJuYmYiOjE2OTExNjAyMTEsImV4cCI6MTY5Mz
            //IzMzgxMSwiaWF0IjoxNjkxMTYwMjExLCJpc3MiOiJKV1RBdXRoZW50aWNhdGlvblNlcnZlciIsImF1ZCI6IkpXVFNlcnZpY2VDbGllb
            //nQifQ.Lnorr6tMXQrChprzGB2n5qn_IdarzK_dZ9E2UX3086U

            return AccessToken;
        }
    }
}
