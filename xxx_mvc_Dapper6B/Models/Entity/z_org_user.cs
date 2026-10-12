
using System.ComponentModel.DataAnnotations;

namespace mvc_Dapper.Models.Entity
{
    public class z_org_user
    {
        [Key]
        public int rowid { get; set; }

        [Display(Name ="")]
        public string? code_user { get; set; }

        [Display(Name ="啟用")]
        public bool is_enabled { get; set; }

        [Display(Name ="代號")]
        public string? mno { get; set; }

        [Display(Name ="名稱")]
        public string? mname { get; set; }

        [Display(Name ="密碼")]
        public string? pass_user { get; set; }

        [Display(Name ="電話")]
        public string? tel_user  { get; set; }

         [Display(Name ="手機")]
        public string? mobile_user { get; set; }

         [Display(Name ="電子郵件")]
        public string? email_user { get; set; }

         [Display(Name ="備註")]
        public string? remark { get; set; }

    }
}