using Microsoft.AspNetCore.SignalR;
using Realtime.Entity;

namespace Realtime.Service
{
    public class ChatHub : Hub
    {
        private readonly AppdbContext _dbContext;
        public ChatHub( )
        {
            this._dbContext = new AppdbContext();
        }
        public async Task SendMessage(Message messageModel)
        {
            var message = new Message
            {
                Content = messageModel.Content,
                Timestamp = DateTime.UtcNow,
                SenderUserId = messageModel.SenderUserId,
                ReceiverUserId = messageModel.ReceiverUserId
            };

            _dbContext.messages.Add(message);
            await _dbContext.SaveChangesAsync();

        }
    }
}
