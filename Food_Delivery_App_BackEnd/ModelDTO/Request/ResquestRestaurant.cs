using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace Food_Delivery_App_BackEnd.Models.DataModels
{
    public class ResquestRestaurant
    {
        public string? Name { get; set; }

        public string? Type { get; set; }

        public List<string>? Tags { get; set; }

        public string? Location { get; set; }

        public string Distance { get; set; }

        public string Times { get; set; }


        public string? Username { get; set; }

        public List<string>? Categories { get; set; }

       
        public IFormFile? Logo { get; set; }
        public IFormFile? Poster { get; set; }
        public IFormFile? Cover { get; set; }

    }
}
