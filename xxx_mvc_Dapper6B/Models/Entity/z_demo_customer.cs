
using System.ComponentModel.DataAnnotations;

namespace mvc_Dapper.Models.Entity
{
    public class z_demo_customer
    {
        [Key]
        public int rowid { get; set; }  

        [Display(Name ="啟用")]
        public bool is_enabled { get; set; }
        
        [Required(ErrorMessage = "客戶編號不可空白")]
        [Display(Name ="客戶編號")]
        public string? mno { get; set; }

        [Required(ErrorMessage = "客戶簡稱不可空白")]
        [Display(Name ="客戶簡稱")]        
        public string? msname { get; set; }

        [Required(ErrorMessage = "客戶名稱不可空白")]    
        [Display(Name ="客戶名稱")]
        public string? mname { get; set; }

        [Display(Name ="統一編號")]
        public string? no_serial { get; set; }
        
        [Display(Name ="發票地址")]
        public string? name_invoice { get; set; }
        
        [Display(Name ="電話")]
        public string? tel_company { get; set; }
        
        [Display(Name ="負責人")]
        public string? name_charge { get; set; }
        
        [Display(Name ="聯絡人")]
        public string? name_contact { get; set; }
        
        [Display(Name ="公司地址")]
        public string? adr_company { get; set; }

        [Display(Name ="發票地址")]
        public string? adr_invoice { get; set; }

        [Display(Name ="備註")]
        public string? remark { get; set; }
    }
}