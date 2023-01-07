using RefreshToken.Dto;
using RefreshToken.Model;

namespace RefreshToken.Services.AuthServices
{
    public class AuthServices : IAuthServices
    {
        public async Task<User> RegisterUser(UserDto request)
        {
            var user = new User { UserName = request.UserName };
            return user;
        }
    }
}
