using Blog.Entities;
using Blog.Services.Dto;
using Blog.Services.Repository.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Blog.Helpers;

namespace Blog.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository; 
        public AccountController(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }
        [Route("login")]
        [HttpGet]
        public IActionResult login()
        {
            return View();
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> login(LoginDto loginDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await ValidateUser(loginDto.Username, loginDto.Password);
                    if (result==null)
                    {
                        throw new Exception("Username and Password didnot matched.");
                    }
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier,result.Id.ToString())
                    };
                    var userIdentity = new ClaimsIdentity(claims, "local");

                    ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                    AuthenticationProperties prop = new AuthenticationProperties();
                    prop.ExpiresUtc = DateTime.UtcNow.AddDays(1);
                    prop.IsPersistent = true;
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, prop);
                    return Redirect("/");
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
                    var result = await IsDuplicate(userDto.Username, userDto.Email);
                    if(result == null)
                    {
                        throw new Exception("Please choose different username and email.");
                    }
                    var user = new User
                    {
                        Email = userDto.Email,
                        Password = userDto.Password,
                        ProfilePicture = userDto.ProfilePicture,
                        Username = userDto.Username,
                    };
                    var add= await _userRepository.AddAsync(user);
                    return Redirect("/login");
                }
            }
            catch (Exception ex)
            {
                AlertHelper.setMessage(this, ex.Message, messageType.error);
            }
            return View();
        }

        async Task<User> IsDuplicate(string username, string email)
        {
            var user= await _userRepository.GetByUsernameAsync(username);
            if(user.Username==username || user.Email == email)
            {
                return null;
            }
            return user;
        }

        async Task<User> ValidateUser(string username, string password)
        {
            var user= await _userRepository.GetByUsernameAsync(username);
            if(user == null)
            {
                return null;
            }
            if (!(password == user.Password))
            {
                return null;
            }
            return user;
        }
    }
}
