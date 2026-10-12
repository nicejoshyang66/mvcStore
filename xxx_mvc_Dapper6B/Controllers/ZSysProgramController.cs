
using Microsoft.AspNetCore.Mvc;
using mvc_Dapper.Filters;
using mvc_Dapper.Models.Entity;
using mvc_Dapper.Models.Repo;
using mvc_Dapper.Models.Vm;

namespace mvc_Dapper.Controllers
{
    [LoginCheck]
    public class ZSysProgramController : Controller
    {
        private readonly IZSysProgramRepo _repo;

        public ZSysProgramController(IZSysProgramRepo repo)
        {
            _repo = repo;
        }

        public IActionResult List()
        {
            var mas = _repo.GetMasList(); // 只抓第一層 Master
            return View(mas);

        }

        // public IActionResult ListDet(string parentno)
        // {
        //     var mas = _repo.GetMas(parentno); // 抓被點選的 Master
        //     var detlist = _repo.GetDetList(parentno); // 抓 Detail

        //     var vm = new ZSysProgramVm
        //     {
        //         Mas = mas,
        //         DetList = detlist
        //     };

        //     return View(vm);
        // }

        public IActionResult ListDet(string parentno)
        {
            var mas=_repo.GetMas(parentno); //one record
            var det=_repo.GetDetList(parentno); // multi-records

            var vm=new ZSysProgramVm
            {
                Mas=mas,
                DetList=det
            };

            return View(vm);

        }


        public IActionResult CreateMas()
        {
            var model = new z_sys_program
            {
                code_no = "M",
                is_enabled = true
            };

            return View(model);
        }

        [HttpPost, ActionName("CreateMas")]
        public IActionResult CreateCFM(z_sys_program mas)
        {
            // if (!ModelState.IsValid) return View();
            if (!ModelState.IsValid) return View(mas);
            bool ok = _repo.AddMas(mas);
            if (!ok)
            {
                ModelState.AddModelError("", "新增失敗");
                return View(mas);
            }
            return RedirectToAction(nameof(List));
        }

        public IActionResult CreateDet(string parentno)
        {
            var mas = _repo.GetMas(parentno);

            ViewBag.parentname = "";
            if (mas != null)
            {
                ViewBag.parentname = mas.mname;
            }

            var model = new z_sys_program
            {
                parentno = parentno,
                code_no = "M",
                is_enabled = true
            };

            return View(model);
        }

        [HttpPost, ActionName("CreateDet")]

        public IActionResult CreateDetCFM(z_sys_program det)
        {
            // if (!string.IsNullOrWhiteSpace(det.parentno))
            if (!ModelState.IsValid)
            {
                // 先暫時以 ! 處理
                var mas = _repo.GetMas(det.parentno!);

                ViewBag.parentname = "";
                if (mas != null)
                {
                    ViewBag.parentname = mas.mname;
                }

                return View(det);   // 重點：一定要把 det 傳回去
            }

            bool ok = _repo.AddDet(det);
            if (!ok)
            {
                ModelState.AddModelError("", "新增失敗");
                return View(det);
            }

            return RedirectToAction(nameof(ListDet), new { parentno = det.parentno });

        }

        public IActionResult Delete(string id)
        {
            z_sys_program? mas = _repo.GetMas(id);
            return View(mas);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteCFM(string id)
        {
            if (_repo.DeleteMas(id))
                return RedirectToAction(nameof(List));

            return Content("刪除一筆記錄，*** Delete失敗！");

        }

        public IActionResult Edit(string id)
        {
            z_sys_program? mas = _repo.GetMas(id);
            return View(mas);
        }

        [HttpPost,ActionName("Edit")]
        public IActionResult EditCFM(z_sys_program mas)
        {
            if (!ModelState.IsValid) return View();
                //return Content("找不到任何記錄，*** Edit 失敗1");
                
            if (_repo.UpdateMas(mas))
                return RedirectToAction(nameof(List));

            return Content("找不到任何記錄，*** Edit 失敗2");
        }

        public IActionResult EditDet(int id)
        {
            z_sys_program? det = _repo.GetDet(id);
            return View(det);
        }

        [HttpPost, ActionName("EditDet")]
        public IActionResult EditDetCFM(z_sys_program det)
        {
            if (!ModelState.IsValid) return View();

            if (_repo.UpdateDet(det))
                return RedirectToAction(nameof(ListDet), new { parentno=det.parentno });

            return Content("找不到任何記錄，*** Edit 失敗2");
        }

        public IActionResult DeleteDet(int id)
        {
            z_sys_program? det = _repo.GetDet(id);
            return View(det);
        }

        [HttpPost, ActionName("DeleteDet")]
        public IActionResult DeleteDetCFM(z_sys_program det)
        {
            if (_repo.DeleteDet(det.rowid))
                return RedirectToAction(nameof(ListDet), new { parentno=det.parentno });

            return Content("刪除一筆記錄，*** Delete失敗！");
        }


    }

}

