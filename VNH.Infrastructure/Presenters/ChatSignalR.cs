using Microsoft.AspNetCore.SignalR;
using VNH.Application.DTOs.Catalog.Forum.Answer;
using VNH.Application.DTOs.Catalog.Posts;

namespace VNH.Infrastructure.Presenters
{
    public class ChatSignalR : Hub
    {
        public async Task SendComment(CommentPostDto comment)
        {
            await Clients.All.SendAsync("ReceiveComment", comment);
        }

        public async Task SendAnswer(AnswerQuestionDto comment)
        {
            await Clients.All.SendAsync("ReceiveAnswer", comment);
        }

        public async Task AddToGroup(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }
        public override async Task OnConnectedAsync()
        {
            Console.WriteLine("Client connected: " + Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine("Client disconnected: " + Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }


    }
}
