using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace chat_email_system_web_project.Hubs
{
    public class ChatHub : Hub
    {
        private static ConcurrentDictionary<string, string> connectedUsers = new ConcurrentDictionary<string, string>();

        public async Task SendMessage(string userName, string msg, string targetUser = "All")
        {
            if (targetUser == "All")
            {
                await Clients.All.SendAsync("ReceiveMessage", userName, msg);
            }
            else
            {
                string targetConnectionId = connectedUsers.FirstOrDefault(u => u.Value == targetUser).Key;
                if (!string.IsNullOrEmpty(targetConnectionId))
                {
                    await Clients.Client(targetConnectionId).SendAsync("ReceivePrivateMessage", userName, msg);
                }
            }
        }

        public async Task JoinChat(string userName)
        {
            string connectionId = Context.ConnectionId;
            connectedUsers[connectionId] = userName;

            await Clients.All.SendAsync("UpdateUserList", connectedUsers.Values);
        }

        public override async Task OnDisconnectedAsync(System.Exception exception)
        {
            if (connectedUsers.TryRemove(Context.ConnectionId, out string removedUser))
            {
                await Clients.All.SendAsync("UpdateUserList", connectedUsers.Values);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
