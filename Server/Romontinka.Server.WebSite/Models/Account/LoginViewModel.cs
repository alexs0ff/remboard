using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Romontinka.Server.WebSite.Models.Account
{
    public class LoginViewModel
    {
        /// <summary>
        /// ��� ������������.
        /// </summary>
        [Required]
        [DisplayName("�����")]
        public string UserName { get; set; }

        /// <summary>
        /// ������.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [DisplayName("������")]
        public string Password { get; set; }

        /// <summary>
        /// ��������� �����������.
        /// </summary>
        [Required]
        [DisplayName("���������")]
        public bool RememberMe { get; set; }

    }
}