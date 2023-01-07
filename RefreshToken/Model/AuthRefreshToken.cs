namespace RefreshToken.Model
{
    public class AuthRefreshToken
    {
        public string Token { get; set; }
        public DateTime Created { get; set; }
        public DateTime Expires { get; set; }
    }
}
