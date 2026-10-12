

using mvc_Dapper.Models.Entity;

namespace mvc_Dapper.Models.Repository
{
    public interface IZDemoVendorRepo
    {
        List<z_demo_vendor>GetAll();

        z_demo_vendor? GetById(int rowid);

        void Create(z_demo_vendor data);

        void Update(z_demo_vendor data);

        void Delete(int rowid);

    }
}