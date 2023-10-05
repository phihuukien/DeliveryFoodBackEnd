namespace Food_Delivery_App_BackEnd.ModelDTO.Request
{
    public class RequestUpdateCategory
    {
        public string Id { get; set; } = String.Empty;
        public string RestaurantId { get; set; } = String.Empty;
        public string Name { get; set; } = String.Empty;
        public int Status { get; set; }
    }
}
