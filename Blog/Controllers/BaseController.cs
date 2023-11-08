using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Blog.Controllers
{
    public class BaseController : Controller
    {
        protected int getLoggedInUserId()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
            if (claims == null) return 0;
            return Convert.ToInt32(claims?.Value);
        }
    }
}
