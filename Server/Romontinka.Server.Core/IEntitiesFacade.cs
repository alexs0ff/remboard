using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.DataLayer.Entities.ReportItems;

namespace Romontinka.Server.Core
{
    /// <summary>
    /// Интерефейс к сервису доступа к данным
    /// </summary>
    public interface IEntitiesFacade
    {
        /// <summary>
        /// Получает заказ руководствуясь привелегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен пользователя.</param>
        /// <param name="orderID">Код заказа.</param>
        /// <returns>Заказ.</returns>
        RepairOrderDTO GetOrder(SecurityToken token,Guid? orderID);

        /// <summary>
        /// Сохранение заказа определенным пользователем.
        /// </summary>
        /// <param name="token">Токен пользователя.</param>
        /// <param name="order">Заказ.</param>
        void SaveRepairOrder(SecurityToken token, RepairOrder order);

        /// <summary>
        /// Получает список пунктов выполненных работ с фильтром.
        /// </summary>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="repairOrderID">Код заказа.</param>
        /// <returns>Список пунктов выполненных работ.</returns>
        IEnumerable<WorkItemDTO> GetWorkItems(SecurityToken token, Guid? repairOrderID, string name, int page, int pageSize, out int count);

        /// <summary>
        /// Возвращает с хранилища пункт выполненных работ.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="workItemID">Код пункта выполненных работ.</param>
        /// <returns>Пункт выполненных работ</returns>
        WorkItemDTO GetWorkItem(SecurityToken token, Guid? workItemID);

        /// <summary>
        /// Возвращает сохраняет в хранилище пункт выполненных работ.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="workItem">Пункт выполненных работ.</param>
        void SaveWorkItem(SecurityToken token, WorkItem workItem);

        /// <summary>
        /// Удаляет из хранилища пункт выполненных работ руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="workItemID">Код пункта выполненных работ.</param>
        void DeleteWorkItem(SecurityToken token, Guid? workItemID);

        /// <summary>
        /// Получает список запчастей с фильтром.
        /// </summary>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="repairOrderID">Код заказа.</param>
        /// <returns>Список запчастей.</returns>
        IEnumerable<DeviceItem> GetDeviceItems(SecurityToken token, Guid? repairOrderID, string name, int page, int pageSize, out int count);

        /// <summary>
        /// Возвращает с хранилища запчасти.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="deviceItemID">Код запчасти.</param>
        /// <returns>Запчасть.</returns>
        DeviceItem GetDeviceItem(SecurityToken token, Guid? deviceItemID);

        /// <summary>
        /// Возвращает сохраняет в хранилище запчасть заказа.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="deviceItem">Запчасть заказа.</param>
        void SaveDeviceItem(SecurityToken token, DeviceItem deviceItem);

        /// <summary>
        /// Удаляет из хранилища пункт выполненных работ руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="deviceItemID">Код пункта выполненных работ.</param>
        void DeleteDeviceItem(SecurityToken token, Guid? deviceItemID);

        /// <summary>
        /// Создает отчет по заказу руководствуясь привелегиями текущего пользователя.
        /// </summary>
        /// <param name="token">Текущий токен пользователя.</param>
        /// <param name="customReportID">Код отчета.</param>
        /// <param name="repairOrderID">Код пользователя.</param>
        /// <returns>Созданный отчет.</returns>
        string CreateRepairOrderReport(SecurityToken token, Guid? customReportID, Guid? repairOrderID);

        /// <summary>
        /// Возвращает список заказов в работе определенных пользователей с фильтром.
        /// </summary>
        /// <param name="token">Контекст безопасности. </param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список заказов.</returns>
        IEnumerable<RepairOrderDTO> GetWorkRepairOrders(SecurityToken token, string name, int page, int pageSize, out int count);

        /// <summary>
        /// Получает пункты истории изменений по конкретному заказу.
        /// </summary>
        /// <param name="token">Текущий токен пользователя.</param>
        /// <param name="repairOrderID">Код заказа.</param>
        /// <returns>Список пунктов истории.</returns>
        IQueryable<OrderTimeline> GetOrderTimelines(SecurityToken token,Guid? repairOrderID);

        /// <summary>
        /// Добавляет комментарий в заказ.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="repairOrderID">Код заказа.</param>
        /// <param name="text">Текст комментария.</param>
        void AddRepairOrderComment(SecurityToken token, Guid? repairOrderID, string text);

        /// <summary>
        /// Получает список статусов заказа с фильтром.
        /// </summary>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список статусов заказа.</returns>
        IEnumerable<OrderStatus> GetOrderStatuses(SecurityToken token, string name, int page, int pageSize, out int count);

        /// <summary>
        /// Возвращает с хранилища статус заказа.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="orderStatusID">Код статуса заказа.</param>
        /// <returns>Статус заказа</returns>
        OrderStatus GetOrderStatus(SecurityToken token, Guid? orderStatusID);

        /// <summary>
        /// Сохраняет в хранилище статус заказа.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="orderStatus">Статус заказа.</param>
        void SaveOrderStatus(SecurityToken token, OrderStatus orderStatus);

        /// <summary>
        /// Удаляет из хранилища статус заказа руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="orderStatusID">Код статсуса заказа.</param>
        void DeleteOrderStatus(SecurityToken token, Guid? orderStatusID);

        /// <summary>
        /// Получает список всех статусов заказа для пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список статусов заказа.</returns>
        IQueryable<OrderStatus> GetOrderStatuses(SecurityToken token);

        /// <summary>
        /// Получение статусов заказа по его типам.
        /// </summary>
        /// <param name="token">Токен безопасности. </param>
        /// <param name="kindId">Тип статуса.</param>
        /// <returns>Если не находит пытается найти ближайший по смыслу.</returns>
        OrderStatus GetOrderStatusByKind(SecurityToken token, byte? kindId);

        /// <summary>
        /// Получает список типов заказа с фильтром.
        /// </summary>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список типов заказа.</returns>
        IEnumerable<OrderKind> GetOrderKinds(SecurityToken token, string name, int page, int pageSize, out int count);

        /// <summary>
        /// Возвращает с хранилища тип заказа.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="orderKindID">Код статуса заказа.</param>
        /// <returns>Тип заказа</returns>
        OrderKind GetOrderKind(SecurityToken token, Guid? orderKindID);

        /// <summary>
        /// Сохраняет в хранилище тип заказа.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="orderKind">Тип заказа.</param>
        void SaveOrderKind(SecurityToken token, OrderKind orderKind);

        /// <summary>
        /// Удаляет из хранилища тип заказа руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="orderKindID">Код типа заказа.</param>
        void DeleteOrderKind(SecurityToken token, Guid? orderKindID);

        /// <summary>
        /// Получает список всех типов заказа для пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список статусов заказа.</returns>
        IQueryable<OrderKind> GetOrderKinds(SecurityToken token);

        /// <summary>
        /// Получает список настраеваемых отчетов с фильтром по типу документа.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="documentKindID">Код типа документа. </param>
        /// <returns>Список настраеваемых отчетов.</returns>
        IEnumerable<CustomReportItem> GetCustomReportItems(SecurityToken token, byte? documentKindID);

        /// <summary>
        /// Получает список всех настраеваемых отчетов для пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список настраеваемых отчетов.</returns>
        IQueryable<CustomReportItem> GetCustomReportItems(SecurityToken token);

        /// <summary>
        /// Возвращает с хранилища настраеваемых отчет.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="customReportItemID">Код настраеваемого отчета.</param>
        /// <returns>Настраеваемый отчет</returns>
        CustomReportItem GetCustomReportItem(SecurityToken token, Guid? customReportItemID);

        /// <summary>
        /// Сохраняет в хранилище настраеваемый отчет.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="customReportItem">Настраеваемый отчет.</param>
        void SaveCustomReportItem(SecurityToken token, CustomReportItem customReportItem);

        /// <summary>
        /// Удаляет из хранилища настраеваемый отчет руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="customReportItemID">Код настраеваемого отчета.</param>
        void DeleteCustomReportItem(SecurityToken token, Guid? customReportItemID);

        /// <summary>
        /// Получает список филиалов с фильтром.
        /// </summary>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список филиалов.</returns>
        IEnumerable<Branch> GetBranches(SecurityToken token, string name, int page, int pageSize, out int count);

        /// <summary>
        /// Получает список всех филиалов для пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список филиалов.</returns>
        IQueryable<Branch> GetBranches(SecurityToken token);

        /// <summary>
        /// Возвращает с хранилища филиал.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="branchID">Код филиала.</param>
        /// <returns>Филиал</returns>
        Branch GetBranch(SecurityToken token, Guid? branchID);

        /// <summary>
        /// Сохраняет в хранилище филиал.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="branch">Филиала.</param>
        void SaveBranch(SecurityToken token, Branch branch);

        /// <summary>
        /// Удаляет из хранилища филиал руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="branchID">Код филиала.</param>
        void DeleteBranch(SecurityToken token, Guid? branchID);

        /// <summary>
        /// Удаляет из хранилища заказ руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="orderID">Код заказа.</param>
        void DeleteOrder(SecurityToken token, Guid? orderID);

        /// <summary>
        /// Возвращает список заказов по фильтром.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="orderStatusId">Код статуса задачи.</param>
        /// <param name="isUrgent">Признак срочности.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список заказов.</returns>
        IEnumerable<RepairOrderDTO> GetRepairOrders(SecurityToken token, Guid? orderStatusId, bool? isUrgent, string name, int page, int pageSize, out int count);

        /// <summary>
        /// Возвращает список заказов по фильтром по филиалам которые доступны пользователям.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="orderStatusId">Код статуса  задачи.</param>
        /// <param name="isUrgent">Признак срочности.</param>
        /// <param name="userId">Код пользователя по которому производится поиск филиалов. </param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список заказов.</returns>
        IEnumerable<RepairOrderDTO> GetRepairOrdersUserBranch(SecurityToken token, Guid? orderStatusId, bool? isUrgent, Guid? userId, string name, int page, int pageSize, out int count);

        /// <summary>
        /// Возвращает список заказов по фильтром по конкретным исполнителям.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="orderStatusId">Код статуса  задачи.</param>
        /// <param name="isUrgent">Признак срочности.</param>
        /// <param name="userId">Код пользователя по которому производится поиск задач. </param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список заказов.</returns>
        IEnumerable<RepairOrderDTO> GetRepairOrdersUser(SecurityToken token, Guid? orderStatusId, bool? isUrgent, Guid? userId, string name, int page, int pageSize, out int count);

        /// <summary>
        /// Получает новый id для заказа.
        /// </summary>
        /// <param name="token">Токен безопасности </param>
        /// <returns>Новый id.</returns>
        long GetNewOrderNumber(SecurityToken token);

        /// <summary>
        /// Получает список пользователей с фильтром.
        /// </summary>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список пользовательов.</returns>
        IEnumerable<User> GetUsers(SecurityToken token, string name, int page, int pageSize, out int count);

        /// <summary>
        /// Получает список всех пользователей для пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список пользователей.</returns>
        IQueryable<User> GetUsers(SecurityToken token);

        /// <summary>
        /// Получает список всех пользователей с определенной ролью.
        /// </summary>
        /// <param name="projectRoleId">Код роли в проекте. </param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список пользователей.</returns>
        IEnumerable<User> GetUsers(SecurityToken token, byte? projectRoleId);

        /// <summary>
        /// Возвращает с хранилища пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="userID">Код пользователя.</param>
        /// <returns>Пользователь</returns>
        User GetUser(SecurityToken token, Guid? userID);

        /// <summary>
        /// Сохраняет в хранилище пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="user">Пользователь.</param>
        void SaveUser(SecurityToken token, User user);

        /// <summary>
        /// Удаляет из хранилища пользователя руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="userID">Код пользователя.</param>
        void DeleteUser(SecurityToken token, Guid? userID);

        /// <summary>
        /// Получает пункты отчета для работы инженеров.
        /// </summary>
        /// <param name="token">Код домена.</param>
        /// <param name="engineerID">Код инженера, может быть null, тогда собираются данные по всем инженерам.</param>
        /// <param name="beginDate">Дата начала.</param>
        /// <param name="endDate">Дата окончания периода.</param>
        /// <returns>Списко пунктов отчета.</returns>
        IEnumerable<EngineerWorkReportItem> GetEngineerWorkReportItems(SecurityToken token, Guid? engineerID, DateTime beginDate, DateTime endDate);

        /// <summary>
        /// Получает список финансовая группаов с фильтром.
        /// </summary>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список финансовая группаов.</returns>
        IEnumerable<FinancialGroupItem> GetFinancialGroupItems(SecurityToken token, string name, int page, int pageSize, out int count);

        /// <summary>
        /// Получает список всех финансовых групп для пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список финансовых групп.</returns>
        IQueryable<FinancialGroupItem> GetFinancialGroupItems(SecurityToken token);

        /// <summary>
        /// Возвращает с хранилища финансовую группу.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="financialGroupID">Код финансовай группы.</param>
        /// <returns>Филиал</returns>
        FinancialGroupItem GetFinancialGroupItem(SecurityToken token, Guid? financialGroupID);

        /// <summary>
        /// Сохраняет в хранилище финансовая группа.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="financialGroupItem">Филиала.</param>
        void SaveFinancialGroupItem(SecurityToken token, FinancialGroupItem financialGroupItem);

        /// <summary>
        /// Удаляет из хранилища финансовая группа руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="financialGroupID">Код финансовая группаа.</param>
        void DeleteFinancialGroupItem(SecurityToken token, Guid? financialGroupID);

        /// <summary>
        /// Получает список финансовых статей с фильтром.
        /// </summary>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список финансовых статей.</returns>
        IEnumerable<FinancialItem> GetFinancialItems(SecurityToken token, string name, int page, int pageSize, out int count);

        /// <summary>
        /// Возвращает с хранилища финансовую статью.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="financialID">Код финансовой статьи.</param>
        /// <returns>Филиал</returns>
        FinancialItem GetFinancialItem(SecurityToken token, Guid? financialID);

        /// <summary>
        /// Сохраняет в хранилище финансовую статью.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="financialItem">Финансовая статья.</param>
        void SaveFinancialItem(SecurityToken token, FinancialItem financialItem);

        /// <summary>
        /// Удаляет из хранилища финансовлй статьи руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="financialID">Код финансовой статьи.</param>
        void DeleteFinancialItem(SecurityToken token, Guid? financialID);

        /// <summary>
        /// Получает список значений статей расходов.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="financialGroupID">Код финансовой группы пользователя.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="endDate">Дата окончания. </param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <param name="beginDate">Дата окончания.</param>
        /// <returns>Список значений финансовых статей.</returns>
        IEnumerable<FinancialItemValueDTO> GetFinancialItemValues(SecurityToken token,Guid? financialGroupID, string name, DateTime beginDate, DateTime endDate, int page, int pageSize, out int count);

        /// <summary>
        /// Возвращает с хранилища значение финансовой статьи.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="financialItemValueID">Код значения финансовой статьи.</param>
        /// <returns>Значение финансовой статьи.</returns>
        FinancialItemValueDTO GetFinancialItemValue(SecurityToken token, Guid? financialItemValueID);

        /// <summary>
        /// Сохраняет в хранилище значение финансовой статьи.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="financialItemValue">Значение финансовой статьи.</param>
        void SaveFinancialItemValue(SecurityToken token, FinancialItemValue financialItemValue);

        /// <summary>
        /// Удаляет из хранилища значение финансовой статьи руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="financialID">Код значения финансовой статьи.</param>
        void DeleteFinancialItemValue(SecurityToken token, Guid? financialID);

        /// <summary>
        /// Получение отчета по пользовательским данным по расходам и доходам.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="financialGroupID">Код финансовой группы филиалов.</param>
        /// <param name="beginDate">Дата начала.</param>
        /// <param name="endDate">Дата завершения.</param>
        /// <returns>Список пунктов отчета.</returns>
        IEnumerable<RevenueAndExpenditureReportItem> GetRevenueAndExpenditureReportItems(SecurityToken token, Guid? financialGroupID, DateTime beginDate, DateTime endDate);

        /// <summary>
        /// Получает общую информацию по установленным запчастям и выполненным работам за определенный период выдачи клиентам для фин группы.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="financialGroupID">Код финансовой группы.</param>
        /// <param name="beginDate">Дата начала.</param>
        /// <param name="endDate">Дата завершения.</param>
        /// <returns>Информация.</returns>
        ItemsInfo GetOrderPaidAmountByOrderIssueDate(SecurityToken token, Guid? financialGroupID, DateTime beginDate, DateTime endDate);

        /// <summary>
        /// Получает отчет по используемым запчастям за определенный период времени.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="branchID">Код филиала.</param>
        /// <param name="financialGroupID">Код финансовой группы.</param>
        /// <param name="beginDate">Дата начала.</param>
        /// <param name="endDate">Дата окончания.</param>
        /// <returns>Пункты отчета.</returns>
        IEnumerable<UsedDeviceItemsReportItem> GetUsedDeviceItemsReportItems(SecurityToken token, Guid? branchID, Guid? financialGroupID, DateTime beginDate, DateTime endDate);

        /// <summary>
        /// Получает список категорий товара с фильтром.
        /// </summary>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список категорий товара.</returns>
        IEnumerable<ItemCategory> GetItemCategories(SecurityToken token, string name, int page, int pageSize, out int count);

        /// <summary>
        /// Возвращает с хранилища категорию товара.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="itemCategoryID">Код категории товара.</param>
        /// <returns>Категория товара.</returns>
        ItemCategory GetItemCategory(SecurityToken token, Guid? itemCategoryID);

        /// <summary>
        /// Сохраняет в хранилище категорию товара.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="itemCategory">Категория товара.</param>
        void SaveItemCategory(SecurityToken token, ItemCategory itemCategory);

        /// <summary>
        /// Удаляет из хранилища категорию товара руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="itemCategoryID">Код категории товара.</param>
        void DeleteItemCategory(SecurityToken token, Guid? itemCategoryID);

        /// <summary>
        /// Получает список складов с фильтром.
        /// </summary>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список складов.</returns>
        IEnumerable<Warehouse> GetWarehouses(SecurityToken token, string name, int page, int pageSize, out int count);

        /// <summary>
        /// Возвращает с хранилища склад.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="warehouseID">Код склада.</param>
        /// <returns>Склад.</returns>
        Warehouse GetWarehouse(SecurityToken token, Guid? warehouseID);

        /// <summary>
        /// Сохраняет в хранилище склад.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="warehouse">Склад.</param>
        void SaveWarehouse(SecurityToken token, Warehouse warehouse);

        /// <summary>
        /// Удаляет из хранилища склад руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="warehouseID">Код склада.</param>
        void DeleteWarehouse(SecurityToken token, Guid? warehouseID);

        /// <summary>
        /// Получает список номенклатуры с фильтром.
        /// </summary>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список номенклатуры.</returns>
        IEnumerable<GoodsItemDTO> GetGoodsItems(SecurityToken token, string name, int page, int pageSize, out int count);

        /// <summary>
        /// Возвращает с хранилища номенклатуру.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="goodsItemID">Код номенклатуры.</param>
        /// <returns>Номенклатура.</returns>
        GoodsItemDTO GetGoodsItem(SecurityToken token, Guid? goodsItemID);

        /// <summary>
        /// Сохраняет в хранилище номенклатуру.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="goodsItem">Номенклатура.</param>
        void SaveGoodsItem(SecurityToken token, GoodsItem goodsItem);

        /// <summary>
        /// Удаляет из хранилища номенклатуру руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="goodsItemID">Код номенклатуры.</param>
        void DeleteGoodsItem(SecurityToken token, Guid? goodsItemID);

        /// <summary>
        /// Получает список остатков на складе с фильтром.
        /// </summary>
        /// <param name="warehouseID">Код склада.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список остатов на складе.</returns>
        IEnumerable<WarehouseItemDTO> GetWarehouseItems(SecurityToken token,Guid? warehouseID, string name, int page, int pageSize, out int count);

        /// <summary>
        /// Возвращает с хранилища остатки на складе.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="warehouseItemID">Код остатков на складе.</param>
        /// <returns>Остатки на складе.</returns>
        WarehouseItemDTO GetWarehouseItem(SecurityToken token, Guid? warehouseItemID);

        /// <summary>
        /// Сохраняет в хранилище остатки на складе.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="warehouseItem">Остатки на складе.</param>
        void SaveWarehouseItem(SecurityToken token, WarehouseItem warehouseItem);

        /// <summary>
        /// Удаляет из хранилища остатки на складе руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="warehouseItemID">Код остатов на складе.</param>
        void DeleteWarehouseItem(SecurityToken token, Guid? warehouseItemID);

        /// <summary>
        /// Получает список контрагентов с фильтром.
        /// </summary>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список контрагентов.</returns>
        IEnumerable<Contractor> GetContractors(SecurityToken token, string name, int page, int pageSize, out int count);

        /// <summary>
        /// Возвращает с хранилища контрагента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="contractorID">Код контрагента.</param>
        /// <returns>Контрагент.</returns>
        Contractor GetContractor(SecurityToken token, Guid? contractorID);

        /// <summary>
        /// Сохраняет в хранилище контрагент.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="contractor">Контрагент.</param>
        void SaveContractor(SecurityToken token, Contractor contractor);

        /// <summary>
        /// Удаляет из хранилища контрагента руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="contractorID">Код контрагента.</param>
        void DeleteContractor(SecurityToken token, Guid? contractorID);

        /// <summary>
        /// Получает список приходных накладных с фильтром.
        /// </summary>
        /// <param name="endDate">Дата окончания. </param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="warehouseID">Код склада. </param>
        /// <param name="beginDate">Дата начала.</param>
        /// <returns>Список приходных накладных.</returns>
        IEnumerable<IncomingDocDTO> GetIncomingDocs(SecurityToken token, Guid? warehouseID, DateTime beginDate, DateTime endDate, string name, int page, int pageSize, out int count);

        /// <summary>
        /// Возвращает с хранилища приходную накладную.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="incomingDocID">Код приходной накладной.</param>
        /// <returns>Приходная накладная.</returns>
        IncomingDocDTO GetIncomingDoc(SecurityToken token, Guid? incomingDocID);

        /// <summary>
        /// Сохраняет в хранилище приходную накладную.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="incomingDoc">Приходная накладная.</param>
        void SaveIncomingDoc(SecurityToken token, IncomingDoc incomingDoc);

        /// <summary>
        /// Удаляет из хранилища приходную накладную руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="incomingDocID">Код приходной накладной.</param>
        void DeleteIncomingDoc(SecurityToken token, Guid? incomingDocID);

        /// <summary>
        /// Получает список элементов приходных накладных с фильтром.
        /// </summary>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="incomingDocID">Код накладной. </param>
        /// <returns>Список элементов приходных накладных.</returns>
        IEnumerable<IncomingDocItemDTO> GetIncomingDocItems(SecurityToken token, Guid? incomingDocID, string name, int page, int pageSize, out int count);

        /// <summary>
        /// Возвращает с хранилища элемент приходной накладной.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="incomingDocItemID">Код приходной накладной.</param>
        /// <returns>Приходная накладная.</returns>
        IncomingDocItemDTO GetIncomingDocItem(SecurityToken token, Guid? incomingDocItemID);

        /// <summary>
        /// Сохраняет в хранилище элемент приходной накладной.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="incomingDocItem">Элемент приходной накладной.</param>
        void SaveIncomingDocItem(SecurityToken token, IncomingDocItem incomingDocItem);

        /// <summary>
        /// Удаляет из хранилища элемент приходной накладной руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="incomingDocItemID">Код элемента приходной накладной.</param>
        void DeleteIncomingDocItem(SecurityToken token, Guid? incomingDocItemID);

        /// <summary>
        /// Получает список документов списаний с фильтром.
        /// </summary>
        /// <param name="endDate">Дата окончания. </param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="warehouseID">Код склада. </param>
        /// <param name="beginDate">Дата начала.</param>
        /// <returns>Список списаний.</returns>
        IEnumerable<CancellationDocDTO> GetCancellationDocs(SecurityToken token, Guid? warehouseID, DateTime beginDate, DateTime endDate, string name, int page, int pageSize, out int count);

        /// <summary>
        /// Возвращает с хранилища документ списаний.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="cancellationDocID">Код документа списаний.</param>
        /// <returns>Документ списания.</returns>
        CancellationDocDTO GetCancellationDoc(SecurityToken token, Guid? cancellationDocID);

        /// <summary>
        /// Сохраняет в хранилище документ списаний.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="cancellationDoc">Документ списаний.</param>
        void SaveCancellationDoc(SecurityToken token, CancellationDoc cancellationDoc);

        /// <summary>
        /// Удаляет из хранилища документ списаний руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="cancellationDocID">Код документа списаний.</param>
        void DeleteCancellationDoc(SecurityToken token, Guid? cancellationDocID);

        /// <summary>
        /// Получает список элементов документов списания с фильтром.
        /// </summary>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="cancellationDocID">Код документа списания. </param>
        /// <returns>Список элементов документов списания.</returns>
        IEnumerable<CancellationDocItemDTO> GetCancellationDocItems(SecurityToken token, Guid? cancellationDocID, string name, int page, int pageSize, out int count);

        /// <summary>
        /// Возвращает с хранилища элемент документа списания.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="cancellationDocItemID">Код документа списания.</param>
        /// <returns>Документа списания.</returns>
        CancellationDocItemDTO GetCancellationDocItem(SecurityToken token, Guid? cancellationDocItemID);

        /// <summary>
        /// Сохраняет в хранилище элемент документа списания.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="cancellationDocItem">Элемент документа списания.</param>
        void SaveCancellationDocItem(SecurityToken token, CancellationDocItem cancellationDocItem);

        /// <summary>
        /// Удаляет из хранилища элемент документа списания руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="cancellationDocItemID">Код элемента документа списания.</param>
        void DeleteCancellationDocItem(SecurityToken token, Guid? cancellationDocItemID);

        /// <summary>
        /// Получает список перемещений со склада на склад с фильтром.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="senderWarehouseID">Код склада с которого делают перемещение.</param>
        /// <param name="endDate">Дата окончания накладных.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <param name="recipientWarehouseID">Код склада на который делают перемещение. </param>
        /// <param name="beginDate">Дата начала создания накладных.</param>
        /// <returns>Список перемещений со склада товаров.</returns>
        IEnumerable<TransferDocDTO> GetTransferDocs(SecurityToken token, Guid? senderWarehouseID, Guid? recipientWarehouseID, DateTime beginDate, DateTime endDate, string name, int page, int pageSize, out int count);

        /// <summary>
        /// Возвращает с хранилища документ перемещений.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="transferDocID">Код документа перемещений.</param>
        /// <returns>Документ перемещения.</returns>
        TransferDocDTO GetTransferDoc(SecurityToken token, Guid? transferDocID);

        /// <summary>
        /// Сохраняет в хранилище документ перемещений.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="transferDoc">Документ перемещений.</param>
        void SaveTransferDoc(SecurityToken token, TransferDoc transferDoc);

        /// <summary>
        /// Удаляет из хранилища документ перемещений руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="transferDocID">Код документа перемещений.</param>
        void DeleteTransferDoc(SecurityToken token, Guid? transferDocID);

        /// <summary>
        /// Получает список элементов документов перемещения с фильтром.
        /// </summary>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="transferDocID">Код документа перемещения. </param>
        /// <returns>Список элементов документов перемещения.</returns>
        IEnumerable<TransferDocItemDTO> GetTransferDocItems(SecurityToken token, Guid? transferDocID, string name, int page, int pageSize, out int count);

        /// <summary>
        /// Возвращает с хранилища элемент документа перемещения.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="transferDocItemID">Код документа перемещения.</param>
        /// <returns>Документа перемещения.</returns>
        TransferDocItemDTO GetTransferDocItem(SecurityToken token, Guid? transferDocItemID);

        /// <summary>
        /// Сохраняет в хранилище элемент документа перемещения.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="transferDocItem">Элемент документа перемещения.</param>
        void SaveTransferDocItem(SecurityToken token, TransferDocItem transferDocItem);

        /// <summary>
        /// Удаляет из хранилища элемент документа перемещения руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="transferDocItemID">Код элемента документа перемещения.</param>
        void DeleteTransferDocItem(SecurityToken token, Guid? transferDocItemID);

        /// <summary>
        /// Получает список всех категорий товара для пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список категорий товара.</returns>
        IQueryable<ItemCategory> GetItemCategories(SecurityToken token);

        /// <summary>
        /// Получает список складов для пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список складов.</returns>
        IQueryable<Warehouse> GetWarehouses(SecurityToken token);

        /// <summary>
        /// Получает список всех контрагентов для пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список контрагентов.</returns>
        IQueryable<Contractor> GetContractors(SecurityToken token);

        /// <summary>
        /// Проверяет документ на признак обработки.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="docID">Код документа.</param>
        /// <returns>Признак обработки.</returns>
        bool WarehouseDocIsProcessed(SecurityToken token, Guid? docID);

        /// <summary>
        /// Обрабатывает пункты приходной накладной.
        /// </summary>
        /// <param name="incomingDocID">Код приходной накладной.</param>
        /// <param name="token">Токен безопастности.</param>
        /// <param name="eventDate">Дата обработи документа </param>
        /// <param name="utcEventDateTime">UTC дата и время обработки документа. </param>
        ProcessWarehouseDocResult ProcessIncomingDocItems(SecurityToken token,Guid? incomingDocID, DateTime eventDate, DateTime utcEventDateTime);

        /// <summary>
        /// Обрабатывает пункты документа о списании.
        /// </summary>
        /// <param name="cancellationDocID">Код документа о списании.</param>
        /// <param name="token">Токен безопастности.</param>
        /// <param name="eventDate">Дата обработи документа </param>
        /// <param name="utcEventDateTime">UTC дата и время обработки документа. </param>
        ProcessWarehouseDocResult ProcessCancellationDocItems(SecurityToken token, Guid? cancellationDocID, DateTime eventDate, DateTime utcEventDateTime);

        /// <summary>
        /// Обрабатывает пункты документа о перемещении со склада на склад.
        /// </summary>
        /// <param name="transferDocID">Код документа о перемещении.</param>
        /// <param name="token">Токен безопастности.</param>
        /// <param name="eventDate">Дата обработи документа </param>
        /// <param name="utcEventDateTime">UTC дата и время обработки документа. </param>
        ProcessWarehouseDocResult ProcessTransferDocItems(SecurityToken token, Guid? transferDocID, DateTime eventDate, DateTime utcEventDateTime);

        /// <summary>
        /// Получает список всех элементов приходной накладной.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="incomingDocID">Код приходной накладной.</param>
        /// <returns>Список элемент приходной накладной товаров.</returns>
        IQueryable<IncomingDocItemDTO> GetIncomingDocItems(SecurityToken token, Guid? incomingDocID);

        /// <summary>
        /// Получает пункты отчета по движениям на складе.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="warehouseID">Код склада.</param>
        /// <param name="beginDate">Дата начала.</param>
        /// <param name="endDate">Дата окончания.</param>
        /// <returns>Пункты отчета.</returns>
        IEnumerable<WarehouseFlowReportItem> GetWarehouseFlowReportItems(SecurityToken token, Guid? warehouseID, DateTime beginDate, DateTime endDate);

        /// <summary>
        /// Отменяет обработку пунктов приходной накладной.
        /// </summary>
        /// <param name="incomingDocID">Код приходной накладной.</param>
        /// <param name="token">Токен безопастности.</param>
        ProcessWarehouseDocResult UnProcessIncomingDocItems(SecurityToken token, Guid? incomingDocID);

        /// <summary>
        /// Отменяет обрабатку пунктов документа о списании.
        /// </summary>
        /// <param name="cancellationDocID">Код документа о списании.</param>
        /// <param name="token">Токен безопастности.</param>
        ProcessWarehouseDocResult UnProcessCancellationDocItems(SecurityToken token, Guid? cancellationDocID);

        /// <summary>
        /// Отменяет обработанные пункты документа о перемещении со склада на склад.
        /// </summary>
        /// <param name="transferDocID">Код отменяемого документа о перемещении.</param>
        /// <param name="token">Токен безопастности.</param>
        ProcessWarehouseDocResult UnProcessTransferDocItems(SecurityToken token, Guid? transferDocID);

        /// <summary>
        /// Получение пункты отчета по завершенным приходным накладным для финансовой группы за определенный период.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="financialGroupID">Код финансовой группы.</param>
        /// <param name="beginDate">Дата начала.</param>
        /// <param name="endDate">Дата окончания.</param>
        /// <returns>Пункты отчета.</returns>
        IEnumerable<WarehouseDocTotalItem> GetWarehouseDocTotalItems(SecurityToken token, Guid? financialGroupID, DateTime beginDate, DateTime endDate);

        /// <summary>
        /// Обновляет информацию по домену пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="updatedEntity">Обновляемый домен.</param>
        void UpdateUserDomain(SecurityToken token, UserDomain updatedEntity);

        /// <summary>
        /// Получает текущий домен пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Домен.</returns>
        UserDomain GetUserDomain(SecurityToken token);

        /// <summary>
        /// Получает списко заказов за определенный период.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="beginDate">Дата начала.</param>
        /// <param name="endDate">Дата окончания.</param>
        /// <returns>Заказы.</returns>
        IEnumerable<RepairOrder> GetRepairOrders(SecurityToken token, DateTime beginDate, DateTime endDate);

        /// <summary>
        /// Получает список запросов на активацию публичных ключей с фильтром.
        /// </summary>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список запросов на активацию.</returns>
        IEnumerable<UserPublicKeyRequestDTO> GetUserPublicKeyRequests(SecurityToken token, string name, int page, int pageSize, out int count);

        /// <summary>
        /// Возвращает с хранилища запрос на активацию ключа.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="userPublicKeyRequestID">Код запроса на активацию публичного ключа.</param>
        /// <returns>Запрос на активацию.</returns>
        UserPublicKeyRequestDTO GetUserPublicKeyRequest(SecurityToken token, Guid? userPublicKeyRequestID);

        /// <summary>
        /// Удаляет из хранилища запрос на регистрацию публичного ключа руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="userPublicKeyRequestID">Код запроса на регистрацию публичного ключа.</param>
        void DeleteUserPublicKeyRequest(SecurityToken token, Guid? userPublicKeyRequestID);

        /// <summary>
        /// Получает список публичных ключей с фильтром.
        /// </summary>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список публичных ключей.</returns>
        IEnumerable<UserPublicKeyDTO> GetUserPublicKeys(SecurityToken token, string name, int page, int pageSize, out int count);

        /// <summary>
        /// Возвращает с хранилища публичный ключ.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="userPublicKeyID">Код публичных ключей.</param>
        /// <returns>Публичный ключ.</returns>
        UserPublicKeyDTO GetUserPublicKey(SecurityToken token, Guid? userPublicKeyID);

        /// <summary>
        /// Сохраняет в хранилище публичный ключ.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="userPublicKey">Публичный ключ.</param>
        void SaveUserPublicKey(SecurityToken token, UserPublicKey userPublicKey);

        /// <summary>
        /// Удаляет из хранилища публичный ключ руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="userPublicKeyID">Код публичного ключа.</param>
        void DeleteUserPublicKey(SecurityToken token, Guid? userPublicKeyID);

        /// <summary>
        /// Определяет есть ли у пользователя доступ к определенному заказу.
        /// </summary>
        /// <param name="userID">Код пользователя.</param>
        /// <param name="repairOrder">Заказ.</param>
        /// <param name="projectRoleId">Код роли.</param>
        /// <returns>Признак наличия доступа.</returns>
        bool UserHasAccessToRepairOrder(Guid? userID, RepairOrder repairOrder, byte? projectRoleId);

        /// <summary>
        /// Получает список пунктов автозаполнения с фильтром.
        /// </summary>
        /// <param name="autocompleteKindID">Код типа пункта автозаполнения.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список пунктов автозаполнения.</returns>
        IEnumerable<AutocompleteItem> GetAutocompleteItems(SecurityToken token, byte? autocompleteKindID, string name, int page, int pageSize, out int count);

        /// <summary>
        /// Получает список всех пунктов автозаполнения для пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список пунктов автозаполнения.</returns>
        IQueryable<AutocompleteItem> GetAutocompleteItems(SecurityToken token);

        /// <summary>
        /// Получает список всех пунктов автозаполнения для пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="autocompleteKindID">Код типа пункта автозаполнения.</param>
        /// <returns>Список пунктов автозаполнений.</returns>
        IEnumerable<AutocompleteItem> GetAutocompleteItems(SecurityToken token, byte? autocompleteKindID);

        /// <summary>
        /// Возвращает с хранилища пункт автозаполнения.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="autocompleteItemID">Код статуса заказа.</param>
        /// <returns>Пункт автозаполнения</returns>
        AutocompleteItem GetAutocompleteItem(SecurityToken token, Guid? autocompleteItemID);

        /// <summary>
        /// Сохраняет в хранилище пункт автозаполнения.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="autocompleteItem">Пункт автозаполнения.</param>
        void SaveAutocompleteItem(SecurityToken token, AutocompleteItem autocompleteItem);

        /// <summary>
        /// Удаляет из хранилища пункт автозаполнения руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="autocompleteItemID">Код пункта автозаполнения.</param>
        void DeleteAutocompleteItem(SecurityToken token, Guid? autocompleteItemID);

        /// <summary>
        /// Получает список пунктов вознаграждения с фильтром.
        /// </summary>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список пунктов вознаграждения.</returns>
        IEnumerable<UserInterestDTO> GetUserInterests(SecurityToken token, string name, int page, int pageSize, out int count);

        /// <summary>
        /// Возвращает с хранилища пункт вознаграждения.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="userInterestID">Код статуса заказа.</param>
        /// <returns>Пункт вознаграждения</returns>
        UserInterestDTO GetUserInterest(SecurityToken token, Guid? userInterestID);

        /// <summary>
        /// Сохраняет в хранилище пункт вознаграждения.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="userInterest">Пункт вознаграждения.</param>
        void SaveUserInterest(SecurityToken token, UserInterest userInterest);

        /// <summary>
        /// Удаляет из хранилища пункт вознаграждения руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="userInterestID">Код пункта вознаграждения.</param>
        void DeleteUserInterest(SecurityToken token, Guid? userInterestID);

        /// <summary>
        /// Получает отчет по вознаграждениям пользователей за определенный период.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="beginDate">Дата начала.</param>
        /// <param name="endDate">Дата окончания.</param>
        /// <returns>Пункты отчета.</returns>
        IEnumerable<InterestReportItem> GetUserInterestReportItems(SecurityToken token, DateTime beginDate, DateTime endDate);

        /// <summary>
        /// Возвращает список заказов без фильтра.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список заказов.</returns>
        IQueryable<RepairOrderDTO> GetRepairOrders(SecurityToken token);

        /// <summary>
        /// Получает список пунктов выполненных работ.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="repairOrderID">Код заказа.</param>
        /// <returns>Список пунктов выполненных работ.</returns>
        IQueryable<WorkItemDTO> GetWorkItems(SecurityToken token, Guid? repairOrderID);

        /// <summary>
        /// Получает список запчастей.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="repairOrderID">Код заказа.</param>
        /// <returns>Список запчастей.</returns>
        IQueryable<DeviceItemDTO> GetDeviceItems(SecurityToken token, Guid? repairOrderID);

        /// <summary>
        /// Получает список остатков на складах.
        /// </summary>
        /// <returns>Список остатов на складе.</returns>
        IQueryable<WarehouseItemDTO> GetWarehouseItems(SecurityToken token);

        /// <summary>
        /// Получает информацию по состоянию грида.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="gridName"></param>
        /// <returns></returns>
        string GetGridUserState(SecurityToken token, string gridName);

        /// <summary>
        ///   Сохраняет информацию состояние гриде.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="gridName">Имя грида.</param>
        /// <param name="state">Состояние грида.</param>
        void SaveUserGridState(SecurityToken token, string gridName, string state);

        /// <summary>
        /// Получает информацию по состоянию грида.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="userGridFilterID">Код фильтра пользователя.</param>
        /// <returns></returns>
        UserGridFilter GetUserGridFilter(SecurityToken token, Guid? userGridFilterID);

        /// <summary>
        ///   Сохраняет информацию о фильтре грида.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="filter">Фильтр грида.</param>
        void SaveUserGridFilter(SecurityToken token, UserGridFilter filter);

        /// <summary>
        /// Получает список пользовательских фильтров для определенного грида.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="gridName">Название грида.</param>
        /// <returns>Фильтры грида.</returns>
        IEnumerable<UserGridFilter> GetUserGridFilters(SecurityToken token, string gridName);

        /// <summary>
        /// Получает список финансовых статей.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список финансовых статей.</returns>
        IQueryable<FinancialItem> GetFinancialItems(SecurityToken token);

        /// <summary>
        /// Получает список значений статей расходов.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список значений финансовых статей.</returns>
        IQueryable<FinancialItemValue> GetFinancialItemValues(SecurityToken token);

        /// <summary>
        /// Получает список пунктов вознаграждения.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список пунктов вознаграждения.</returns>
        IQueryable<UserInterest> GetUserInterests(SecurityToken token);

        /// <summary>
        /// Получает список запросов на активацию публичных ключей с фильтром.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список запросов на активацию.</returns>
        IQueryable<UserPublicKeyRequestDTO> GetUserPublicKeyRequests(SecurityToken token);

        /// <summary>
        /// Получает список публичных ключей.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список публичных ключей.</returns>
        IQueryable<UserPublicKeyDTO> GetUserPublicKeys(SecurityToken token);

        /// <summary>
        /// Получает список номенклатуры.
        /// </summary>
     
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список номенклатуры.</returns>
        IQueryable<GoodsItem> GetGoodsItems(SecurityToken token);

        /// <summary>
        /// Получает список приходных накладных.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список приходных накладных.</returns>
        IQueryable<IncomingDocDTO> GetIncomingDocs(SecurityToken token);

        /// <summary>
        /// Получает список документов списаний.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список списаний.</returns>
        IQueryable<CancellationDocDTO> GetCancellationDocs(SecurityToken token);

        /// <summary>
        /// Получает список элементов документов списания.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="cancellationDocID">Код документа списания. </param>
        /// <returns>Список элементов документов списания.</returns>
        IQueryable<CancellationDocItemDTO> GetCancellationDocItems(SecurityToken token, Guid? cancellationDocID);

        /// <summary>
        /// Получает список перемещений со склада на склад.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список перемещений со склада товаров.</returns>
        IQueryable<TransferDocDTO> GetTransferDocs(SecurityToken token);

        /// <summary>
        /// Получает список элементов документов перемещения.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="transferDocID">Код документа перемещения. </param>
        /// <returns>Список элементов документов перемещения.</returns>
        IQueryable<TransferDocItemDTO> GetTransferDocItems(SecurityToken token, Guid? transferDocID);
    }
}
