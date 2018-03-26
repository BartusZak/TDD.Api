using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TDD.Model
{
    public class RegisterModel
    {
        public RegisterModel(string username, string password, string confirmpassword)
        {
            this.Username = username;
            this.Password = password;
            this.ConfirmPassword = confirmpassword;
        }

        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
