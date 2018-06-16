using System;

namespace Remontinka.Client.DataLayer.Entities
{
    /// <summary>
    /// Заказ.
    /// </summary>
    public class RepairOrder:EntityBase<Guid>
    {
        private string _repairOrderID;

        /// <summary>
        /// Задает или получает код заказа.
        /// </summary>
        public string RepairOrderID
        {
            get { return _repairOrderID; }
            set { FormatUtils.ExchangeFields(ref _repairOrderIDGuid, ref _repairOrderID, value); }
        }

        private Guid? _repairOrderIDGuid;

        /// <summary>
        /// Задает или получает код заказа.
        /// </summary>
        public Guid? RepairOrderIDGuid
        {
            get { return _repairOrderIDGuid; }
            set
            {
                FormatUtils.ExchangeFields(ref _repairOrderIDGuid, ref _repairOrderID, value);
            }
        }

        private string _issuerID;

        /// <summary>
        /// Задает или получает код выдавшего пользователя.
        /// </summary>
        public string IssuerID
        {
            get { return _issuerID; }
            set { FormatUtils.ExchangeFields(ref _issuerIDGuid, ref _issuerID, value); }
        }

        private Guid? _issuerIDGuid;

        /// <summary>
        /// Задает или получает код выдавшего пользователя.
        /// </summary>
        public Guid? IssuerIDGuid
        {
            get { return _issuerIDGuid; }
            set
            {
                FormatUtils.ExchangeFields(ref _issuerIDGuid, ref _issuerID, value);
            }
        }

        private string _orderStatusID;

        /// <summary>
        /// Задает или получает код статуса заказа.
        /// </summary>
        public string OrderStatusID
        {
            get { return _orderStatusID; }
            set { FormatUtils.ExchangeFields(ref _orderStatusIDGuid, ref _orderStatusID, value); }
        }

        private Guid? _orderStatusIDGuid;

        /// <summary>
        /// Задает или получает код статуса заказа.
        /// </summary>
        public Guid? OrderStatusIDGuid
        {
            get { return _orderStatusIDGuid; }
            set
            {
                FormatUtils.ExchangeFields(ref _orderStatusIDGuid, ref _orderStatusID, value);
            }
        }

        private string _engineerID;

        /// <summary>
        /// Задает или получает код назначенного инженера.
        /// </summary>
        public string EngineerID
        {
            get { return _engineerID; }
            set { FormatUtils.ExchangeFields(ref _engineerIDGuid, ref _engineerID, value); }
        }

        private Guid? _engineerIDGuid;

        /// <summary>
        /// Задает или получает код назначенного инженера.
        /// </summary>
        public Guid? EngineerIDGuid
        {
            get { return _engineerIDGuid; }
            set
            {
                FormatUtils.ExchangeFields(ref _engineerIDGuid, ref _engineerID, value);
            }
        }

        private string _managerID;

        /// <summary>
        /// Задает или получает код назначенного менеджера.
        /// </summary>
        public string ManagerID
        {
            get { return _managerID; }
            set { FormatUtils.ExchangeFields(ref _managerIDGuid, ref _managerID, value); }
        }

        private Guid? _managerIDGuid;

        /// <summary>
        /// Задает или получает код назначенного менеджера.
        /// </summary>
        public Guid? ManagerIDGuid
        {
            get { return _managerIDGuid; }
            set
            {
                FormatUtils.ExchangeFields(ref _managerIDGuid, ref _managerID, value);
            }
        }

        private string _orderKindID;

        /// <summary>
        /// Задает или получает код типа заказа.
        /// </summary>
        public string OrderKindID
        {
            get { return _orderKindID; }
            set
            {
                FormatUtils.ExchangeFields(ref _orderKindIDGuid, ref _orderKindID, value);
            }
        }

        private Guid? _orderKindIDGuid;

        /// <summary>
        /// Задает или получает код типа заказа.
        /// </summary>
        public Guid? OrderKindIDGuid
        {
            get { return _orderKindIDGuid; }
            set
            {
                FormatUtils.ExchangeFields(ref _orderKindIDGuid, ref _orderKindID, value);
            }
        }

        private string _eventDate;

        /// <summary>
        /// Задает или получает дату и время заказа.
        /// </summary>
        public string EventDate
        {
            get { return _eventDate; }
            set { FormatUtils.ExchangeFields(ref _eventDateDateTime, ref _eventDate, value); }
        }

        private DateTime _eventDateDateTime;

        /// <summary>
        /// Задает или получает дату и время заказа.
        /// </summary>
        public DateTime EventDateDateTime
        {
            get { return _eventDateDateTime; }
            set
            {
                FormatUtils.ExchangeFields(ref _eventDateDateTime,ref _eventDate,value);
            }
        }

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

        private string _callEventDate;

        /// <summary>
        /// Задает или получает дату и время вызова мастера.
        /// </summary>
        public string CallEventDate
        {
            get { return _callEventDate; }
            set { FormatUtils.ExchangeFields(ref _callEventDateDateTime, ref _callEventDate, value); }
        }

        private DateTime? _callEventDateDateTime;

        /// <summary>
        /// Задает или получает дату и время вызова мастера.
        /// </summary>
        public DateTime? CallEventDateDateTime
        {
            get { return _callEventDateDateTime; }
            set
            {
                FormatUtils.ExchangeFields(ref _callEventDateDateTime, ref _callEventDate,value);
            }
        }

        private string _dateOfBeReady;

        /// <summary>
        /// Задает или получает ориентировачную дату выдачу.
        /// </summary>
        public string DateOfBeReady
        {
            get { return _dateOfBeReady; }
            set { FormatUtils.ExchangeFields(ref _dateOfBeReadyDateTime, ref _dateOfBeReady, value); }
        }

        private DateTime _dateOfBeReadyDateTime;

        /// <summary>
        /// Задает или получает ориентировачную дату выдачу.
        /// </summary>
        public DateTime DateOfBeReadyDateTime
        {
            get { return _dateOfBeReadyDateTime; }
            set
            {
                FormatUtils.ExchangeFields(ref _dateOfBeReadyDateTime, ref _dateOfBeReady, value);
            }
        }

        /// <summary>
        /// Задает или получает ориентировочную цену.
        /// </summary>
        public double? GuidePrice { get; set; }

        /// <summary>
        /// Задает или получает аванс.
        /// </summary>
        public double? PrePayment { get; set; }

        private long _isUrgent;

        /// <summary>
        /// Задает или получает срочность.
        /// </summary>
        public long IsUrgent
        {
            get { return _isUrgent; }
            set { FormatUtils.ExchangeFields(ref _isUrgentBoolean, ref _isUrgent, value); }
        }

        private bool _isUrgentBoolean;

        /// <summary>
        /// Задает или получает срочность.
        /// </summary>
        public bool IsUrgentBoolean
        {
            get { return _isUrgentBoolean; }
            set
            {
                FormatUtils.ExchangeFields(ref _isUrgentBoolean, ref _isUrgent, value);
            }
        }

        /// <summary>
        /// Задает или получает рекомендации клиенту.
        /// </summary>
        public string Recommendation { get; set; }

        private string _issueDate;

        /// <summary>
        /// Задает или получает дату выдачи.
        /// </summary>
        public string IssueDate
        {
            get { return _issueDate; }
            set { FormatUtils.ExchangeFields(ref _issueDateDateTime, ref _issueDate, value); }
        }

        private DateTime? _issueDateDateTime;

        /// <summary>
        /// Задает или получает дату выдачи.
        /// </summary>
        public DateTime? IssueDateDateTime
        {
            get { return _issueDateDateTime; }
            set
            {
                FormatUtils.ExchangeFields(ref _issueDateDateTime, ref _issueDate,value);
            }
        }

        private string _warrantyTo;

        /// <summary>
        /// Задает или срок гарантии.
        /// </summary>
        public string WarrantyTo
        {
            get { return _warrantyTo; }
            set { FormatUtils.ExchangeFields(ref _warrantyToDateTime, ref _warrantyTo, value); }
        }

        private DateTime? _warrantyToDateTime;

        /// <summary>
        /// Задает или срок гарантии.
        /// </summary>
        public DateTime? WarrantyToDateTime
        {
            get { return _warrantyToDateTime; }
            set
            {
                FormatUtils.ExchangeFields(ref _warrantyToDateTime, ref _warrantyTo,value);
            }
        }

        private string _branchID;

        /// <summary>
        /// Задает или получает код филиала.
        /// </summary>
        public string BranchID
        {
            get { return _branchID; }
            set { FormatUtils.ExchangeFields(ref _branchIDGuid, ref _branchID, value); }
        }

        private Guid? _branchIDGuid;

        /// <summary>
        /// Задает или получает код филиала.
        /// </summary>
        public Guid? BranchIDGuid
        {
            get { return _branchIDGuid; }
            set
            {
                FormatUtils.ExchangeFields(ref _branchIDGuid, ref _branchID,value);
            }
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return RepairOrderIDGuid;
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
            entity.EngineerID = EngineerID;
        }
    }
}
