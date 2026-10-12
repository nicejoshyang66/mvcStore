
using mvc_Dapper.Models.Entity;

namespace mvc_Dapper.Models.ViewModel
{
    public class ZSysGroupUsersVM
    {
        public string? group_no { get; set; }

        public string? group_name { get; set; }

        public List<z_org_user_group> Users { get; set; } = new List<z_org_user_group>();
    }
}