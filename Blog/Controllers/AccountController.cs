using Blog.Entities;
using Blog.Services.Dto;
using Blog.Services.Repository.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Blog.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Blog.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUserRepository _userRepository;
        public AccountController(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        [Authorize]
        [Route("changepassword")]
        [HttpGet]
        public async Task<IActionResult> changepassword()
        {
            return View();
        }

        [Authorize]
        [Route("changepassword")]
        [HttpPost]
        public async Task<IActionResult> changepassword(ChangePasswordDto changePasswordDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int userId = getLoggedInUserId();
                    var user = await _userRepository.GetByIdAsync(userId);
                    if (changePasswordDto.OldPassword == user.Password)
                    {
                        user.Password = changePasswordDto.NewPassword;
                        var result= await _userRepository.UpdateAsync(user);
                        AlertHelper.setMessage(this, "Password changed Succssfulley!", messageType.success);
                        return Redirect("/changepassword");
                    }
                }
            }
            catch (Exception)
            {
                AlertHelper.setMessage(this, "Error occured", messageType.error);
            }
            AlertHelper.setMessage(this, "Model is not valid!!", messageType.error);
            return View();
        }

        [Route("login")]
        [HttpGet]
        public async Task<IActionResult> login(string? ReturnUrl = null)
        {
            ViewData["ReturnUrl"] = ReturnUrl;
            return View();
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> login(LoginDto loginDto, string? ReturnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await ValidateUser(loginDto.Username, loginDto.Password);
                    if (result == null)
                    {
                        throw new Exception("Username and Password didnot matched.");
                    }
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier,result.Id.ToString()),
                        new Claim("username",result.Username.ToString()),

                    };
                    var userIdentity = new ClaimsIdentity(claims, "local");

                    ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                    AuthenticationProperties prop = new AuthenticationProperties();
                    prop.ExpiresUtc = DateTime.UtcNow.AddDays(1);
                    prop.IsPersistent = true;
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, prop);
                    if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            catch (Exception ex)
            {
                AlertHelper.setMessage(this, ex.Message, messageType.error);
            }
            return View();
        }


        [HttpGet]
        [Route("logout")]
        public async Task<IActionResult> logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/login");
        }

        [Route("register")]
        [HttpGet]
        public IActionResult register()
        {
            return View();
        }
        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> register(UserDto userDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var duplicateUser = await IsDuplicateUsername(userDto.Username);
                    if (duplicateUser != null)
                    {
                        AlertHelper.setMessage(this, "Please choose different username.", messageType.error);
                        return View();
                    }
                    var duplicateEmail = await IsDuplicateEmail(userDto.Email);
                    if (duplicateEmail != null)
                    {
                        AlertHelper.setMessage(this, "Please choose different email.", messageType.error);
                        return View();
                    }
                    //var hashedpassword = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
                    var user = new User
                    {
                        Email = userDto.Email,
                        Password = userDto.Password,
                        ProfilePicture = userDto.ProfilePicture,
                        Username = userDto.Username,
                    };
                    var add = await _userRepository.AddAsync(user);
                    AlertHelper.setMessage(this, "User Registered!!", messageType.success);
                    return Redirect("/login");
                }
            }
            catch (Exception ex)
            {
                AlertHelper.setMessage(this, ex.Message, messageType.error);
            }
            AlertHelper.setMessage(this, "Error Occured!!", messageType.error);
            return View();
        }

        async Task<User> IsDuplicateUsername(string username)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null) return null;
            return user;
        }

        async Task<User> IsDuplicateEmail(string email)
        {
            var sameEmailUser = await _userRepository.GetByEmailAsync(email);
            if (sameEmailUser == null) return null;
            return sameEmailUser;
        }

        async Task<User> ValidateUser(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null)
            {
                return null;
            }
            //if(!BCrypt.Net.BCrypt.Verify(password, user.Password)){
            //    return null;
            //}
            if (!(password == user.Password))
            {
                return null;
            }
            return user;
        }
    }
}
