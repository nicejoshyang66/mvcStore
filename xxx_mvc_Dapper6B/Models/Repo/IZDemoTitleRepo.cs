using mvc_Dapper.Models.Entity;

namespace mvc_Dapper.Models.Repository
{
    public interface IZDemoTitleRepo
    {
        List<z_demo_title> GetAll();

        z_demo_title? GetById(int rowid);

        void Create(z_demo_title data);

        void Update(z_demo_title data);

        void Delete(int rowid);
    }
}