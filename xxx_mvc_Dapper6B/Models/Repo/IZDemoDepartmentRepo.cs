

using mvc_Dapper.Models.Entity;

namespace mvc_Dapper.Models.Repository
{
    public interface IZDemoDepartmentRepo
    {
        List<z_demo_department> GetAll();

        z_demo_department? GetById(int rowid);

        void Create(z_demo_department data);

        void Update(z_demo_department data);

        void Delete(int rowid);
    }
}