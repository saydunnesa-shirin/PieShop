using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace PieShop.Core.SignalR
{
    [Authorize(Policy = "CustomHubAuthorizatioPolicy")]
    public class NotificationHub : Hub<INotificationHub>
    {
        [HubMethodName("SendMsg")]
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.ReceiveMessage(user, message);
        }

        public Task SendMessageToCaller(string user, string message)
        {
            return Clients.Caller.ReceiveMessage(user, message);
        }

        public Task SendMessageToGroup(string user, string message)
        {
            return Clients.Group("SignalR Users").ReceiveMessage(user, message);
        }

        [HubMethodName("SendMessageToUser")]
        public Task DirectMessage(string user, string message)
        {
            return Clients.All.ReceiveMessage(user, message);
            //return Clients.User(user).ReceiveMessage(user, message);
        }
        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnDisconnectedAsync(exception);
        }
    }

    public interface INotificationHub
    {
        Task ReceiveMessage(string user, string message); //Angular
        Task ReceiveMessage2(string user, string message); //Android
    }
}
