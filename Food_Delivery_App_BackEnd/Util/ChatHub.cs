using Microsoft.AspNetCore.SignalR;

namespace Food_Delivery_App_BackEnd.Util
{
    public class ChatHub : Hub
    {
        public async Task SendMessageToWeb(string message)
        {
            await Clients.All.SendAsync("ReceiveMessageFromMobile", message);
        }
        public async Task SendMessageToMobile(string message)
        {
            await Clients.All.SendAsync("ReceiveMessageFromWeb", message);
        }

        public async Task SendStuasToMobileForShipper(string message)
        {
            await Clients.All.SendAsync("SendStutusToMobileForShipper", message);
        }

    }
}
