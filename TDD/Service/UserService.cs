using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TDD.Dto;
using TDD.Interface;
using TDD.Model;

namespace TDD.Service
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IConfigurationManager _configuration;

        public UserService(IRepository<User> userRepository, IConfigurationManager configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }
        public ResultDto<LoginResultDto> Login(LoginModel loginModel)
        {
            var result = new ResultDto<LoginResultDto>
            {
                Errors = new List<string>()
            };

            var isUserExist = _userRepository.Exist(x => x.Username == loginModel.Username);




            var user = _userRepository.GetBy(x => x.Username == loginModel.Username);

            if (user?.Password != GetHash(loginModel.Password))
            {
                result.Errors.Add("Haslo lub uzytkownik jest bledne ");
                return result;
            }

            var token = BuildToken(user, _configuration.GetValue("Jwt:Key"), _configuration.GetValue("Issuer"));

            //var token = GetToken(user, SecretKey???, issuer???, DateTime.Now);


            result.SuccessResult = new LoginResultDto
            {
                Email = user.Email
            };



            return result;
        }

        public string BuildToken(User user, string secretKey, string issuer, DateTime? expirationDate = null)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.GivenName, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
            };

            var token = new JwtSecurityToken(issuer,
                issuer,
                claims,
                expires: expirationDate,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GetHash(string text)
        {
            // SHA512 is disposable by inheritance.  
            using (var sha256 = SHA256.Create())
            {
                // Send a sample text to hash.  
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
                // Get the hashed string.  
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}
