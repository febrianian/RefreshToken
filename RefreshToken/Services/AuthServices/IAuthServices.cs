using RefreshToken.Dto;
using RefreshToken.Model;

namespace RefreshToken.Services.AuthServices
{
    public interface IAuthServices
    {
        Task<User> RegisterUser(UserDto request);
        Task<ResponseApi> Login(UserDto request);
        Task<ResponseApi> AuthRefreshToken();
    }
}
