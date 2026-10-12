
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;

namespace mvc_Dapper.Models.Entity
{
    public class z_sys_program
    {
        public int rowid { get; set; }

        [Display(Name = "代號")]
        [Required(ErrorMessage = "代號不可空白")]
        public string? mno { get; set; }

        [Display(Name = "名稱")]
        public string? mname { get; set; }

        [Display(Name = "主階代碼")]
        public string? parentno { get; set; }

        [Display(Name = "啟用")]
        public bool is_enabled { get; set; }
        
        [Display(Name = "類別")]
        public string? code_no { get; set; }

        [Display(Name = "類別")]
        public string? code_name { get; set; }

        [Display(Name ="type_no")]
        public string? type_no { get; set; }

        [Display(Name = "Controller")]
        public string? controllername { get; set; }
        
        [Display(Name = "Action")]
        public string? actionname { get; set; }
        
        [Display(Name = "排序")]
        public string? no_sort { get; set; }

        [Display(Name = "備註")]
        public string? remark { get; set; }

        [Display(Name = "參數")]
        public string? routename { get; set; }

        [Display(Name ="Children")]
        public List<z_sys_program> Children { get; set; } = new List<z_sys_program>();

        [Display(Name = "程式")]
        public string url
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(controllername) && !string.IsNullOrWhiteSpace(actionname))
                {
                    return $"/{controllername}/{actionname}";
                }
                return "#";
            }
        }

    }

}