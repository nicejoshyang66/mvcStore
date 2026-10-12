using mvc_Dapper.Models.Entity;

namespace mvc_Dapper.Models.Repository
{
    public interface IZDemoCustomerRepo
    {
        List<z_demo_customer> GetAll();

        z_demo_customer? GetById(int rowid);

        void Create(z_demo_customer data);

        void Update(z_demo_customer data);

        void Delete(int rowid);
    }
}