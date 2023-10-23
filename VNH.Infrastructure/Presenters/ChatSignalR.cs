using Microsoft.AspNetCore.SignalR;
using VNH.Application.DTOs.Catalog.Posts;

namespace VNH.Infrastructure.Presenters
{
    public class ChatSignalR : Hub
    {
        public async Task SendComment(CommentPostDto comment)
        {
            // Broadcast the comment to all connected clients
            await Clients.All.SendAsync("ReceiveComment", comment);
        }
    }
}
