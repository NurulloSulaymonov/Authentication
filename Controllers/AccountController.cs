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
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
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
            if (ModelState.IsValid == false) return View(model);
            
            var existing = await _userManager.FindByNameAsync(model.Username);
            if (existing != null)
            {
                var checkPassword = await _userManager.CheckPasswordAsync(existing, model.Password);
                if (checkPassword == true)
                {
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, existing.UserName),
                        new Claim(ClaimTypes.Email, existing.Email),
                        new Claim(ClaimTypes.Role, "Manager"),
                        new Claim(ClaimTypes.DateOfBirth,"2005.02.02"),
                        new Claim("Tax","2345654"),
                    
                    };
                    //cookie mesoza 
                     await _signInManager.SignInWithClaimsAsync(existing, model.RememberMe, claims);
                     if (string.IsNullOrEmpty(model.ReturnUrl) == true)
                     {
                         return RedirectToAction("Index");
                     }
                     else return Redirect(model.ReturnUrl);
                }
            }
            return View(model);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            
            return View(new RegisterViewModel());
        }
        
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid == false) return View(model);
          
                var user = new IdentityUser()
                {
                    Email = model.Email,
                    UserName = model.Username
                };
                var result = await _userManager.CreateAsync(user,model.Password);
                if (result.Succeeded == true)
                {
                    return RedirectToAction("Login");
                }
            ModelState.AddModelError("ConfirmPassword",result.Errors.FirstOrDefault().Description);
            return View(model);
        }

        
        public async Task<IActionResult> LogOutAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}