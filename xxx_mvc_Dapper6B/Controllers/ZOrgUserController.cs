


using Microsoft.AspNetCore.Mvc;
using mvc_Dapper.Filters;
using mvc_Dapper.Models.Entity;
using mvc_Dapper.Models.Repository;
using mvc_Dapper.Models.Vm;

namespace mvc_Dapper.Controllers
{
    [LoginCheck]
    public class ZOrgUserController:Controller
    {
        private readonly IZOrgUserRepo _repo;
        
        public ZOrgUserController(IZOrgUserRepo repo)
        {
            _repo=repo;
        }

        public IActionResult List(string code_user)
        {
            ViewBag.CodeUser=code_user;
            var mas = _repo.List(code_user);
            return View(mas);
        }       

        public IActionResult Create(string code_user)
        {
            ViewBag.CodeUser=code_user;
            var mas=new z_org_user
            {
                code_user=code_user,
                is_enabled=true
            };

            return View(mas);
            
        }

        [HttpPost,ActionName("Create")]
        public IActionResult CreateCFM(z_org_user mas, string code_user)
        {
            mas.code_user=code_user;
            if (!ModelState.IsValid)
            {
                ViewBag.code_user=code_user;
                return View(mas);
            }

            _repo.Create(mas);

            return RedirectToAction("List",new{code_user=code_user});                

        }

        public IActionResult Edit(int rowid, string code_user)
        {
            ViewBag.CodeUser=code_user;
            var mas=_repo.GetById(rowid);

            if (mas==null)
            {
                return NotFound();
            }
            return View(mas);

        }

        [HttpPost,ActionName("Edit")]
        public IActionResult EditCFM(z_org_user mas, string code_user)
        {
            mas.code_user=code_user;
            if (!ModelState.IsValid)
            {
                return View(mas);
            }

            _repo.Update(mas);

            return RedirectToAction("List",new{code_user=code_user});
        }

        public IActionResult Delete(int rowid, string code_user)
        {
            ViewBag.CodeUser=code_user;
            var mas=_repo.GetById(rowid);
            if (mas==null)
            {
                return NotFound();
            }

            return View(mas);
        }                            

        [HttpPost,ActionName("Delete")]
        public IActionResult DeleteCFM(int rowid, string code_user)
        {
            _repo.Delete(rowid);
            return RedirectToAction("List",new{code_user=code_user});
        }

        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordVM mas)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                   success=false,
                   message="資料驗證失敗，請確認密碼是否一致。"
                });
            }

            _repo.ChangePassword(mas.rowid,mas.NewPassword!);

            return Json(new
            {
                success=true,
                message="密碼變更完成。"
            });

        }

    }
}