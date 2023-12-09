using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Realtime.Entity;
using Realtime.Service;

namespace Realtime.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly ChatHub _chatHub;
        public ChatController(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
            _chatHub = new ChatHub();
        }

        [HttpPost("GuiTinNhan")]
        public async Task<IActionResult> SendMessage([FromBody] Message messageModel)
        {
            if (messageModel == null)
            {
                return BadRequest("Invalid message model");
            }
            _chatHub.SendMessage(messageModel);
            await _hubContext.Clients.User(messageModel.ReceiverUserId.ToString()).SendAsync("ReceiveMessage", messageModel);
            await _hubContext.Clients.All.SendAsync("SendMessage", messageModel);

            return Ok("gửi thành công");
        }
        [HttpGet("GetMessage")]
        public async Task<IActionResult> GetMessage (int senUserId,int ResenUserid)
        {
            var res = _chatHub.GetMessages(senUserId, ResenUserid);
            return Ok(res);
        }

    }
}
