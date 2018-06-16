using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;
using Romontinka.Server.Core.Security;

namespace Remontinka.Server.WebPortal.Models.SystemForm
{
    /// <summary>
    /// Модель для настроек интерфейса.
    /// </summary>
    public class UserInterfaceModel
    {
        private static readonly List<ThemeComboBoxItem> _avalibleThemes = new List<ThemeComboBoxItem>
        {
            new ThemeComboBoxItem("Default","Default"),
            new ThemeComboBoxItem("Aqua","Aqua"),
            new ThemeComboBoxItem("BlackGlass","BlackGlass"),
            new ThemeComboBoxItem("DevEx","DevEx"),
            new ThemeComboBoxItem("Glass","Glass"),
            new ThemeComboBoxItem("Metropolis","Metropolis"),
            new ThemeComboBoxItem("MetropolisBlue","MetropolisBlue"),
            new ThemeComboBoxItem("Office2003Blue","Office2003Blue"),
            new ThemeComboBoxItem("Office2003Olive","Office2003Olive"),
            new ThemeComboBoxItem("Office2003Silver","Office2003Silver"),
            new ThemeComboBoxItem("Office2010Black","Office2010Black"),
            new ThemeComboBoxItem("Office2010Blue","Office2010Blue"),
            new ThemeComboBoxItem("Office2010Silver","Office2010Silver"),
            new ThemeComboBoxItem("PlasticBlue","PlasticBlue"),
            new ThemeComboBoxItem("RedWine","RedWine"),
            new ThemeComboBoxItem("SoftOrange","SoftOrange"),
            new ThemeComboBoxItem("Youthful","Youthful"),
            new ThemeComboBoxItem("iOS","iOS"),
            new ThemeComboBoxItem("Material","Material"),
            new ThemeComboBoxItem("Moderno","Moderno"),
            new ThemeComboBoxItem("Mulberry","Mulberry"),
        };

        /// <summary>
        /// Получает доступные темы.
        /// </summary>
        public List<ThemeComboBoxItem> Themes{ get { return _avalibleThemes; } }

        /// <summary>
        /// Задает или получает текущую тему.
        /// </summary>
        [DisplayName("Тема")]
        [Required]
        public string CurrentTheme { get; set; }

        /// <summary>
        /// Задает или получает текущий токен.
        /// </summary>
        public SecurityToken Token { get; set; }
    }
}