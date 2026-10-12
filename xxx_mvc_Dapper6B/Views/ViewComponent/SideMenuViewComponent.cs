

using Microsoft.AspNetCore.Mvc;
using mvc_Dapper.Models.Service;

namespace mvc_Dapper.ViewComp
{
    public class SideMenuViewComponent : ViewComponent
    {
        private readonly IZSysProgramService _service;

        public SideMenuViewComponent(IZSysProgramService service)
        {
            _service = service;
        }

        // public IViewComponentResult Invoke()
        // {
        //     var menu=_service.GetMenuTree();
        //     return View(menu);
        // }

        public IViewComponentResult Invoke()
        {
            // 從 Session 取得登入者的權限代碼：u / m
            var codeUser = HttpContext.Session.GetString("CodeUser") ?? "";

            // 把 codeUser 傳到 Service
            var menu = _service.GetMenuTree(codeUser);

            return View(menu);
        }

    }
}