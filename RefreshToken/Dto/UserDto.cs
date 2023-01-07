namespace RefreshToken.Dto
{
    public class UserDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RefreshToken { get; set; }
        public DateTime TokenExpires { get; set; }
    }
}
