using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PieShop.Core.SignalR
{
    public class NotificationHubServices<THub> : INotificationHubServices<THub> where THub : Hub
    {
        private readonly IHubContext<THub> _hubContext;

        public NotificationHubServices(IHubContext<THub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendToGroupAsync(string group, string method, string user, object notificationMessage, CancellationToken cancellationToken = default)
        {
            await _hubContext.Clients.Group(group).SendAsync(method, user, notificationMessage, cancellationToken);
        }

        public async Task SendToSingleClientAsync(string connectionId, string method, string user, object notificationMessage, CancellationToken cancellationToken = default)
        {
            await _hubContext.Clients.Client(connectionId).SendAsync(method, user, notificationMessage, cancellationToken);
        }
    }

    public interface INotificationHubServices<THub> where THub : Hub
    {
        Task SendToGroupAsync(string group, string method, string user, object notificationMessage, CancellationToken cancellationToken = default);
        Task SendToSingleClientAsync(string connectionId, string method, string user, object notificationMessage, CancellationToken cancellationToken = default);
    }
}
