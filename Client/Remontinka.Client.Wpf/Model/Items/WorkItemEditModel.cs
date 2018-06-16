using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Remontinka.Client.Wpf.Controllers.Items;
using Remontinka.Client.Wpf.Model.Controls;
using Remontinka.Client.Wpf.Model.Forms;

namespace Remontinka.Client.Wpf.Model.Items
{
    /// <summary>
    /// Модель редактирования выполненной работы.
    /// </summary>
    public class WorkItemEditModel : ViewModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает код связанного заказа.
        /// </summary>
        [ModelData]
        public Guid? RepairOrderID { get; set; }

        /// <summary>
        /// Задает или получает код инженера.
        /// </summary>
        [DisplayName("Инженер")]
        [ComboBoxControl(ControllerType = typeof(EngineerComboBoxController), AllowNull = false, ShowNullValue = true)]
        public Guid? WorkItemUserID { get; set; }

        /// <summary>
        /// Задает или получает наименование работы.
        /// </summary>
        [DisplayName("Наименование работы")]
        [RegexValue(Regex = ModelConstants.RequiredRegex)]
        public string WorkItemTitle { get; set; }

        /// <summary>
        /// Задает или получает дату выполненной работы.
        /// </summary>
        [DisplayName("Дата выполнения")]
        public DateTime WorkItemEventDate { get; set; }

        /// <summary>
        /// Задает или получает стоимость работ.
        /// </summary>
        [DisplayName("Стоимость")]
        [RegexValue(Regex = ModelConstants.RequiredRegex)]
        public string WorkItemPrice { get; set; }
    }
}
