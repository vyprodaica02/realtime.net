using Realtime.Enum;

namespace Realtime.IService
{
    public interface ILogin
    {
        Task<ErrorHelper> LoginAsync(string userName,string password);
    }
}
