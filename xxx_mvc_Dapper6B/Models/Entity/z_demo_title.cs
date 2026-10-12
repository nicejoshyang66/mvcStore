

using System.ComponentModel.DataAnnotations;

namespace mvc_Dapper.Models.Entity
{
    
    public class  z_demo_title
    {
        [Key]
        public int rowid  { get; set; }
        
        [Display(Name ="職務編號")]
        public string? mno { get; set; }

        [Display(Name ="職務名稱")]
        public string? mname { get; set; }

        [Display(Name ="備註")]
        public string? remark { get; set; }

    }


}