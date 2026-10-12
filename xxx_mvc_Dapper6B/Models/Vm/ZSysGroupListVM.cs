
using mvc_Dapper.Models.Entity;

namespace mvc_Dapper.Models.ViewModel
{
    public class ZSysGroupListVM
    {
        public List<z_sys_group> Groups { get; set; } = new List<z_sys_group>();

        public string? SelectedGroupNo { get; set; }

        public z_sys_group? SelectedGroup { get; set; }

        public List<z_org_user_group> Details { get; set; } = new List<z_org_user_group>();
    }
}