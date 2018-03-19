using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TDD.Dto;
using TDD.Interface;
using TDD.Model;
using TDD.Service;
using Xunit;

namespace TDD.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void ShouldReturnOnInvalidPassword()
        {
            var error = "Haslo lub uzytkownik jest bledne ";
            //user w bazie
            var user = new User { Username = "login", Email = "mail@mail.com", Password = GetHash("asdsdasd") };
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

            //Assert.Equal(user.Email, okResultValue.SuccessResult.Email);
            Assert.NotNull(okResultValue.SuccessResult?.Token);
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
            var error = "Haslo lub uzytkownik jest bledne ";
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


        

        

        

        

        

        

        

        

        

        

    }
}