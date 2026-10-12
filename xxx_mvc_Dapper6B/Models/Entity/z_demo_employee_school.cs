

using System.ComponentModel.DataAnnotations;

namespace mvc_Dapper.Models.Entity
{
    public class z_demo_employee_school
    {
        [Key]
        public int rowid { get; set; }

        [Display(Name = "員工編號")]
        public string? employee_no { get; set; }

        [Display(Name = "學校名稱")]
        public string? name_school { get; set; }

        [Display(Name = "科系")]
        public string? name_subject { get; set; }

        [Display(Name = "入學日期")]
        public DateTime? date_start { get; set; }

        [Display(Name = "離校日期")]
        public DateTime? date_end { get; set; }

        [Display(Name = "畢業狀況")]
        public string? code_status { get; set; }

        [Display(Name = "備註")]
        public string? remark { get; set; }
    }
}