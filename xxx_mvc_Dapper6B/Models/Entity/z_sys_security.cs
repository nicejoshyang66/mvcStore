

using System.ComponentModel.DataAnnotations;

namespace mvc_Dapper.Models.Entity
{
    public class z_sys_security
    {
        [Key]
        public int rowid { get; set; }
        public string? type_no { get; set; }
        public string? target_no { get; set; }

        [Display(Name = "程式代號")]
        public string? program_no { get; set; }

        [Display(Name = "程式名稱")]
        public string? mname { get; set; }

        [Display(Name = "新增")]
        public bool is_add { get; set; }

        [Display(Name = "修改")]
        public bool is_edit { get; set; }
        
        [Display(Name = "刪除")]
        public bool is_delete { get; set; }
        
        [Display(Name = "簽核")]
        public bool is_confirm { get; set; }

        [Display(Name ="備註")]
        public string? remark { get; set; }
    }
}