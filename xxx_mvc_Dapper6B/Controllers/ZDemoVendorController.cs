
using Microsoft.AspNetCore.Mvc;
using mvc_Dapper.Models.Entity;
using mvc_Dapper.Models.Repository;
using mvc_Dapper.Models.ViewModel;

namespace mvc_Dapper.Controllers
{
    public class ZDemoVendorController : Controller
    {
        private readonly IZDemoVendorRepo _repo;

        public ZDemoVendorController(IZDemoVendorRepo repo)
        {
            _repo = repo;
        }

        public IActionResult List(int page=1,int pageSize=10)
        {
            if (page < 1)
            {
                page=1;
            }

            if (pageSize<=10)
            {
                pageSize=10;
            }

            var allData=_repo.GetAll();
            int totalCount=allData.Count;
            int totalPages=(int)Math.Ceiling((double)totalCount/pageSize);

            if (totalPages > 0 && page > totalPages)
            {
                page=totalPages;
            }

            var pageData=allData
            .Skip((page-1)*pageSize)
            .Take(pageSize)
            .ToList();

            var vm=new ListVM<z_demo_vendor>();

            vm.Items=pageData;
            vm.PageInfo=new PageInfo();
            vm.PageInfo.Page=page;
            vm.PageInfo.PageSize=pageSize;
            vm.PageInfo.TotalCount=totalCount;
            vm.PageInfo.ActionName="List";
           
            return View(vm);
            
        }

        public IActionResult Details(int id)
        {
            var data=_repo.GetById(id);
            if (data==null)
            {
                return NotFound();
            }

            return View(data);
        }

        public IActionResult Create()
        {
            var data=new z_demo_vendor();
            data.is_enabled=true;
            return View(data);
        }

        [HttpPost]
        public IActionResult Create(z_demo_vendor data)
        {
            if (ModelState.IsValid==false)
            {
                return View(data);
            }

            _repo.Create(data);

            return RedirectToAction(nameof(List));

        }

        public IActionResult Edit(int id)
        {
            var data=_repo.GetById(id);
            if (data==null)
            {
                return NotFound();
            }
            return View(data);
     
        }

        [HttpPost]
        public IActionResult Edit(z_demo_vendor data)
        {
            if (ModelState.IsValid==false)
            {
                return View(data);
            }

            _repo.Update(data);

            return RedirectToAction(nameof(List));

        }

        public IActionResult Delete(int id)
        {
            var data=_repo.GetById(id);

            if (data==null)
            {
                return NotFound();
            }                

            return View(data);

        }

        [HttpPost]
        public IActionResult DeleteConfirm(int rowid)
        {
            _repo.Delete(rowid);
            return RedirectToAction(nameof(List));
        }





    }


}