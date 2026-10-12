

using mvc_Dapper.Models.Entity;

namespace mvc_Dapper.Models.Repository
{
    public interface IZOrgUserRepo
    {

        List<z_org_user>List(string code_user);
        z_org_user? GetById(int rowid);
        bool Create(z_org_user mas);
        bool Update(z_org_user mas);
        bool Delete(int rowid);
        bool ChangePassword(int rowid,string newPassword);
        z_org_user? Login(string userId,string password);

    }
}