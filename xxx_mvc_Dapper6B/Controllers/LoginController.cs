using Microsoft.AspNetCore.Mvc;
using mvc_Dapper.Models.Repository;

namespace mvc_Dapper.Controllers
{
    public class LoginController : Controller
    {
        private readonly IZOrgUserRepo _repo;

        private const string CookieLoginUserId="A";
        private const string CookieLoginUserName = "B";
        private const string CookieCodeUser = "C";

        public LoginController(IZOrgUserRepo repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            // 1. 如果 Session 還在，直接進首頁
            var sessionUserId=HttpContext.Session.GetString("LoginUserId");
            if (!string.IsNullOrEmpty(sessionUserId))
            {
                return RedirectToAction("Index","Home");
            }

            // 如果 Session 不在，但 Cookie 還在，還原 Session

            var cookieUserId=Request.Cookies[CookieLoginUserId];
            var cookieUserName=Request.Cookies[CookieLoginUserName];
            var cookieCodeUser=Request.Cookies[CookieCodeUser];

            if (!string.IsNullOrEmpty(cookieUserId))
            {
                HttpContext.Session.SetString("LoginUserId",cookieUserId);
                HttpContext.Session.SetString("LoginUserName",cookieUserName?? "");
                HttpContext.Session.SetString("CodeUser",cookieCodeUser??"");

                return RedirectToAction("Index","Home");
            }

            return View();

        }

        [HttpPost]
        public IActionResult Index(string userid,string password, bool rememberMe)
        {
            var user=_repo.Login(userid,password);
            if (user!=null)
            {
                // 登入帳號
                HttpContext.Session.SetString(
                    "LoginUserId",user.mno??""                        
                );

                // 使用者名稱
                HttpContext.Session.SetString(
                    "LoginUserName",user.mname??""
                );

                // 權限代碼 u / m
                HttpContext.Session.SetString(
                    "CodeUser",user.code_user??""
                );

                // 勾選「記住登入」才寫入 Cookie

                if (rememberMe)
                {
                    var cookieOptions=new CookieOptions
                    {
                        Expires=DateTimeOffset.Now.AddDays(30),
                        HttpOnly=true,
                        IsEssential=true,
                        SameSite=SameSiteMode.Lax,
                        Secure=Request.IsHttps
                    };

                    Response.Cookies.Append(
                        CookieLoginUserId,user.mno??"",cookieOptions
                    );

                    Response.Cookies.Append(
                        CookieLoginUserName,user.mname??"",cookieOptions
                    );

                    Response.Cookies.Append(
                        CookieCodeUser,user.code_user??"",cookieOptions
                    );                        

                }

                return RedirectToAction("Index","Home");
            }

            ViewBag.ErrorMessage="帳號或密碼錯誤!!";

            return View();

        }
     
        public IActionResult Logout()
        {

            HttpContext.Session.Clear();   
            // 登出時一併清除 Cookie

            Response.Cookies.Delete(CookieLoginUserId);
            Response.Cookies.Delete(CookieLoginUserName);
            Response.Cookies.Delete(CookieCodeUser);

            return RedirectToAction("Index","Login");            

        }
    }
}