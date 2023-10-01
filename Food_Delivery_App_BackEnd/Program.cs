using Food_Delivery_App_BackEnd.Models.BusinessModels;
using Food_Delivery_App_BackEnd.Repositories.ImplRepositories;
using Food_Delivery_App_BackEnd.Repositories.IRepositories;
using Food_Delivery_App_BackEnd.Util;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSignalR();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigins",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
             .AllowAnyHeader()
             .AllowAnyMethod()
             .AllowCredentials();
        });
});

builder.Services.AddScoped<FoodDeliveryAppDbContext>();

builder.Services.AddScoped<utilities>();

builder.Services.AddScoped<IRepositoryUser, RepositoryUser>();
builder.Services.AddScoped<IRepositoryCarts, RepositoryCarts>();
builder.Services.AddScoped<IRepositoryRestaurants, RepositoryRestaurants>();
builder.Services.AddScoped<IRepositoryFoods, RepositoryFoods>();
builder.Services.AddScoped<IRepositoryBookMarks, RepositoryBookMarks>();
builder.Services.AddScoped<IRepositoryOrders, RepositoryOrders>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
           .AddJwtBearer(options =>
           {
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   //có kiểm tra Issuer (default true)
                   ValidateIssuer = true,
                   ValidIssuer = builder.Configuration["Jwt:Issuer"],
                   //có kiểm tra Audience (default true)
                   ValidateAudience = true,
                   ValidAudience = builder.Configuration["Jwt:Audience"],

                   //Đảm bảo phải có thời gian hết hạn trong token
                   RequireExpirationTime = true,
                   ValidateLifetime = true,
                   //Chỉ ra key sử dụng trong token
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                   RequireSignedTokens = true

               };
           });



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowOrigins");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();
app.MapHub<ChatHub>("/chatHub");

app.Run();
