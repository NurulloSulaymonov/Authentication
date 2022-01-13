using Auth.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Auth.Controllers
{
    public class AccountController : Controller
    {
        List<User> users;
        public AccountController(IHttpContextAccessor httpContextAccessor)
        {
            users = new List<User>
            {
            new Models.User
            {
                Password = "khayriddin",
                Username="khayriddin@gmail.com",
                Role= "Admin"
            },
            new Models.User
            {
                Password = "nurullo",
                Username="nurullo@gmail.com",
                Role = "Manager"
            }
        };
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {

            return View(new UserViewModel());
        }

        [HttpPost]
        public IActionResult Login(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = users.FirstOrDefault(x => x.Password == model.Password && x.Username == model.Username);
                if (user == null) return View(model);

                 // claim is the information about a user 
                 //example Name, PhoneNumber, Email, Role 
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Username),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var driverClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Username),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                ClaimsIdentity driverIdentity = new ClaimsIdentity(claims, "Cookies");
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(driverClaims, "Cookies");

                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(new [] { claimsIdentity,driverIdentity });
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
                return RedirectToAction("Index", "Home");
            
            }

            return View(model);


        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public async Task<IActionResult> LogOutAsync()
        {
            await HttpContext.SignOutAsync();
           return RedirectToAction("Index", "Home");
        }
    }
}
