namespace mvc_Dapper.Models.ViewModel
{
    public class GroupSecurityVM
    {
        public List<GroupSecurityMasterVM> Groups { get; set; } = new();

        public string? SelectedMno { get; set; }

        public string? SelectedMname { get; set; }

        public List<GroupSecurityDetailVM> Details { get; set; } = new();
    }

    public class GroupSecurityMasterVM
    {
        public string? mno { get; set; }

        public string? mname { get; set; }
    }

    public class GroupSecurityDetailVM
    {
        public int rowid { get; set; }

        public string? target_no { get; set; }

        public string? program_no { get; set; }

        public string? program_name { get; set; }

        public bool is_add { get; set; }

        public bool is_edit { get; set; }

        public bool is_delete { get; set; }

        public bool is_confirm { get; set; }

        public string? remark { get; set; }
    }
}