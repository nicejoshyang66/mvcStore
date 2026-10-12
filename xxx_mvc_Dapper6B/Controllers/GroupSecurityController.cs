
using Microsoft.AspNetCore.Mvc;
using mvc_Dapper.Models.Repository;
using mvc_Dapper.Models.ViewModel;

namespace mvc_Dapper.Controllers
{
    public class GroupSecurityController : Controller
    {
        private readonly IZSysSecurityRepo _repo;

        public GroupSecurityController(IZSysSecurityRepo repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult List(string? mno)
        {
            var vm = new GroupSecurityVM();

            vm.Groups = _repo.GetGroups();

            if (string.IsNullOrWhiteSpace(mno))
            {
                var firstGroup = vm.Groups.FirstOrDefault();

                if (firstGroup != null)
                {
                    mno = firstGroup.mno;
                }
            }

            vm.SelectedMno = mno;

            var group = vm.Groups.FirstOrDefault(x => x.mno == mno);
            vm.SelectedMname = group?.mname;

            if (!string.IsNullOrWhiteSpace(mno))
            {
                vm.Details = _repo.GetSecurityByGroup(mno);

                ViewBag.SourceGroups = _repo.GetSourceGroups(mno);
            }
            else
            {
                ViewBag.SourceGroups = new List<GroupSecurityMasterVM>();
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(GroupSecurityVM vm)
        {
            if (string.IsNullOrWhiteSpace(vm.SelectedMno))
            {
                TempData["Message"] = "沒有選擇群組，無法存檔";
                return RedirectToAction(nameof(List));
            }

            if (vm.Details == null)
            {
                vm.Details = new List<GroupSecurityDetailVM>();
            }

            _repo.UpdateGroupSecurity(vm.SelectedMno, vm.Details);

            TempData["Message"] = "權限資料已存檔";

            return RedirectToAction(nameof(List), new { mno = vm.SelectedMno });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CopyGroupSecurity(string targetNo, string sourceNo)
        {
            if (string.IsNullOrWhiteSpace(targetNo))
            {
                TempData["Message"] = "沒有目標群組，無法複製";
                return RedirectToAction(nameof(List));
            }

            if (string.IsNullOrWhiteSpace(sourceNo))
            {
                TempData["Message"] = "請選擇來源群組";
                return RedirectToAction(nameof(List), new { mno = targetNo });
            }

            if (targetNo == sourceNo)
            {
                TempData["Message"] = "來源群組不可與目前群組相同";
                return RedirectToAction(nameof(List), new { mno = targetNo });
            }

            _repo.CopyGroupSecurity(targetNo, sourceNo);

            TempData["Message"] = "已將來源群組權限複製到目前群組";

            return RedirectToAction(nameof(List), new { mno = targetNo });
        }

        [HttpGet]
        public IActionResult Cancel(string mno)
        {
            return RedirectToAction(nameof(List), new { mno });
        }
    }
}