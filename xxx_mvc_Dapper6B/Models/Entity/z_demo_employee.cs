

using System.ComponentModel.DataAnnotations;

namespace mvc_Dapper.Models.Entity
{
    public class z_demo_employee
    {
        [Key]
        public int rowid { get; set; }

        [Display(Name ="啟用")]
        public bool is_enabled { get; set; }

        [Display(Name = "員工編號")]
        public string? mno { get; set; }

        [Display(Name = "姓名")]
        public string? mname { get; set; }

        [Display(Name = "身分證號")]
        public string? no_serial { get; set; }

        [Display(Name = "部門")]
        public string? dept_no { get; set; }

        [Display(Name = "職稱")]
        public string? title_no { get; set; }

        [Display(Name = "出生年月日")]
        public DateTime? date_birth { get; set; }

        [Display(Name = "姓別")]
        public string? code_gender { get; set; }

        [Display(Name = "底薪")]
        public int? salary_base { get; set; }
 
        [Display(Name = "電話")]
        public string? tel_home { get; set; }

        [Display(Name = "地址")]
        public string? adr_home { get; set; }

        [Display(Name = "備註")]
        public string? remark { get; set; }

    }
}