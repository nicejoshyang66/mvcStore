using mvc_Dapper.Models.Entity;

namespace mvc_Dapper.Models.Repository
{
    public interface IZDemoEmployeeExperienceRepo
    {
        List<z_demo_employee_experience> GetByEmployeeNo(string employeeNo);

        z_demo_employee_experience? GetById(int rowid);

        void Create(z_demo_employee_experience data);

        void Update(z_demo_employee_experience data);

        void Delete(int rowid);
    }
}