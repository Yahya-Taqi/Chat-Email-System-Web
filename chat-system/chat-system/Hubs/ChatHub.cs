using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace first.Hubs
{
    public class ChatHub : Hub
    {
        private static ConcurrentDictionary<string, string> connectedUsers = new ConcurrentDictionary<string, string>();

        public async Task SendMessage(string userName, string msg)
        {
            await Clients.All.SendAsync("ReceiveMessage", userName, msg);
        }

        public async Task JoinChat(string userName)
        {
            string connectionId = Context.ConnectionId;

            // Prevent duplicate users
            if (!connectedUsers.ContainsKey(connectionId))
            {
                connectedUsers[connectionId] = userName;
            }

            // Debugging log
            System.Console.WriteLine($"User Joined: {userName}, Connection ID: {connectionId}");

            // Notify all clients about updated user list
            await Clients.All.SendAsync("UpdateUserList", connectedUsers.Values);
        }

        public override async Task OnDisconnectedAsync(System.Exception exception)
        {
            if (connectedUsers.TryRemove(Context.ConnectionId, out string removedUser))
            {
                System.Console.WriteLine($"User Left: {removedUser}");
                await Clients.All.SendAsync("UpdateUserList", connectedUsers.Values);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
