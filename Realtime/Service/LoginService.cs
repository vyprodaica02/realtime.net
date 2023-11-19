using Realtime.Enum;
using Realtime.IService;

namespace Realtime.Service
{
    public class LoginService : ILogin
    {
        private readonly AppdbContext dbcontext;

        public LoginService()
        {
            this.dbcontext = new AppdbContext();

        }
        public async Task<ErrorHelper> LoginAsync(string email, string password)
        {
            if (string.IsNullOrEmpty(email))
            {
                return ErrorHelper.userNameNull;
            }
            if (string.IsNullOrEmpty(password))
            {
                return ErrorHelper.passWordNull;
            }

            try
            {
                var userLogin =  dbcontext.users.FirstOrDefault(x => x.Email == email);

                if (userLogin != null)
                {
                    // Kiểm tra mật khẩu đã mã hóa
                    if (BCrypt.Net.BCrypt.Verify(password, userLogin.Password))
                    {
                        return ErrorHelper.loginThanhCong;
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu cần
            }

            return ErrorHelper.loginThatBai;
        }

    }
}
