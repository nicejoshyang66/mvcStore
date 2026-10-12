

using mvc_Dapper.Models.Entity;
using mvc_Dapper.Models.ViewModels;

namespace mvc_Dapper.Models.Repository
{
    public interface IZDemoEmployeeRepo
    {
        
        List<z_demo_employee> GetAll();

        z_demo_employee? GetById(int rowid);

        List<SelectOptionVM> GetDepartmentOptions();

        List<SelectOptionVM> GetTitleOptions();

        void Create(z_demo_employee data);

        void Update(z_demo_employee data);
        
        void Delete(int rowid);

    }
}