using mvc_Dapper.Models.Entity;

namespace mvc_Dapper.Models.Repository
{
    public interface IZDemoEmployeeSchoolRepo
    {
        List<z_demo_employee_school> GetByEmployeeNo(string employeeNo);

        z_demo_employee_school? GetById(int rowid);

        void Create(z_demo_employee_school data);

        void Update(z_demo_employee_school data);

        void Delete(int rowid);
    }
}