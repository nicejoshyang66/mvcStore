using System.ComponentModel.DataAnnotations;

namespace mvc_Dapper.Models.Vm
{
    public class ChangePasswordVM
    {
        public int rowid { get; set; }

        public string? code_user { get; set; }

        [Required(ErrorMessage = "請輸入新密碼")]
        [Display(Name = "新密碼")]
        public string? NewPassword { get; set; }

        [Required(ErrorMessage = "請再次輸入密碼")]
        [Compare("NewPassword", ErrorMessage = "兩次輸入的密碼不一致")]
        [Display(Name = "確認密碼")]
        public string? ConfirmPassword { get; set; }
    }
}