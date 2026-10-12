


using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using mvc_Dapper.Models.Entity;
using mvc_Dapper.Models.Repository;
using mvc_Dapper.Models.ViewModel;

namespace mvc_Dapper.Controllers
{
    public class ZDemoEmployeeController : Controller
    {

        private readonly IZDemoEmployeeRepo _repo;
        private readonly IZDemoEmployeeSchoolRepo _schoolRepo;
        private readonly IZDemoEmployeeExperienceRepo _experienceRepo;

        public ZDemoEmployeeController(
            IZDemoEmployeeRepo repo,
            IZDemoEmployeeSchoolRepo schoolRepo,
            IZDemoEmployeeExperienceRepo experienceRepo
        )
        {
            _repo = repo;
            _schoolRepo = schoolRepo;
            _experienceRepo = experienceRepo;
        }

        public IActionResult List(int page = 1, int pageSize = 10)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (pageSize <= 0)
            {
                pageSize = 10;
            }

            var allData = _repo.GetAll();
            int totalCount = allData.Count;

            int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var pageData = allData
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

            var vm = new ListVM<z_demo_employee>();

            vm.Items = pageData;
            vm.PageSize = pageSize;
            vm.PageInfo = new PageInfo();
            vm.PageInfo.Page = page;
            vm.PageInfo.PageSize = pageSize;
            vm.PageInfo.TotalCount = totalCount;
            vm.PageInfo.ActionName = "List";

            return View(vm);

        }

        // Vue 版本的員工清單頁面，可由 /ZDemoEmployee/VueList 單獨開啟。
        public IActionResult VueList()
        {
            return View();
        }

        // 提供 VueList 使用的分頁 JSON 資料。
        [HttpGet]
        public IActionResult VueListData(int page = 1, int pageSize = 10)
        {
            page = Math.Max(page, 1);
            pageSize = new[] { 5, 10, 20, 50 }.Contains(pageSize)
                ? pageSize
                : 10;

            var allData = _repo.GetAll();
            var totalCount = allData.Count;
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            if (totalPages > 0 && page > totalPages)
            {
                page = totalPages;
            }

            var items = allData
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Json(new
            {
                items,
                page,
                pageSize,
                totalCount,
                totalPages
            });
        }


        public IActionResult Details(int id)
        {
            var data = _repo.GetById(id);

            if (data == null)
            {
                return NotFound();
            }

            ViewBag.DepartmentName = "";
            ViewBag.TitleName = "";

            var departments = _repo.GetDepartmentOptions();

            foreach (var item in departments)
            {
                if (item.Value == data.dept_no)
                {
                    ViewBag.DepartmentName = item.Text;
                    break;
                }

            }

            var titles = _repo.GetTitleOptions();

            foreach (var item in titles)
            {
                if (item.Value == data.title_no)
                {
                    ViewBag.TitleName = item.Text;
                    break;
                }
            }

            string employeeNo = "";

            if (data.mno != null)
            {
                employeeNo = data.mno;
            }

            ViewBag.Schools = _schoolRepo.GetByEmployeeNo(employeeNo);
            ViewBag.Experiences = _experienceRepo.GetByEmployeeNo(employeeNo);

            return View(data);

        }

        public IActionResult Create()
        {

            var data = new z_demo_employee();
            data.is_enabled = true;
            ViewBag.departments = _repo.GetDepartmentOptions();
            ViewBag.titles = _repo.GetTitleOptions();

            return View(data);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(z_demo_employee data)
        {
            if (ModelState.IsValid == false)
            {
                ViewBag.departments = _repo.GetDepartmentOptions();
                ViewBag.titles = _repo.GetTitleOptions();
                return View(data);
            }
            _repo.Create(data);

            return RedirectToAction(nameof(List));
        }

        public IActionResult Edit(int id)
        {

            var data = _repo.GetById(id);
            if (data == null)
            {
                return NotFound();
            }

            ViewBag.departments = _repo.GetDepartmentOptions();
            ViewBag.titles = _repo.GetTitleOptions();

            return View(data);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(z_demo_employee data)
        {
            if (ModelState.IsValid == false)
            {
                ViewBag.departments = _repo.GetDepartmentOptions();
                ViewBag.titles = _repo.GetTitleOptions();

                return View(data);

            }

            _repo.Update(data);

            return RedirectToAction("List");

        }

        public IActionResult Delete(int id)
        {
            var data = _repo.GetById(id);
            if (data == null)
            {
                return NotFound();
            }

            ViewBag.DepartmentName = "";
            ViewBag.TitleName = "";


            var departments = _repo.GetDepartmentOptions();

            foreach (var item in departments)
            {
                if (item.Value == data.dept_no)
                {
                    ViewBag.DepartmentName = item.Text;
                    break;
                }
            }

            var titles = _repo.GetTitleOptions();

            foreach (var item in titles)
            {
                if (item.Value == data.title_no)
                {
                    ViewBag.TitleName = item.Text;
                    break;
                }
            }

            return View(data);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirm(int rowid)
        {
            _repo.Delete(rowid);

            return RedirectToAction("List");
        }

    }
}
