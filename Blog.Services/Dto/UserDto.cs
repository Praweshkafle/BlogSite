using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; } // Hashed and salted
        public string Repassword { get; set; } // Hashed and salted

        public string ProfilePicture { get; set; }
    }
}
