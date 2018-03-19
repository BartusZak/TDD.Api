using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using TDD.Dto;
using TDD.Interface;
using TDD.Model;

namespace TDD.Service
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
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

            var token = GetToken(user, SecretKey???, issuer???, DateTime.Now);


            result.SuccessResult = new LoginResultDto
            {
                Email = user.Email
            };



            return result;
        }

        private object GetToken(User user, object p1, object p2, DateTime now)
        {
            throw new NotImplementedException();
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
