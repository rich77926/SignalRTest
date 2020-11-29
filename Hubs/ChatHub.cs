using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalRTest.Hubs
{
    public class ChatHub : Hub
    {
        public async Task NewMessage(long username, string message)
        {

            if (UserHandler.ConnectedIds.Count >= 2)
            {
                await Clients.Group("ttt").SendAsync("messageReceived", username, message);
              
            }
            else
            {
                await Clients.All.SendAsync("messageReceived", username, message);
            }
        }

        public override async Task OnConnectedAsync()
        {
            if (UserHandler.ConnectedIds.Count <= 2)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "ttt");
            }

            UserHandler.ConnectedIds.Add(Context.ConnectionId);
            base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            UserHandler.ConnectedIds.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
    public static class UserHandler
    {
        public static HashSet<string> ConnectedIds = new HashSet<string>();
    }

}