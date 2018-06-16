using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Core.Context
{
    /// <summary>
    /// Константы контекста.
    /// </summary>
    public static class ContextConstants
    {
        #region Embedded tables

        /// <summary>
        /// Список установленных деталей и выполненных работ с итогом.
        /// </summary>
        public const string DeviceAndWorkItemsList = "DeviceAndWorkItemsList";

        /// <summary>
        /// Товарный чек таблица.
        /// </summary>
        public const string ProductList = "ProductList";

        #endregion Embedded tables

        #region Barcodes 

        /// <summary>
        /// Содержит параметр который указывает на штрих код в формате Code128 содержащий номер заказа.
        /// </summary>
        public const string RepairOrderNumberCode128 = "RepairOrderNumberCode128";

        #endregion Barcodes

        /// <summary>
        /// Содержит параметр который указывает на сегодняшний день.
        /// </summary>
        public const string Today = "Today";

        /// <summary>
        /// Содержит параметр который указывает на название филиала.
        /// </summary>
        public const string BranchTitle = "BranchTitle";

        /// <summary>
        /// Содержит параметр который указывает на юр название филиала.
        /// </summary>
        public const string BranchLegalName = "BranchLegalName";

        /// <summary>
        /// Содержит параметр который указывает на адрес филиала.
        /// </summary>
        public const string BranchAddress = "BranchAddress";

        /// <summary>
        /// Содержит параметр который указывает на ФИО клиента.
        /// </summary>
        public const string ClientFullName = "ClientFullName";

        /// <summary>
        /// Содержит параметр который указывает на телефон клиента.
        /// </summary>
        public const string ClientPhone = "ClientPhone";

        /// <summary>
        /// Содержит параметр который указывает на дату готовности.
        /// </summary>
        public const string DateOfBeReady = "DateOfBeReady";

        /// <summary>
        /// Содержит параметр который указывает на неисправности.
        /// </summary>
        public const string Defect = "Defect";

        /// <summary>
        /// Содержит параметр который указывает на внешний вид устройства.
        /// </summary>
        public const string DeviceAppearance = "DeviceAppearance";

        /// <summary>
        /// Содержит параметр который указывает на модель устройства.
        /// </summary>
        public const string DeviceModel = "DeviceModel";

        /// <summary>
        /// Содержит параметр который указывает на серийный номер устройства.
        /// </summary>
        public const string DeviceSN = "DeviceSN";

        /// <summary>
        /// Содержит параметр который указывает на название устройства.
        /// </summary>
        public const string DeviceTitle = "DeviceTitle";

        /// <summary>
        /// Содержит параметр который указывает на торговую марку устройства.
        /// </summary>
        public const string DeviceTrademark = "DeviceTrademark";

        /// <summary>
        /// Содержит параметр который указывает на ФИО инженера.
        /// </summary>
        public const string EngineerFullName = "EngineerFullName";

        /// <summary>
        /// Содержит параметр который указывает на дату заказа.
        /// </summary>
        public const string EventDate = "EventDate";

        /// <summary>
        /// Содержит параметр который указывает на ориентировочную цену.
        /// </summary>
        public const string GuidePrice = "GuidePrice";

        /// <summary>
        /// Содержит параметр который указывает срочность заказа.
        /// </summary>
        public const string IsUrgent = "IsUrgent";

        /// <summary>
        /// Содержит параметр который указывает на дату выдачи клиенту.
        /// </summary>
        public const string IssueDate = "IssueDate";

        /// <summary>
        /// Содержит параметр который указывает на ФИО менеджера.
        /// </summary>
        public const string ManagerFullName = "ManagerFullName";

        /// <summary>
        /// Содержит параметр который указывает на заметки приемщика.
        /// </summary>
        public const string Notes = "Notes";

        /// <summary>
        /// Содержит параметр который указывает на номер заказа.
        /// </summary>
        public const string Number = "Number";

        /// <summary>
        /// Содержит параметр который указывает на комплектацию заказа.
        /// </summary>
        public const string Options = "Options";

        /// <summary>
        /// Содержит параметр который указывает на название типа заказа.
        /// </summary>
        public const string OrderKindTitle = "OrderKindTitle";

        /// <summary>
        /// Содержит параметр который указывает на название статуса заказа.
        /// </summary>
        public const string OrderStatusTitle = "OrderStatusTitle";

        /// <summary>
        /// Содержит параметр который указывает на аванс по заказу.
        /// </summary>
        public const string PrePayment = "PrePayment";

        /// <summary>
        /// Содержит параметр который указывает на рекомендации клиенту по заказу.
        /// </summary>
        public const string Recommendation = "Recommendation";

        /// <summary>
        /// Содержит параметр который указывает на срок окончания гарантии.
        /// </summary>
        public const string WarrantyTo = "WarrantyTo";

        /// <summary>
        /// Содержит параметр который указывает на общее количество работ и запчастей.
        /// </summary>
        public const string TotalItemsCount = "TotalItemsCount";

        /// <summary>
        /// Содержит параметр который указывает на сумму по работам и запчастям.
        /// </summary>
        public const string TotalItemsAmount = "TotalItemsAmount";

        /// <summary>
        /// Содержит параметр который указывает на сумму по работам и запчастям в прописном формате.
        /// </summary>
        public const string TotalItemsAmountRus = "TotalItemsAmountRus";

        /// <summary>
        /// Содержит параметр который указывает на глобальную ссылку на заказ, в не зависимости от домена.
        /// </summary>
        public const string DomainRepairOrderNumber = "DomainRepairOrderNumber";

        /// <summary>
        /// Содержит параметр который указывает на пароль к глобальной ссылке на заказ.
        /// </summary>
        public const string DomainRepairOrderPassword = "DomainRepairOrderPassword";
    }
}
