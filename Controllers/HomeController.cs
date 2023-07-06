using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Auth.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Auth.Controllers;


public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [Authorize]
    public IActionResult Secret()
    {
        return View();
    }

    [Authorize(Roles = "Manager"),]
    public IActionResult OnlyManager()
    {
        return View();
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    
}
