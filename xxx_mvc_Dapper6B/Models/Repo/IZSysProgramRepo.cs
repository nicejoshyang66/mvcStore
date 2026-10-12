
using mvc_Dapper.Models.Entity;

namespace mvc_Dapper.Models.Repo
{
    public interface IZSysProgramRepo
    {

        List<z_sys_program> GetMasList();
        bool AddMas(z_sys_program mas);
        bool UpdateMas(z_sys_program mas);
        //key=mno
        bool DeleteMas(string id);
        bool AddDet(z_sys_program det);
        bool UpdateDet(z_sys_program det);
        // key = rowid
        bool DeleteDet(int id);
        //key = mno
        z_sys_program? GetMas(string id);
        List<z_sys_program> GetEnabled(string code_user);
        List<z_sys_program> GetDetList(string parentno);
        //key=rowid
        z_sys_program? GetDet(int id);

    }
}