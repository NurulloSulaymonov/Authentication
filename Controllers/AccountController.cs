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
        List<User> users;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;


        public AccountController(SignInManager<User> signInManager,
            UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
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
        public async Task<IActionResult> LoginAsync(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Username);

                if (user == null) return View(model);

                var exists = await _userManager.CheckPasswordAsync(user, model.Password);

                if (exists == false) return View(model);

                var userClaims = await _userManager.GetRolesAsync(user);

                // claim is the information about a user 
                //example Name, PhoneNumber, Email, Role 
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Username),
                };
                //add roles to claim
                foreach (var userClaim in userClaims)
                {
                    claims.Add(new Claim(ClaimTypes.Role, userClaim));
                }

                await _userManager.AddClaimsAsync(user, claims);
                await _signInManager.SignInAsync(user, model.RememberMe);

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new UserRegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserRegisterViewModel model)
        {
            if (ModelState.IsValid == false) return View(model);

            var user = new User()
            {
                UserName = model.Username,
                Email = model.Username
            };

            var response  = await _userManager.CreateAsync(user,model.Password);

            if (response.Succeeded) return RedirectToAction("Index","Home");
            
            ModelState.AddModelError("Username",response.Errors.First().Description);
            return View(model);
        }

        public async Task<IActionResult> LogOutAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}