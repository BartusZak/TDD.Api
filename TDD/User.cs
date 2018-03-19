using System;
using System.Collections.Generic;
using System.Text;

namespace TDD
{
    public class User : Entity
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
