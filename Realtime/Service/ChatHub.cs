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

        public async Task<List<Message>> GetMessages(int userSenID, int userResenId)
        {
            using (var trans = _dbContext.Database.BeginTransaction())
            {
                if (userSenID == 0 || userResenId == 0)
                {
                    return null;
                }

                var messages = _dbContext.messages
                    .Where(x => (x.SenderUserId == userSenID && x.ReceiverUserId == userResenId) ||
                                (x.SenderUserId == userResenId && x.ReceiverUserId == userSenID))
                    .OrderByDescending(x => x.Timestamp)
                    .ToList();

                return messages;
            }
        }


    }
}
