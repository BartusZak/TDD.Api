﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace TDD.Tests
{
    public class UnitTest1
    {

        [Fact]
        public void ShouldLoginWithSuccess()
        {
            var user = new User { Username = "login", Email = "mail@mail.com", Password = GetHash("asd") };

            var loginModel = new LoginModel
            {
                Username = "login",
                Password = "asd"
            };
            var userRepository = new Mock<IRepository<User>>();
            userRepository.Setup(x => x.Exist(It.IsAny<Func<User, bool>>())).Returns(true);
            userRepository.Setup(x => x.GetBy(It.IsAny<Func<User, bool>>())).Returns(user);

            var userService = new UserService(userRepository.Object);

            var accountController = new AccountController(userService);


            var result = accountController.Login(loginModel);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var okResultValue = Assert.IsAssignableFrom<ResultDto<LoginResultDto>>(okResult.Value);

            Assert.Equal(user.Email, okResultValue.SuccessResult.Email);
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

        [Fact]
        public void ShouldReturnErrorOnInvalidUser()
        {
            var error = "Hasło luub Użytkownik błędne";
            //user w bazie
            var user = new User { Username = "login", Email = "mail@mail.com", Password = GetHash("asd") };

            var loginModel = new LoginModel
            {
                Username = "login",
                Password = "asd"
            };
            var userRepository = new Mock<IRepository<User>>();

            var userService = new UserService(userRepository.Object);

            var accountController = new AccountController(userService);


            var result = accountController.Login(loginModel);
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var errorResult = Assert.IsAssignableFrom<ResultDto<LoginResultDto>>(badRequest.Value);

            Assert.Contains(error, errorResult.Errors);
        }
       

        public class User : Entity
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class LoginModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public interface IRepository<T> where T : Entity
        {
            bool Exist(Func<User, bool> function);
            T GetBy(Func<User, bool> function);
        }

        public interface IUserService
        {
            ResultDto<LoginResultDto> Login(LoginModel loginModel);
        }

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

                if (!isUserExist)
                {
                    result.Errors.Add("Hasło luub Użytkownik błędne");
                    return result;
                }

                var user = _userRepository.GetBy(x => x.Username == loginModel.Username);

                if (user.Password == GetHash(loginModel.Password))
                {
                    result.SuccessResult = new LoginResultDto
                    {
                        Email = user.Email
                    };
                    return result;
                }

                return result;
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

        public class LoginResultDto : BaseDto
        {
            public string Email { get; set; }
        }

        public class ResultDto<T> where T : BaseDto
        {
            public T SuccessResult { get; set; }
            public List<string> Errors { get; set; }
            public bool IsError { get { return Errors?.Count > 0; } }
        }

        public class BaseDto
        {

        }

        public class Entity
        {
            public long Id { get; set; }
        }

        public class AccountController : Controller
        {
            private readonly IUserService _userService;

            public AccountController(IUserService userService)
            {
                _userService = userService;
            }

            public IActionResult Login(LoginModel loginModel)
            {
                var result = _userService.Login(loginModel);

                if(result.IsError)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
        }

    }
}
