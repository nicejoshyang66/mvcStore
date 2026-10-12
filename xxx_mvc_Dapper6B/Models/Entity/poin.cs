

using System.ComponentModel.DataAnnotations;

namespace mvc_Dapper.Models.Entity
{
    public class Poin
    {
        [Key]
        [Display(Name ="流水編號")]
        public string? tran_no  { get; set; }

        [Display(Name ="日期")]
        public DateTime po_date { get; set; }

        [Display(Name ="廠商編號")]
        public string? fact_no { get; set; }

        [Display(Name ="廠商名稱")]
        public string? brief_name { get; set; }

        [Display(Name ="金額")]       
        public int tran_amt { get; set; }

        [Display(Name ="備註")]
        public string? fact_memo { get; set; }
    
    }
}