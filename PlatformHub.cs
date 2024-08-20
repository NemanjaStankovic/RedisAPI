using Microsoft.AspNetCore.SignalR;
namespace SignalRChat.Hubs;

public class PlatformHub : Hub
{
    public async Task SendMessage(string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", message);
    }
}