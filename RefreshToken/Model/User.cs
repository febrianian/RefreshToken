namespace RefreshToken.Model
{
    public class User
    {
        public int IdUser { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; } = new byte[32];
        public byte[] PasswordSalt { get; set; } = new byte[32];

    }
}
