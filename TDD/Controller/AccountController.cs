using System;
using System.Collections.Generic;
using System.Text;
using TDD.Interface;
using TDD.Model;
using Microsoft.AspNetCore.Mvc;

namespace TDD
{
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

            if (result.IsError)
            {
                return BadRequest(result);
            }


            return Ok(result);
        }

        public IActionResult Register(RegisterModel registerModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _userService.Register(registerModel);

            if (result.IsError)
            {
                return BadRequest(result);
            }


            return Ok(result);
        }
    }
}
