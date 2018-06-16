using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Romontinka.Server.WebSite.Models.Account
{
    /// <summary>
    /// ������ ��� ����� ����� ������.
    /// </summary>
    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "������� ������")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "������ {0} ������ ���� �� ������  {2} ��������.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "����� ������")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "����������� ������")]
        [Compare("NewPassword", ErrorMessage = "������ ������ ���� �����������")]
        public string ConfirmPassword { get; set; }
    }
}