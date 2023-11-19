using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Realtime.Entity;
using Realtime.IService;
using Realtime.Service;

namespace Realtime.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _user;

        public UserController()
        {
            _user = new UserService();
        }
        [HttpPost("dangky")]
        public async Task<IActionResult> DangKy(User user)
        {
            var res = await _user.createUser(user);
            if(res == Enum.ErrorHelper.ThanhCong)
            {
                return Ok("Đăng Ký Thành công");
            }
            else if(res == Enum.ErrorHelper.UserDaTonTai) 
            {
                return BadRequest("Tài Khoản Đã tồn tại");
            }
            else
            {
                return BadRequest("Thất Bại");
            }

        }
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> Update(User user)
        {
            var res = await _user.SuaUser(user);
            if(res == Enum.ErrorHelper.CapNhatThanhCong)
            {
                return Ok("Sửa Thành Công");
            }else
            {
                return BadRequest("Cập Nhật Thất Bại");
            }
        }
        [HttpGet("DanhSachUser")]
        public  IActionResult DanhSachUser()
        {
            var res =  _user.getAll();
            if(res != null)
            {
            return Ok(res);
            }
            else
            {
                return BadRequest("Dữ liệu không hợp lệ");
            }
        }
        [HttpDelete("XoaUser:id")]
        public async Task<IActionResult> XoaUser(int id)
        {
            var res = await _user.XoaUser(id);
            if(res == Enum.ErrorHelper.XoaThanhCong)
            {
                return Ok("Xóa Thành Công");
            }
            else
            {
                return BadRequest("Không tìm thấy user");
            }
        }
    }
}
