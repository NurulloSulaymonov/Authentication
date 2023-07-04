using Auth.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Auth.Data;

namespace Auth.Controllers
{
    public class AccountController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl)
        {
            return View(new UserViewModel()
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(UserViewModel model)
        {
            if (model.Username == "alijon" && model.Password == "1234")
            {
                //fill claims  - 
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, "Alijon"),
                    new Claim(ClaimTypes.Email, "alijon@gmail.com"),
                    new Claim(ClaimTypes.Role, "Manager"),
                    new Claim(ClaimTypes.DateOfBirth,"2005.02.02"),
                    new Claim("Tax","2345654"),
                    
                };
                
                //create identity 
                var userIdentity = new ClaimsIdentity(claims,"Cookies");
               
                // create principal
                var userPrincipal = new ClaimsPrincipal(userIdentity);

                await  HttpContext.SignInAsync(
                    "Cookies",
                    userPrincipal, 
                    new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(10),
                    IsPersistent = true,
                });
                if (string.IsNullOrEmpty(model.ReturnUrl))
                {
                  return Redirect("/home/index");
                }
                else
                {
                    return Redirect(model.ReturnUrl);
                }
               
            }
        
            return View(model);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
        

        public async Task<IActionResult> LogOutAsync()
        {
            await HttpContext.SignOutAsync("Cookies");
            return RedirectToAction("Index", "Home");
        }
    }
}