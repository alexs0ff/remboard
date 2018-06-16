using System;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Заказ.
    /// </summary>
    public class RepairOrder:EntityBase<Guid>
    {
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

        /// <summary>
        /// Задает или получает код домена пользователей.
        /// </summary>
        public Guid? UserDomainID { get; set; }

        /// <summary>
        /// Задает или получает пароль доступа к заказу.
        /// </summary>
        public string AccessPassword { get; set; }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return RepairOrderID;
        }

        /// <summary>
        ///   Копирует поля указанного объекта в другой объект.
        /// </summary>
        /// <param name="entityBase"> Объект для копирования. </param>
        public override void CopyTo(EntityBase<Guid> entityBase)
        {
            var entity = (RepairOrder)entityBase;
            entity.BranchID = BranchID;
            entity.CallEventDate = CallEventDate;
            entity.ClientAddress = ClientAddress;
            entity.ClientEmail = ClientEmail;
            entity.ClientFullName = ClientFullName;
            entity.ClientPhone = ClientPhone;
            entity.DateOfBeReady = DateOfBeReady;
            entity.Defect = Defect;
            entity.DeviceAppearance = DeviceAppearance;
            entity.DeviceModel = DeviceModel;
            entity.DeviceSN = DeviceSN;
            entity.DeviceTitle = DeviceTitle;
            entity.DeviceTrademark = DeviceTrademark;
            entity.EventDate = EventDate;
            entity.GuidePrice = GuidePrice;
            entity.IsUrgent = IsUrgent;
            entity.IssueDate = IssueDate;
            entity.IssuerID = IssuerID;
            entity.ManagerID = ManagerID;
            entity.Notes = Notes;
            entity.Number = Number;
            entity.Options = Options;
            entity.OrderKindID = OrderKindID;
            entity.OrderStatusID = OrderStatusID;
            entity.PrePayment = PrePayment;
            entity.Recommendation = Recommendation;
            entity.RepairOrderID = RepairOrderID;
            entity.WarrantyTo = WarrantyTo;
            entity.UserDomainID = UserDomainID;
            entity.EngineerID = EngineerID;
            entity.AccessPassword = AccessPassword;
        }
    }
}
