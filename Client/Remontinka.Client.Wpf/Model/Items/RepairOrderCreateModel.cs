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
    /// Модель создания заказа.
    /// </summary>
    public class RepairOrderCreateModel : ViewModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает уникальный номер заказа.
        /// </summary>
        [ModelData]
        public string Number { get; set; }

        /// <summary>
        /// Задает или получает код назначенного инженера.
        /// </summary>
        [DisplayName("Инженер")]
        [ComboBoxControl(ControllerType = typeof(EngineerComboBoxController), AllowNull = true, ShowNullValue = true)]
        public Guid? EngineerID { get; set; }

        /// <summary>
        /// Задает или получает код назначенного менеджера.
        /// </summary>
        [DisplayName("Менеджер")]
        [ComboBoxControl(ControllerType = typeof(ManagerComboBoxController), AllowNull = false, ShowNullValue = true)]
        public Guid? ManagerID { get; set; }

        /// <summary>
        /// Задает или получает код типа заказа.
        /// </summary>
        [DisplayName("Тип заказа")]
        [ComboBoxControl(ControllerType = typeof(OrderKindController), AllowNull = false, ShowNullValue = true)]
        public Guid? OrderKindID { get; set; }

        /// <summary>
        /// Задает или получает ФИО клиента.
        /// </summary>
        [DisplayName("ФИО клиента")]
        [RegexValue(Regex = ModelConstants.RequiredRegex)]
        public string ClientFullName { get; set; }

        /// <summary>
        /// Задает или получает адрес клиента.
        /// </summary>
        [DisplayName("Адрес клиента")]
        [RegexValue(Regex = ModelConstants.RequiredRegex)]
        public string ClientAddress { get; set; }

        /// <summary>
        /// Задает или получает телефон клиента.
        /// </summary>
        [RegexValue(Regex = ModelConstants.RequiredRegex)]
        [DisplayName("Телефон клиента")]
        public string ClientPhone { get; set; }

        /// <summary>
        /// Задает или получает email клиента.
        /// </summary>
        [DisplayName("Email клиента")]
        public string ClientEmail { get; set; }

        /// <summary>
        /// Задает или получает название устройства.
        /// </summary>
        [RegexValue(Regex = ModelConstants.RequiredRegex)]
        [DisplayName("Устройство")]
        public string DeviceTitle { get; set; }

        /// <summary>
        /// Задает или получает серийный номер устройства.
        /// </summary> 
        [RegexValue(Regex = ModelConstants.RequiredRegex)]
        [DisplayName("Серийный номер")]
        public string DeviceSN { get; set; }

        /// <summary>
        /// Задает или получает марку устройства.
        /// </summary>
        [RegexValue(Regex = ModelConstants.RequiredRegex)]
        [DisplayName("Бренд")]
        public string DeviceTrademark { get; set; }

        /// <summary>
        /// Задает или получает модель устройства.
        /// </summary>
        [RegexValue(Regex = ModelConstants.RequiredRegex)]
        [DisplayName("Модель")]
        public string DeviceModel { get; set; }

        /// <summary>
        /// Задает или получает описание неисправностей.
        /// </summary>
        [DisplayName("Неисправности")]
        [RegexValue(Regex = ModelConstants.RequiredRegex)]
        public string Defect { get; set; }

        /// <summary>
        /// Задает или получает комплектацию устройства.
        /// </summary>
        [DisplayName("Комплектация")]
        public string Options { get; set; }

        /// <summary>
        /// Задает или получает внешний вид устройства.
        /// </summary>
        [DisplayName("Внешний вид")]
        public string DeviceAppearance { get; set; }

        /// <summary>
        /// Задает или получает заметки приемщика.
        /// </summary>
        [DisplayName("Заметки")]
        public string Notes { get; set; }

        /// <summary>
        /// Задает или получает дату и время вызова мастера.
        /// </summary>
        [DisplayName("Дата составления")]
        public DateTime? CallEventDate { get; set; }

        /// <summary>
        /// Задает или получает ориентировачную дату выдачу.
        /// </summary>
        [DisplayName("Дата готовности")]
        public DateTime DateOfBeReady { get; set; }

        /// <summary>
        /// Задает или получает ориентировочную цену.
        /// </summary>
        [DisplayName("Пред. цена")]
        public string GuidePrice { get; set; }

        /// <summary>
        /// Задает или получает аванс.
        /// </summary>
        [DisplayName("Аванс")]
        public string PrePayment { get; set; }

        /// <summary>
        /// Задает или получает срочность.
        /// </summary>
        [DisplayName("Срочный")]
        public bool IsUrgent { get; set; }

        /// <summary>
        /// Задает или получает код филиала.
        /// </summary>
        [DisplayName("Филиал")]
        [ComboBoxControl(ControllerType = typeof(BranchComboBoxController), AllowNull = false, ShowNullValue = true)]
        public Guid? BranchID { get; set; }
    }
}
