
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using mvcStore.Models;

// namespace mvcStore.Controllers;
namespace mvcStore.Controllers;

// [Microsoft.AspNetCore.Authorization.Authorize]
[Microsoft.AspNetCore.Authorization.Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [Microsoft.AspNetCore.Authorization.AllowAnonymous]

    public IActionResult Error()
    {
        var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        var model = new ErrorViewModel
        {
            RequestId = requestId
        };
        return View(model);
    }
}


