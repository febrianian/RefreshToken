using System.Security.Cryptography;

namespace RefreshToken.Services
{
    public class ResponseApi
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
