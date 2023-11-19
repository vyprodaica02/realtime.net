using Microsoft.EntityFrameworkCore;
using Realtime.Entity;
using Realtime.Enum;
using Realtime.IService;
using System;
using System.Data.Entity;
using EntityState = Microsoft.EntityFrameworkCore.EntityState;

namespace Realtime.Service
{
    public class UserService : IUser
    {
        private readonly AppdbContext dbcontext;

        public UserService()
        {
            this.dbcontext = new AppdbContext();
        }
        public async Task<ErrorHelper> createUser(User user)
        {
            using (var trans = dbcontext.Database.BeginTransaction())
            {
                try
                {
                    if (dbcontext.users.Any(x => x.Email == user.Email))
                    {
                        return ErrorHelper.UserDaTonTai;
                    }
                    else
                    {
                        // Mã hóa mật khẩu trước khi lưu vào cơ sở dữ liệu
                        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

                        await dbcontext.AddAsync(user);
                        dbcontext.SaveChanges();
                        trans.Commit();
                        return ErrorHelper.ThanhCong;
                    }
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    // Xử lý ngoại lệ nếu cần
                }
                return ErrorHelper.ThatBai;
            }
        }



        public IQueryable<User> getAll()
        {
            var query = dbcontext.users.OrderByDescending(x => x.TenUser).AsQueryable();
            return query;
        }

        public async Task<ErrorHelper> SuaUser(User user)
        {
            using (var trans = dbcontext.Database.BeginTransaction())
            {
                try
                {
                    var existingUser = await dbcontext.users.FindAsync(user.Id); // Thay thế Id bằng khóa chính của User

                    if (existingUser == null)
                    {
                        return ErrorHelper.userNameNull;
                    }

                    // Update các thông tin khác của User (nếu có)
                    existingUser.TenUser = user.TenUser;
                    existingUser.Email = user.Email;

                    // Kiểm tra xem có cập nhật mật khẩu không
                    if (!string.IsNullOrEmpty(user.Password))
                    {
                        // Mã hóa mật khẩu trước khi lưu vào cơ sở dữ liệu
                        existingUser.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                    }

                    await dbcontext.SaveChangesAsync();
                    trans.Commit();
                    return ErrorHelper.CapNhatThanhCong;
                }
                catch (Exception er)
                {
                    trans.Rollback();
                    // Thêm xử lý lỗi nếu cần
                }

                return ErrorHelper.ThatBai;
            }
        }



        public async Task<ErrorHelper> XoaUser(int id)
        {
            var userHt = dbcontext.users.FirstOrDefault(x => x.Id == id);
            if(userHt != null)
            {
                dbcontext.Remove(userHt);
                await dbcontext.SaveChangesAsync();
                return ErrorHelper.XoaThanhCong;
            }
            else
            {
                return ErrorHelper.ThatBai;
            }
        }
    }
}
