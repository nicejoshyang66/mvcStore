

using System.ComponentModel.DataAnnotations;

namespace mvc_Dapper.Models.Entity
{
    public class z_demo_employee_experience
    {

        [Key]
        public int rowid { get; set; }

        [Display(Name = "員工編號")]
        public string? employee_no { get; set; }

        [Display(Name = "公司名稱")]
        public string? name_company { get; set; }

        [Display(Name = "職稱")]
        public string? title_company { get; set; }

        [Display(Name = "薪資")]
        public int? base_salary { get; set; }

        [Display(Name = "到職日期")]
        public DateTime? date_start { get; set; }

        [Display(Name = "離職日期")]
        public DateTime? date_end { get; set; }

        [Display(Name = "備註")]
        public string? remark { get; set; }
    }
}