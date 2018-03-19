using System;
using System.Collections.Generic;
using System.Text;
using TDD.Dto;
using TDD.Model;

namespace TDD.Interface
{
    public interface IUserService
    {
        ResultDto<LoginResultDto> Login(LoginModel loginModel);
    }
}
