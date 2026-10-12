
namespace mvc_Dapper.Models.Entity
{
    public class z_org_user_group
    {
        public int rowid { get; set; }

        public string? user_no { get; set; }

        public string? mname { get; set; }

        public string? group_no { get; set; }

        public string? code_user { get; set; }

        // FORMA checkbox 使用
        public bool is_checked { get; set; }
    }
}