using Microsoft.AspNetCore.DataProtection.KeyManagement;
using RefreshToken.Dto;
using RefreshToken.Model;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;
using RefreshToken.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace RefreshToken.Services.AuthServices
{
    public class AuthServices : IAuthServices
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public AuthServices(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ResponseApi> Login(UserDto request)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.UserName == request.UserName);

            if (user == null)
            {
                return new ResponseApi { Message = "User Not Found" };
            }

            //check if password is incorrect
            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return new ResponseApi { Message = "Wrong Password" };
            }

            string token = GenerateToken(user);
            return new ResponseApi { Success = true, Token = token };
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

        private string GenerateToken (User user) 
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.IdUser.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:TokenSecretKey").Value));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var jwtToken = new JwtSecurityToken
                            (
                                claims: claims, 
                                expires: DateTime.Now.AddDays(1), 
                                signingCredentials: credential
                            );

            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return token;

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
