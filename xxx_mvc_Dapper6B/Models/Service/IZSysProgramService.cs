
using mvc_Dapper.Models.Entity;

namespace mvc_Dapper.Models.Service
{
    public interface IZSysProgramService
    {
        List<z_sys_program> GetMenuTree(string code_user);
    }
}