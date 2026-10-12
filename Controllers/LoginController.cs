using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using mvcStore.Services;

namespace mvcStore.Controllers;

[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
public sealed class LoginController : Controller
{
    private readonly NavigationRepository _repository;
    private readonly ILogger<LoginController> _logger;

    public LoginController(
        NavigationRepository repository,
        ILogger<LoginController> logger
    )
    {
        _repository = repository;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }


    [HttpPost("api/auth/login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.UserId) ||
        string.IsNullOrEmpty(request.Password))
        {
            return BadRequest(new
            {
                message = "請輸入帳號與密碼"
            });
        }


        LoginUser? user;

        try
        {
            user = await _repository.myLogin(request.UserId, request.Password);

        }
        catch (Exception ex) when (ex is Microsoft.Data.SqlClient.SqlException or InvalidOperationException)
        {
            _logger.LogError(ex, "Login database unavailable");
            return StatusCode(503, new
            {
                message = "登入服務暫時無法使用，請確認資料庫連線設定"
            });
        }

        if (user == null)
        {
            return Unauthorized(new
            {
                message = "帳號或密碼錯誤"
            });
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier,user.UserId),
            new Claim(ClaimTypes.Name, user.UserName??""),
            new Claim("CodeUser",user.CodeUser??"")
        };

        var identity = new ClaimsIdentity(claims, "StoreCookie");
        var principal = new ClaimsPrincipal(identity);
        var AuthenticationProperties = new AuthenticationProperties
        {
            IsPersistent = request.RememberMe
        };

        await HttpContext.SignInAsync("StoreCookie", principal, AuthenticationProperties);

        return Ok(new
        {
            redirectUrl = Url.Action("Index", "Home")
        });

    }


    [Authorize]
    [HttpPost("api/auth/logout")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> logout()
    {
        await HttpContext.SignOutAsync("StoreCookie");
        return Ok(new
        {
            redirectUrl = Url.Action("Index", "Login")
        });
    }

}


public sealed record LoginRequest(
    string? UserId,
    string? Password,
    bool RememberMe
);
