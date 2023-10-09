namespace Food_Delivery_App_BackEnd.ModelDTO.Response
{
    public class ResponseRestaurantReviews
    {
        public string foodName { get; set; }
        public string username { get; set; }
        public int Rate { get; set; }
        public DateTime Date { get; set; }
        public string context { get; set; }
        public List<string> ReviewImg { get; set; }
        public string ReviewId { get; set; }
    }
}
