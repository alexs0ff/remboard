using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol.SynchronizationMessages
{
    /// <summary>
    /// DTO объект для объектов заказа.
    /// </summary>
    public class RepairOrderDTO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public RepairOrderDTO()
        {
            OrderTimelines = new List<OrderTimelineDTO>();
            WorkItems = new List<WorkItemDTO>();
            DeviceItems = new List<DeviceItemDTO>();
        }

        /// <summary>
        /// Получает список истории изменений заказа.
        /// </summary>
        public List<OrderTimelineDTO> OrderTimelines { get; private set; }

        /// <summary>
        /// Получает список выполненных работ.
        /// </summary>
        public List<WorkItemDTO> WorkItems { get; private set; }

        /// <summary>
        /// Получает список установленных запчастей.
        /// </summary>
        public List<DeviceItemDTO> DeviceItems { get; private set; }

        /// <summary>
        /// Задает или получает код заказа.
        /// </summary>
        public Guid? RepairOrderID { get; set; }

        /// <summary>
        /// Задает или получает код выдавшего пользователя.
        /// </summary>
        public Guid? IssuerID { get; set; }

        /// <summary>
        /// Задает или получает код статуса заказа.
        /// </summary>
        public Guid? OrderStatusID { get; set; }

        /// <summary>
        /// Задает или получает код назначенного инженера.
        /// </summary>
        public Guid? EngineerID { get; set; }

        /// <summary>
        /// Задает или получает код назначенного менеджера.
        /// </summary>
        public Guid? ManagerID { get; set; }

        /// <summary>
        /// Задает или получает код типа заказа.
        /// </summary>
        public Guid? OrderKindID { get; set; }

        /// <summary>
        /// Задает или получает дату и время заказа.
        /// </summary>
        public DateTime EventDate { get; set; }

        /// <summary>
        /// Задает или получает уникальный номер заказа.
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Задает или получает ФИО клиента.
        /// </summary>
        public string ClientFullName { get; set; }

        /// <summary>
        /// Задает или получает адрес клиента.
        /// </summary>
        public string ClientAddress { get; set; }

        /// <summary>
        /// Задает или получает телефон клиента.
        /// </summary>
        public string ClientPhone { get; set; }

        /// <summary>
        /// Задает или получает email клиента.
        /// </summary>
        public string ClientEmail { get; set; }

        /// <summary>
        /// Задает или получает название устройства.
        /// </summary>
        public string DeviceTitle { get; set; }

        /// <summary>
        /// Задает или получает серийный номер устройства.
        /// </summary> 
        public string DeviceSN { get; set; }

        /// <summary>
        /// Задает или получает марку устройства.
        /// </summary>
        public string DeviceTrademark { get; set; }

        /// <summary>
        /// Задает или получает модель устройства.
        /// </summary>
        public string DeviceModel { get; set; }

        /// <summary>
        /// Задает или получает описание неисправностей.
        /// </summary>
        public string Defect { get; set; }

        /// <summary>
        /// Задает или получает комплектацию устройства.
        /// </summary>
        public string Options { get; set; }

        /// <summary>
        /// Задает или получает внешний вид устройства.
        /// </summary>
        public string DeviceAppearance { get; set; }

        /// <summary>
        /// Задает или получает заметки приемщика.
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Задает или получает дату и время вызова мастера.
        /// </summary>
        public DateTime? CallEventDate { get; set; }

        /// <summary>
        /// Задает или получает ориентировачную дату выдачу.
        /// </summary>
        public DateTime DateOfBeReady { get; set; }

        /// <summary>
        /// Задает или получает ориентировочную цену.
        /// </summary>
        public decimal? GuidePrice { get; set; }

        /// <summary>
        /// Задает или получает аванс.
        /// </summary>
        public decimal? PrePayment { get; set; }

        /// <summary>
        /// Задает или получает срочность.
        /// </summary>
        public bool IsUrgent { get; set; }

        /// <summary>
        /// Задает или получает рекомендации клиенту.
        /// </summary>
        public string Recommendation { get; set; }

        /// <summary>
        /// Задает или получает дату выдачи.
        /// </summary>
        public DateTime? IssueDate { get; set; }

        /// <summary>
        /// Задает или срок гарантии.
        /// </summary>
        public DateTime? WarrantyTo { get; set; }

        /// <summary>
        /// Задает или получает код филиала.
        /// </summary>
        public Guid? BranchID { get; set; }
    }
}
