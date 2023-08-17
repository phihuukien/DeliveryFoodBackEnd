using Food_Delivery_App_BackEnd.Models.DataModels;
using MongoDB.Driver;

namespace Food_Delivery_App_BackEnd.Models.BusinessModels
{
    public class FoodDeliveryAppDbContext
    {
        IConfiguration Configuration;
        public FoodDeliveryAppDbContext(IConfiguration Configuration)
        {
            this.Configuration = Configuration;
        }
        public IMongoDatabase Connection
        {
            get
            {
                var client = new MongoClient(Configuration.GetConnectionString("MongoConnection"));
                var database = client.GetDatabase(Configuration.GetConnectionString("database"));
                return database;
            }
        }
        public IMongoCollection<Users> Users => Connection.GetCollection<Users>("users");
        public IMongoCollection<Restaurants> Restaurants => Connection.GetCollection<Restaurants>("restaurants");
        public IMongoCollection<Foods> Foods => Connection.GetCollection<Foods>("foods");
        public IMongoCollection<BookMarks> BookMarks => Connection.GetCollection<BookMarks>("bookmarks");
        public IMongoCollection<Cart> Cart => Connection.GetCollection<Cart>("carts");
        public IMongoCollection<Orders> Orders => Connection.GetCollection<Orders>("orders");
        public IMongoCollection<OrderDetails> OrderDetails => Connection.GetCollection<OrderDetails>("orderdetails");

    }
}
