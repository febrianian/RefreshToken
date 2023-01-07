using Microsoft.AspNetCore.DataProtection.KeyManagement;
using RefreshToken.Dto;
using RefreshToken.Model;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;
using RefreshToken.Data;

namespace RefreshToken.Services.AuthServices
{
    public class AuthServices : IAuthServices
    {
        private readonly DataContext _context;

        public AuthServices(DataContext context)
        {
            _context = context;

        }
        public async Task<User> RegisterUser(UserDto request)
        {
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User 
            { 
                UserName = request.UserName, 
                PasswordHash = passwordHash, 
                PasswordSalt = passwordSalt 
            };

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            //using (var hmac = new HMACSHA512(new byte[32]))
            using (var hmac = new HMACSHA512())
            {

                //here with the create password hash method
                //we generate a salt password
                //now this is as it states here, a key that is used in the HMac calculation
                //and when you combine the salt with the password, you get a different password hash
                //although you use the same password.

                //now to better see the whole process here i would say we just test this

                //lets use the create password hash method already

                //and then you will see when we use the password papper several times
                //we will get a different password hash value

                //that's because of the salt, but if he would always use the same salt,the password hash would be the same
                //and the problem with that is that someday, very smart people will crack the cryptography algorithms and then
                //they can see a password hash and from the password hash, they can get to the plane text password but this is not done with a salt.

                //meaning if they don't know the salt, they are not able to get the password, the real password

                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }
    }
}
