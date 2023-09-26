using Food_Delivery_App_BackEnd.Models.DataModels;

namespace Food_Delivery_App_BackEnd.ModelDTO.Response
{
    public class ResponseDashboard
    {
        public int? TotalOrderToday { get; set; }
        public string? Message { get; set; }
        public bool Status { get; set; }
        public int? Earnings { get; set; }
        public int? OrderFinish { get; set; }

        public int OrderCancel { get; set; }

        public int FoodSoldOut { get; set; }

        public List<Orders>? OrderPending { get; set; }
    }
}
