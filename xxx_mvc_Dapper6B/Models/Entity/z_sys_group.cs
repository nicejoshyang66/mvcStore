

using System.ComponentModel.DataAnnotations;

namespace mvc_Dapper.Models.Entity
{
    public class z_sys_group
    {
        [Key]
        public int rowid { get; set; }

        [Display(Name = "啟用")]
        public bool is_enabled { get; set; }

        [Display(Name = "代號")]
        public string? mno { get; set; }

        [Display(Name = "名稱")]
        public string? mname { get; set; }

        [Display(Name = "備註")]
        public string? remark { get; set; }

    }
}