using mvc_Dapper.Models.Entity;

namespace mvc_Dapper.Models.Repository
{
    public interface IZSysGroupRepo
    {
        List<z_sys_group> List();

        z_sys_group? GetByRowid(int rowid);

        z_sys_group? GetByMno(string mno);

        List<z_org_user_group> ListGroupUsers(string groupNo);

        List<z_org_user_group> ListAllUsersForGroup(string groupNo);

        int UpdateGroupUsers(string groupNo, List<string>? selectedUserNo);

        int Create(z_sys_group model);

        int Edit(z_sys_group model);

        int Delete(int rowid);
    }
}