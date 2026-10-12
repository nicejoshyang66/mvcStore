using System.Security.Claims;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mvcStore.Services;
// using Microsoft.Extensions.Configuration.UserSecrets;
// using Microsoft.AspNetCore.Components.Endpoints;

namespace mvcStore.Controllers;

[Authorize]
[Route("api/menu")]
[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]

public sealed class MenuController : Controller
{
    private readonly NavigationRepository _repository;
    private readonly ILogger<MenuController> _logger;

    public MenuController(NavigationRepository repository, ILogger<MenuController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            // 1. 取得登入者的權限代碼，再查詢可以顯示的選單。
            string codeUser = User.FindFirstValue("CodeUser") ?? "";
            List<MenuRow> rows = await _repository.Menu(codeUser);

            //2. 收集選單編號，用來判斷父選單是否存在。
            List<string> menuNumbers = new List<string>();

            foreach (MenuRow row in rows)
            {
                if (!string.IsNullOrWhiteSpace(row.mno))
                {
                    menuNumbers.Add(row.mno);
                }
            }

            // 3. 逐一建立主選單，以及它底下的子選單。
            List<object> menu = new List<object>();

            foreach (MenuRow root in rows)
            {
                bool hasNoParent = string.IsNullOrWhiteSpace(root.parentno);
                // ask 
                bool parentExists = menuNumbers.Contains(root.parentno ?? "");
                if (!hasNoParent && parentExists)
                {
                    continue;
                }

                List<object> children = new List<object>();

                foreach (MenuRow child in rows)
                {
                    if (!string.IsNullOrWhiteSpace(root.mno) && child.parentno == root.mno)
                    {
                        children.Add(new
                        {
                            id = child.rowid,
                            name = child.mname ?? "",
                            Url = MenuUrl(child)
                        });
                    }
                }

                menu.Add(new
                {
                    id = root.rowid,
                    name = root.mname ?? "",
                    url = MenuUrl(root),
                    children = children
                });

            }

            return Ok(new
            {
                userId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                userName = User.Identity?.Name,
                menu = menu

            });
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "Menu database unavailable");
            return StatusCode(503, new { message = " SQL 選單載入失敗，請稍後重試 " });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Menu2 database unavailable");
            return StatusCode(503, new { message = "   選單載入失敗，請稍後重試。 " });
        }

    }
    private string MenuUrl(MenuRow row)
    {
        // 沒有指定控制器或動作時，不提供頁面連結。
        if (string.IsNullOrWhiteSpace(row.controllername) || string.IsNullOrWhiteSpace(row.actionname))
        {
            return "#";
        }

        // 組成「網站根目錄/控制器/動作」的網址。
        string controllerName = Uri.EscapeDataString(row.controllername);
        string actionName = Uri.EscapeDataString(row.actionname);
        string path = Url.Content("~/") + controllerName + "/" + actionName;

        if (!string.IsNullOrWhiteSpace(row.routename))
        {
            path = path + "?" + row.routename.TrimStart('?');
        }

        return path;
    }
}
