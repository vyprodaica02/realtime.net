using Realtime.Entity;
using Realtime.Enum;
using System;

namespace Realtime.IService
{
    public interface IUser
    {
        Task< ErrorHelper> createUser(User user);
        Task<ErrorHelper> SuaUser(User user);
        Task<ErrorHelper> XoaUser(int id);
        IQueryable<User> getAll();
    }
}
