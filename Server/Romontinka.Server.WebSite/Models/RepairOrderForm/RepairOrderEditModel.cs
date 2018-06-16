using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;

namespace Romontinka.Server.WebSite.Models.RepairOrderForm
{
    /// <summary>
    /// Модель редактирования данных о заказе.
    /// </summary>
    public class RepairOrderEditModel : JGridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает флаг указывающий, что пункт для инжинера.
        /// </summary>
        public bool IsItemForEngeener { get; set; }

        /// <summary>
        /// Задает или получает название выдавшего пользователя.
        /// </summary>
        public string IssuerFullName { get; set; }

        /// <summary>
        /// Задает или получает код статуса заказа.
        /// </summary>
        [DisplayName("Статус заказа")]
        [UIHint("AjaxComboBox")]
        [EditorHtmlClass("order-edit")]
        [LabelHtmlClass("order-label")]
        [Required]
        [AjaxComboBox("AjaxOrderStatus")]
        public Guid? RepairOrderStatusID { get; set; }
        
        /// <summary>
        /// Задает или получает рекомендации клиенту.
        /// </summary>
        [UIHint("MultilineString")]
        [DisplayName("Рекомендации клиенту")]
        [EditorHtmlClass("order-edit")]
        [LabelHtmlClass("order-label")]
        [MultilineString(2, 3)]
        public string Recommendation { get; set; }

        /// <summary>
        /// Задает или получает дату выдачи.
        /// </summary>
        [DisplayName("Дата выдачи")]
        [EditorHtmlClass("order-edit-small")]
        [LabelHtmlClass("order-label")]
        public DateTime? IssueDate { get; set; }

        /// <summary>
        /// Задает или срок гарантии.
        /// </summary>
        [DisplayName("Срок гарантии")]
        [EditorHtmlClass("order-edit-small")]
        [LabelHtmlClass("order-label-small")]
        public DateTime? WarrantyTo { get; set; }

        /// <summary>
        /// Задает или получает код назначенного инженера.
        /// </summary>
        [DisplayName("Инженер")]
        [UIHint("AjaxComboBox")]
        [EditorHtmlClass("order-edit-small")]
        [LabelHtmlClass("order-label-small")]
        [AjaxComboBox("AjaxEngineerComboBox")]
        public Guid? EngineerID { get; set; }

        /// <summary>
        /// Задает или получает код назначенного менеджера.
        /// </summary>
        [DisplayName("Менеджер")]
        [UIHint("AjaxComboBox")]
        [EditorHtmlClass("order-edit-small")]
        [LabelHtmlClass("order-label")]
        [Required]
        [AjaxComboBox("AjaxManagerComboBox")]
        public Guid? ManagerID { get; set; }

        /// <summary>
        /// Задает или получает код типа заказа.
        /// </summary>
        [DisplayName("Тип заказа")]
        [UIHint("AjaxComboBox")]
        [EditorHtmlClass("order-edit-small")]
        [LabelHtmlClass("order-label")]
        [Required]
        [AjaxComboBox("AjaxOrderKind")]
        public Guid? OrderKindID { get; set; }

        /// <summary>
        /// Задает или получает ФИО клиента.
        /// </summary>
        [Required]
        [DisplayName("ФИО клиента")]
        [EditorHtmlClass("order-edit")]
        [LabelHtmlClass("order-label")]
        public string ClientFullName { get; set; }

        /// <summary>
        /// Задает или получает адрес клиента.
        /// </summary>
        [UIHint("MultilineString")]
        [DisplayName("Адрес клиента")]
        [Required]
        [EditorHtmlClass("order-edit")]
        [LabelHtmlClass("order-label")]
        [MultilineString(2, 3)]
        public string ClientAddress { get; set; }

        /// <summary>
        /// Задает или получает телефон клиента.
        /// </summary>
        [Required]
        [DisplayName("Телефон клиента")]
        [EditorHtmlClass("order-edit-small")]
        [LabelHtmlClass("order-label")]
        public string ClientPhone { get; set; }

        /// <summary>
        /// Задает или получает email клиента.
        /// </summary>
        [DisplayName("Email клиента")]
        [EditorHtmlClass("order-edit-small")]
        [LabelHtmlClass("order-label-small")]
        public string ClientEmail { get; set; }

        /// <summary>
        /// Задает или получает название устройства.
        /// </summary>
        [Required]
        [DisplayName("Устройство")]
        [EditorHtmlClass("order-edit")]
        [LabelHtmlClass("order-label")]
        public string DeviceTitle { get; set; }

        /// <summary>
        /// Задает или получает серийный номер устройства.
        /// </summary> 
        [Required]
        [DisplayName("Серийный номер")]
        [EditorHtmlClass("order-edit-small")]
        [LabelHtmlClass("order-label")]
        public string DeviceSN { get; set; }

        /// <summary>
        /// Задает или получает марку устройства.
        /// </summary>
        [Required]
        [DisplayName("Бренд")]
        [EditorHtmlClass("order-edit-small")]
        [LabelHtmlClass("order-label-small")]
        public string DeviceTrademark { get; set; }

        /// <summary>
        /// Задает или получает модель устройства.
        /// </summary>
        [Required]
        [DisplayName("Модель")]
        [EditorHtmlClass("order-edit")]
        [LabelHtmlClass("order-label")]
        public string DeviceModel { get; set; }

        /// <summary>
        /// Задает или получает описание неисправностей.
        /// </summary>
        [UIHint("MultilineString")]
        [DisplayName("Неисправности")]
        [EditorHtmlClass("order-edit")]
        [LabelHtmlClass("order-label")]
        [MultilineString(2, 3)]
        [Required]
        public string Defect { get; set; }

        /// <summary>
        /// Задает или получает комплектацию устройства.
        /// </summary>
        [DisplayName("Комплектация")]
        [EditorHtmlClass("order-edit")]
        [LabelHtmlClass("order-label")]
        public string Options { get; set; }

        /// <summary>
        /// Задает или получает внешний вид устройства.
        /// </summary>
        [DisplayName("Внешний вид")]
        [EditorHtmlClass("order-edit")]
        [LabelHtmlClass("order-label")]
        public string DeviceAppearance { get; set; }

        /// <summary>
        /// Задает или получает заметки приемщика.
        /// </summary>
        [DisplayName("Заметки")]
        [UIHint("MultilineString")]
        [EditorHtmlClass("order-edit")]
        [LabelHtmlClass("order-label")]
        [MultilineString(3, 3)]
        public string Notes { get; set; }

        /// <summary>
        /// Задает или получает дату и время вызова мастера.
        /// </summary>
        [DisplayName("Дата составления")]
        [EditorHtmlClass("order-edit-small")]
        [LabelHtmlClass("order-label")]
        public DateTime? CallEventDate { get; set; }

        /// <summary>
        /// Задает или получает ориентировачную дату выдачу.
        /// </summary>
        [DisplayName("Дата готовности")]
        [EditorHtmlClass("order-edit-small")]
        [LabelHtmlClass("order-label-small")]
        [Required]
        public DateTime DateOfBeReady { get; set; }

        /// <summary>
        /// Задает или получает ориентировочную цену.
        /// </summary>
        [UIHint("Decimal")]
        [DisplayName("Пред. цена")]
        [EditorHtmlClass("order-edit-small")]
        [LabelHtmlClass("order-label")]
        public decimal? GuidePrice { get; set; }

        /// <summary>
        /// Задает или получает аванс.
        /// </summary>
        [UIHint("Decimal")]
        [DisplayName("Аванс")]
        [EditorHtmlClass("order-edit-small")]
        [LabelHtmlClass("order-label-small")]
        public decimal? PrePayment { get; set; }

        /// <summary>
        /// Задает или получает срочность.
        /// </summary>
        [UIHint("Boolean")]
        [DisplayName("Срочный")]
        [EditorHtmlClass("order-edit")]
        [LabelHtmlClass("order-label")]
        public bool IsUrgent { get; set; }

        /// <summary>
        /// Задает или получает код филиала.
        /// </summary>
        [DisplayName("Филиал")]
        [UIHint("AjaxComboBox")]
        [EditorHtmlClass("order-edit-small")]
        [LabelHtmlClass("order-label-small")]
        [Required]
        [AjaxComboBox("AjaxBranchComboBox")]
        public Guid? BranchID { get; set; }
    }
}