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
    /// Модель установленной запчасти.
    /// </summary>
    public class DeviceItemEditModel : ViewModelBase<Guid>
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
        public Guid? DeviceItemUserID { get; set; }

        /// <summary>
        /// Задает или получает дату выполненной работы.
        /// </summary>
        [DisplayName("Дата")]
        public DateTime DeviceItemEventDate { get; set; }

        /// <summary>
        /// Наименование запчасти.
        /// </summary>
        [DisplayName("Наименование запчасти")]
        public string DeviceItemTitle { get; set; }

        /// <summary>
        /// Задает или получает количество запчастей.
        /// </summary>
        [DisplayName("Количество")]
        [RegexValue(Regex = ModelConstants.RequiredRegex)]
        public string DeviceItemCount { get; set; }

        /// <summary>
        /// Задает или получает себестоимость.
        /// </summary>
        [DisplayName("Себестоимость")]
        [RegexValue(Regex = ModelConstants.RequiredRegex)]
        public string DeviceItemCostPrice { get; set; }

        /// <summary>
        /// Задает или получает окончательную цену.
        /// </summary>
        [DisplayName("Цена")]
        [RegexValue(Regex = ModelConstants.RequiredRegex)]
        public string DeviceItemPrice { get; set; }

        /// <summary>
        /// Задает или получает код номенклатуры.
        /// </summary>
        [DisplayName("Деталь со склада")]
        [ComboBoxControl(ControllerType = typeof(WarehouseItemComboboxController), AllowNull = true, ShowNullValue = true)]
        public Guid? WarehouseItemID { get; set; }
    }
}
