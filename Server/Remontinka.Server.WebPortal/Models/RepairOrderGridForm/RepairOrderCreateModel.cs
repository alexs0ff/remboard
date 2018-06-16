using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using Remontinka.Server.WebPortal.Models.Common;

namespace Remontinka.Server.WebPortal.Models.RepairOrderGridForm
{
    /// <summary>
    /// Модель для создания заказа.
    /// </summary>
    public class RepairOrderCreateModel : GridDataModelBase<Guid>
    {

        /// <summary>
        /// Задает или получает уникальный номер заказа.
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Задает или получает код назначенного инженера.
        /// </summary>
        [DisplayName("Инженер")]
        public Guid? EngineerID { get; set; }

        /// <summary>
        /// Задает или получает код назначенного менеджера.
        /// </summary>
        [DisplayName("Менеджер")]
        [Required]
        public Guid? ManagerID { get; set; }

        /// <summary>
        /// Задает или получает код типа заказа.
        /// </summary>
        [DisplayName("Тип заказа")]
        [Required]
        public Guid? OrderKindID { get; set; }

        /// <summary>
        /// Задает или получает ФИО клиента.
        /// </summary>
        [Required]
        [DisplayName("ФИО клиента")]
        public string ClientFullName { get; set; }

        /// <summary>
        /// Задает или получает адрес клиента.
        /// </summary>
        [DisplayName("Адрес клиента")]
        [Required]
        public string ClientAddress { get; set; }

        /// <summary>
        /// Задает или получает телефон клиента.
        /// </summary>
        [Required]
        [DisplayName("Телефон клиента")]
        [Mask("+0(000) 000-0000", IncludeLiterals = MaskIncludeLiteralsMode.None, ErrorMessage = "Ошибочный телефонный номер")]
        public string ClientPhone { get; set; }

        /// <summary>
        /// Задает или получает email клиента.
        /// </summary>
        [DisplayName("Email клиента")]
        [EmailAddress(ErrorMessage = "Ошибочный email")]
        public string ClientEmail { get; set; }

        /// <summary>
        /// Задает или получает название устройства.
        /// </summary>
        [Required]
        [DisplayName("Название")]
        public string DeviceTitle { get; set; }

        /// <summary>
        /// Задает или получает серийный номер устройства.
        /// </summary> 
        [Required]
        [DisplayName("Серийный номер")]
        public string DeviceSN { get; set; }

        /// <summary>
        /// Задает или получает марку устройства.
        /// </summary>
        [Required]
        [DisplayName("Бренд")]
        public string[] DeviceTrademark { get; set; }

        /// <summary>
        /// Задает или получает модель устройства.
        /// </summary>
        [Required]
        [DisplayName("Модель")]
        public string DeviceModel { get; set; }

        /// <summary>
        /// Задает или получает описание неисправностей.
        /// </summary>
        [DisplayName("Неисправности")]
        [Required]
        public string Defect { get; set; }

        /// <summary>
        /// Задает или получает комплектацию устройства.
        /// </summary>
        [DisplayName("Комплектация")]
        public string[] Options { get; set; }

        /// <summary>
        /// Задает или получает внешний вид устройства.
        /// </summary>
        [DisplayName("Внешний вид")]
        public string[] DeviceAppearance
        {
            get; set;
        }
        
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
        [Required]
        public DateTime DateOfBeReady { get; set; }

        /// <summary>
        /// Задает или получает ориентировочную цену.
        /// </summary>
        [DisplayName("Пред. цена")]
        public decimal? GuidePrice { get; set; }

        /// <summary>
        /// Задает или получает аванс.
        /// </summary>
        [DisplayName("Аванс")]
        public decimal? PrePayment { get; set; }

        /// <summary>
        /// Задает или получает срочность.
        /// </summary>
        [DisplayName("Срочный")]
        public bool IsUrgent { get; set; }

        /// <summary>
        /// Задает или получает код филиала.
        /// </summary>
        [DisplayName("Филиал")]
        [Required]
        public Guid? BranchID { get; set; }

        /// <summary>
        /// Получает идентификатор.
        /// </summary>
        public override Guid? GetId()
        {
            return Guid.Empty;
        }
    }
}