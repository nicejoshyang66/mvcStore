
using Microsoft.AspNetCore.Mvc;
using mvc_Dapper.Models.Entity;
using mvc_Dapper.Models.Repository;

namespace mvc_Dapper.Controllers
{
    public class ZDemoEmployeeSchoolController : Controller
    {
        private readonly IZDemoEmployeeSchoolRepo _repo;

        public ZDemoEmployeeSchoolController(IZDemoEmployeeSchoolRepo repo)
        {
            _repo = repo;
        }

        public IActionResult List(string employeeNo)
        {
            ViewBag.EmployeeNo = employeeNo;

            var data = _repo.GetByEmployeeNo(employeeNo);

            return View(data);
        }

        public IActionResult Create(string employeeNo)
        {
            var data = new z_demo_employee_school();

            data.employee_no = employeeNo;

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(z_demo_employee_school data)
        {
            if (ModelState.IsValid == false)
            {
                return View(data);
            }

            _repo.Create(data);

            return RedirectToAction("List", new { employeeNo = data.employee_no });
        }

        public IActionResult Edit(int id)
        {
            var data = _repo.GetById(id);

            if (data == null)
            {
                return NotFound();
            }

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(z_demo_employee_school data)
        {
            if (ModelState.IsValid == false)
            {
                return View(data);
            }

            _repo.Update(data);

            return RedirectToAction("List", new { employeeNo = data.employee_no });
        }

        public IActionResult Delete(int id)
        {
            var data = _repo.GetById(id);

            if (data == null)
            {
                return NotFound();
            }

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirm(int rowid, string employeeNo)
        {
            _repo.Delete(rowid);

            return RedirectToAction("List", new { employeeNo });
        }
    }
}