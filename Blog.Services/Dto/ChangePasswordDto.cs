using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.Dto
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "Password is required.")]
        public string OldPassword { get; set; }


        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string NewPassword { get; set; }


        [Compare("NewPassword", ErrorMessage = "Repeated password do not match.")]
        public string ConfirmPassword { get; set; }

    }
}
