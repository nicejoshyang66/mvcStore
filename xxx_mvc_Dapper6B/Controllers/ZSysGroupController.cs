using Microsoft.AspNetCore.Mvc;
using mvc_Dapper.Filters;
using mvc_Dapper.Models.Entity;
using mvc_Dapper.Models.Repository;
using mvc_Dapper.Models.ViewModel;

namespace mvc_Dapper.Controllers
{
    [LoginCheck]
    public class ZSysGroupController : Controller
    {
        private readonly IZSysGroupRepo _repo;

        public ZSysGroupController(IZSysGroupRepo repo)
        {
            _repo = repo;
        }

        public IActionResult List(string? groupNo)
        {
            var vm = new ZSysGroupListVM();

            vm.Groups = _repo.List();

            // 一進入 List，如果沒有指定 groupNo，就自動抓第一筆群組
            if (string.IsNullOrEmpty(groupNo))
            {
                if (vm.Groups != null && vm.Groups.Count > 0)
                {
                    groupNo = vm.Groups[0].mno;
                }
            }

            vm.SelectedGroupNo = groupNo;

            if (!string.IsNullOrEmpty(groupNo))
            {
                vm.SelectedGroup = _repo.GetByMno(groupNo);
                vm.Details = _repo.ListGroupUsers(groupNo);
            }

            return View(vm);
        }

        public IActionResult GroupUsers(string groupNo)
        {
            if (string.IsNullOrEmpty(groupNo))
            {
                return RedirectToAction(nameof(List));
            }

            var group = _repo.GetByMno(groupNo);

            if (group == null)
            {
                return NotFound();
            }

            var vm = new ZSysGroupUsersVM
            {
                group_no = group.mno,
                group_name = group.mname,
                Users = _repo.ListAllUsersForGroup(groupNo)
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GroupUsers(string groupNo, List<string>? selectedUserNo)
        {
            if (string.IsNullOrEmpty(groupNo))
            {
                return RedirectToAction(nameof(List));
            }

            _repo.UpdateGroupUsers(groupNo, selectedUserNo);

            return RedirectToAction(nameof(List), new { groupNo = groupNo });
        }

        public IActionResult Create()
        {
            var model = new z_sys_group
            {
                is_enabled = true
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(z_sys_group model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _repo.Create(model);
            return RedirectToAction(nameof(List));
        }

        public IActionResult Edit(int rowid)
        {
            var model = _repo.GetByRowid(rowid);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(z_sys_group model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _repo.Edit(model);
            return RedirectToAction(nameof(List));
        }

        public IActionResult Delete(int rowid)
        {
            var model = _repo.GetByRowid(rowid);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCFM(int rowid)
        {
            _repo.Delete(rowid);
            return RedirectToAction(nameof(List));
        }
    }
}