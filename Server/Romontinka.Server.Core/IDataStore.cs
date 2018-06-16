using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.DataLayer.Entities.HashItems;
using Romontinka.Server.DataLayer.Entities.ReportItems;

namespace Romontinka.Server.Core
{
    /// <summary>
    /// Интерфейс данных.
    /// </summary>
    public interface IDataStore
    {
        User GetUser(string loginName);

        /// <summary>
        /// Получение пользователя по логину и хэшу пароля.
        /// </summary>
        /// <param name="loginName">Логин пользователя.</param>
        /// <param name="passwordHash">Хэш пароля.</param>
        /// <returns>Пользователь, если найден.</returns>
        User GetUser(string loginName,string passwordHash);

        /// <summary>
        /// Обновляет хеш пароля на указанном пользователе.
        /// </summary>
        /// <param name="userID">Код пользователя.</param>
        /// <param name="newPasswordHash">Новый хэш пароля.</param>
        /// <returns>Признак успешности смены</returns>
        bool UpdatePasswordHash(Guid? userID, string newPasswordHash);

        /// <summary>
        /// Создает в системе нового пользователя.
        /// </summary>
        void CreateUser(User user);

        /// <summary>
        /// Обновление информации по существующему пользователю.
        /// Сохраняет все поля кроме хэша пароля.
        /// </summary>
        /// <param name="user">Пользователь для обновления информации.</param>
        void UpdateUser(User user);

        /// <summary>
        /// Удаляет из хранилища пользователя по его ID.
        /// </summary>
        /// <param name="id">Код пользователя.</param>
        void DeleteUser(Guid? id);

        /// <summary>
        ///   Сохраняет информацию филиале.
        /// </summary>
        /// <param name="branch"> Сохраняемый филиал. </param>
        void SaveBranch(Branch branch);

        /// <summary>
        /// Получает филиал по его ID.
        /// </summary>
        /// <param name="id">Код описания филиала.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Филиал, если существует.</returns>
        Branch GetBranch(Guid? id, Guid? userDomainID);

        /// <summary>
        /// Удаляет из хранилища филиал по его ID.
        /// </summary>
        /// <param name="id">Код филиала.</param>
        void DeleteBranch(Guid? id);

        /// <summary>
        /// Получает список филиалов с фильтром.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список филиалов.</returns>
        IEnumerable<Branch> GetBranches(Guid? userDomainID, string name, int page, int pageSize, out int count);

        /// <summary>
        /// Получает список типов заказа с фильтром.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя </param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список типов заказа.</returns>
        IEnumerable<OrderKind> GetOrderKinds(Guid? userDomainID,string name, int page, int pageSize, out int count);

        /// <summary>
        /// Получает полный список типов заказа.
        /// </summary>
        /// <returns>Список типов заказа.</returns>
        IQueryable<OrderKind> GetOrderKinds(Guid? userDomainID);

        /// <summary>
        ///   Сохраняет информацию пользователе.
        /// </summary>
        /// <param name="user"> Сохраняемый пользователь. </param>
        void SaveUser(User user);

        /// <summary>
        /// Получает пользователя по его ID.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="id">Код пользователя.</param>
        /// <returns>Пользователь, если существует.</returns>
        User GetUser(Guid? id, Guid? userDomainID);

        /// <summary>
        /// Получает список пользователей с фильтром.
        /// </summary>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список пользователей.</returns>
        IEnumerable<User> GetUsers(Guid? userDomainID,string name, int page, int pageSize, out int count);

        /// <summary>
        /// Получает список филиалов без фильтра.
        /// </summary>
        /// <returns>Список филиалов.</returns>
        IQueryable<Branch> GetBranches(Guid? userDomainID);

        /// <summary>
        /// Удаляет все связанные с пользователем филиалы.
        /// </summary>
        /// <param name="userId">Код пользователя.</param>
        void DeleteUserBranchMapItems(Guid? userId);

        /// <summary>
        /// Возвращает информацию о связах конкретного пользователя с филиалами.
        /// </summary>
        /// <param name="userId">Код пользователя.</param>
        /// <returns>Филиалы.</returns>
        IEnumerable<UserBranchMapItemDTO> GetUserBranchMapByItemsByUser(Guid? userId);

        /// <summary>
        ///   Сохраняет информацию о соответствии пользователя и филиала.
        /// </summary>
        /// <param name="userBranchMapItem"> Сохраняемое соответствие. </param>
        void SaveUserBranchMapItem(UserBranchMapItem userBranchMapItem);

        /// <summary>
        /// Осуществляет проверку привязан ли пользователь с филиалом.
        /// </summary>
        /// <param name="userId">Код пользователя.</param>
        /// <param name="branchId">Код филиала.</param>
        /// <returns>Признак существования связи.</returns>
        bool UserHasBranch(Guid? userId,Guid? branchId);

        /// <summary>
        /// Получает список всех пользователей.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Список пользователей.</returns>
        IQueryable<User> GetUsers(Guid? userDomainID);

        /// <summary>
        /// Возвращает информацию о связах конкретного пользователя с филиалами.
        /// </summary>
        /// <param name="userId">Код пользователя.</param>
        /// <param name="userProjectRoleId">Код проектной роли пользователя.</param>
        /// <returns>Филиалы.</returns>
        IEnumerable<UserBranchMapItemDTO> GetUserBranchMapByItemsByUser(Guid? userId,byte? userProjectRoleId);

        /// <summary>
        /// Получает список всех пользователей.
        /// </summary>
        /// <param name="projectRoleId">Код роли в проекте. </param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Список пользователей.</returns>
        IEnumerable<User> GetUsers(byte? projectRoleId, Guid? userDomainID);

        /// <summary>
        /// Получает новый id для заказа.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Новый id.</returns>
        long GetNewOrderNumber(Guid? userDomainID);

        /// <summary>
        ///   Сохраняет информацию о заказе.
        /// </summary>
        /// <param name="repairOrder"> Сохраняемый заказ. </param>
        void SaveRepairOrder(RepairOrder repairOrder);

        /// <summary>
        /// Получает заказа по его ID.
        /// </summary>
        /// <param name="id">Код заказа.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Заказ, если существует.</returns>
        RepairOrderDTO GetRepairOrder(Guid? id, Guid? userDomainID);

        /// <summary>
        /// Удаляет из хранилища заказ по его ID.
        /// </summary>
        /// <param name="id">Код заказа.</param>
        void DeleteRepairOrder(Guid? id);

        /// <summary>
        /// Возвращает список заказов по фильтром.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="orderStatusId">Код статуса задачи.</param>
        /// <param name="isUrgent">Признак срочности.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список заказов.</returns>
        IEnumerable<RepairOrderDTO> GetRepairOrders(Guid? userDomainID, Guid? orderStatusId,bool? isUrgent,string name, int page, int pageSize, out int count);

        /// <summary>
        /// Возвращает список заказов по фильтром по филиалам которые доступны пользователям.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="orderStatusId">Код статуса  задачи.</param>
        /// <param name="isUrgent">Признак срочности.</param>
        /// <param name="userId">Код пользователя по которому производится поиск филиалов. </param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список заказов.</returns>
        IEnumerable<RepairOrderDTO> GetRepairOrdersUserBranch(Guid? userDomainID,Guid? orderStatusId,bool? isUrgent,Guid? userId,string name, int page, int pageSize, out int count);

        /// <summary>
        /// Возвращает список заказов по фильтром по конкретным исполнителям.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователей.</param>
        /// <param name="orderStatusId">Код статуса  задачи.</param>
        /// <param name="isUrgent">Признак срочности.</param>
        /// <param name="userId">Код пользователя по которому производится поиск задач. </param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список заказов.</returns>
        IEnumerable<RepairOrderDTO> GetRepairOrdersUser(Guid? userDomainID,Guid? orderStatusId, bool? isUrgent, Guid? userId, string name, int page, int pageSize, out int count);

        /// <summary>
        /// Получает список статусов заказов с фильтром.
        /// </summary>
        /// <returns>Список статусов заказа.</returns>
        IQueryable<OrderStatus> GetOrderStatuses(Guid? userDomainID);

        /// <summary>
        /// Получение статусов заказа по его типам.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя. </param>
        /// <param name="kindId">Тип статуса.</param>
        /// <returns>Если не находит пытается найти ближайший по смыслу.</returns>
        OrderStatus GetOrderStatusByKind(Guid? userDomainID,byte? kindId);

        /// <summary>
        ///   Сохраняет информацию о проделанной работе.
        /// </summary>
        /// <param name="workItem"> Сохраняемая проделанная работа. </param>
        void SaveWorkItem(WorkItem workItem);

        /// <summary>
        /// Получает проделанную работу по его ID.
        /// </summary>
        /// <param name="id">Код проделанной работы.</param>
        /// <returns>Проделанная работа, если существует.</returns>
        WorkItemDTO GetWorkItem(Guid? id);

        /// <summary>
        /// Удаляет из хранилища проделанную работу по его ID.
        /// </summary>
        /// <param name="id">Код проделанной работы.</param>
        void DeleteWorkItem(Guid? id);

        /// <summary>
        /// Получает код филиала по коду заказа.
        /// </summary>
        /// <param name="repairOrderID">Код заказа.</param>
        /// <returns>Код филиала.</returns>
        Guid? GetRepairOrderBranchID(Guid? repairOrderID);

        /// <summary>
        /// Получает код инженера по коду заказа.
        /// </summary>
        /// <param name="repairOrderID">Код заказа.</param>
        /// <returns>Код инженера.</returns>
        Guid? GetRepairOrderEngineerID(Guid? repairOrderID);

        /// <summary>
        /// Получает список пунктов выполненных работ с фильтром.
        /// </summary>
        /// <param name="repairOrderID">Код пункта выполненных работ.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список пунктов выполненных работ.</returns>
        IEnumerable<WorkItemDTO> GetWorkItems(Guid? repairOrderID, string name, int page, int pageSize, out int count);

        /// <summary>
        /// Получает список запчастей для заказас фильтром.
        /// </summary>
        /// <param name="repairOrderID">Код заказа выполненных работ.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список запчатей заказа.</returns>
        IEnumerable<DeviceItem> GetDeviceItems(Guid? repairOrderID, string name, int page, int pageSize, out int count);

        /// <summary>
        ///   Сохраняет информацию замененой запчасти.
        /// </summary>
        /// <param name="deviceItem"> Сохраняемая запчасть. </param>
        void SaveDeviceItem(DeviceItem deviceItem);

        /// <summary>
        /// Получает запчасти по его ID.
        /// </summary>
        /// <param name="id">Код запчасти.</param>
        /// <returns>Запчасть, если существует.</returns>
        DeviceItem GetDeviceItem(Guid? id);

        /// <summary>
        /// Удаляет из хранилища запчасть по его ID.
        /// </summary>
        /// <param name="id">Код запчасти.</param>
        void DeleteDeviceItem(Guid? id);

        /// <summary>
        /// Получает список документов без фильтра.
        /// </summary>
        /// <returns>Список документов.</returns>
        IQueryable<CustomReportItem> GetCustomReportItems(Guid? userDomainID);

        /// <summary>
        ///   Сохраняет информацию документе.
        /// </summary>
        /// <param name="customReportItem"> Сохраняемый документ. </param>
        void SaveCustomReportItem(CustomReportItem customReportItem);

        /// <summary>
        /// Получает документ по его ID.
        /// </summary>
        /// <param name="id">Код описания документа.</param>
        /// <param name="userDomainID">Код домена пользователя. </param>
        /// <returns>Документ, если существует.</returns>
        CustomReportItem GetCustomReportItem(Guid? id, Guid? userDomainID);

        /// <summary>
        /// Удаляет из хранилища документ по его ID.
        /// </summary>
        /// <param name="id">Код документа.</param>
        void DeleteCustomReportItem(Guid? id);

        /// <summary>
        ///   Сохраняет информацию о графике заказа.
        /// </summary>
        /// <param name="orderTimeline"> Сохраняемый пункт о графике работы над заказом. </param>
        void SaveOrderTimeline(OrderTimeline orderTimeline);

        /// <summary>
        /// Получает график проделанной работы над заказом по его ID.
        /// </summary>
        /// <param name="id">Код графика заказа.</param>
        /// <returns>График заказа, если существует.</returns>
        OrderTimeline GetOrderTimeline(Guid? id);

        /// <summary>
        /// Удаляет из хранилища график заказа по его ID.
        /// </summary>
        /// <param name="id">Код графика заказа.</param>
        void DeleteOrderTimeline(Guid? id);

        /// <summary>
        /// Получает номер заказа по коду заказа.
        /// </summary>
        /// <param name="repairOrderID">Код заказа.</param>
        /// <returns>Номер заказа.</returns>
        string GetRepairOrderNumber(Guid? repairOrderID);

        /// <summary>
        /// Получает список документов по определенному типу.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя. </param>
        /// <param name="documentKindID">Код типа документа.</param>
        /// <returns>Список документов.</returns>
        IEnumerable<CustomReportItem> GetCustomReportItems(Guid? userDomainID, byte? documentKindID);

        /// <summary>
        /// Возвращает список заказов в работе определенных пользователей с фильтром.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="engineerID">Код связанного инженера или null. </param>
        /// <param name="managerID">Код связанного менеджера или null.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список заказов.</returns>
        IEnumerable<RepairOrderDTO> GetWorkRepairOrders(Guid? userDomainID,Guid? engineerID,Guid? managerID, string name, int page, int pageSize, out int count);

        /// <summary>
        /// Получает пункты истории изменений по конкретному заказу.
        /// </summary>
        /// <param name="repairOrderID">Код заказа.</param>
        /// <returns>Список пунктов истории.</returns>
        IQueryable<OrderTimeline> GetOrderTimelines(Guid? repairOrderID);

        /// <summary>
        /// Получает список пунктов выполненных работ для определенного заказа.
        /// </summary>
        /// <param name="repairOrderID">Код заказа.</param>
        /// <returns>Список пунктов заказа.</returns>
        IQueryable<WorkItemDTO> GetWorkItems(Guid? repairOrderID);

        /// <summary>
        /// Получает список пунктов установленных запчастей для определенного заказа.
        /// </summary>
        /// <param name="repairOrderID">Код заказа.</param>
        /// <returns>Список пунктов запчастей.</returns>
        IQueryable<DeviceItemDTO> GetDeviceItems(Guid? repairOrderID);

        /// <summary>
        /// Получает статистическую информацию по установленным запчастям определенного заказа.
        /// </summary>
        /// <param name="repairOrderID">Заказ.</param>
        /// <returns>Статистика.</returns>
        ItemsInfo GetDeviceItemsTotal(Guid? repairOrderID);

        /// <summary>
        /// Получает статистическую информацию по выполненным работам определенного заказа.
        /// </summary>
        /// <param name="repairOrderID">Заказ.</param>
        /// <returns>Статистика.</returns>
        ItemsInfo GetWorkItemsTotal(Guid? repairOrderID);

        /// <summary>
        /// Возвращает информацию о связах конкретного филиала с пользователями определенной роли.
        /// </summary>
        /// <param name="branchId">Код пользователя.</param>
        /// <param name="userProjectRoleId">Код роли.</param>
        /// <returns>Филиалы.</returns>
        IEnumerable<UserBranchMapItemDTO> GetUserBranchMapByItemsByBranch(Guid? branchId, byte? userProjectRoleId);

        /// <summary>
        ///   Сохраняет информацию домене пользователей.
        /// </summary>
        /// <param name="userDomain"> Сохраняемый домен пользователей. </param>
        void SaveUserDomain(UserDomain userDomain);

        /// <summary>
        /// Получает домен пользователя по его ID.
        /// </summary>
        /// <param name="id">Код домена пользователя.</param>
        /// <returns>Домен пользователя, если существует.</returns>
        UserDomain GetUserDomain(Guid? id);

        /// <summary>
        /// Удаляет из хранилища домен пользователя по его ID.
        /// </summary>
        /// <param name="id">Домен пользователя.</param>
        void DeleteUserDomain(Guid? id);

        /// <summary>
        ///   Сохраняет информацию о типе заказа.
        /// </summary>
        /// <param name="orderKind"> Сохраняемый тип заказа. </param>
        void SaveOrderKind(OrderKind orderKind);

        /// <summary>
        /// Получает тип заказа по его ID.
        /// </summary>
        /// <param name="id">Код типа заказа.</param>
        /// <param name="userDomainID">Код домена пользователя. </param>
        /// <returns>Тип заказа, если существует.</returns>
        OrderKind GetOrderKind(Guid? id, Guid? userDomainID);

        /// <summary>
        /// Удаляет из хранилища тип заказа по его ID.
        /// </summary>
        /// <param name="id">Код типа заказа.</param>
        void DeleteOrderKind(Guid? id);

        /// <summary>
        ///   Сохраняет информацию о статусе заказа.
        /// </summary>
        /// <param name="orderStatus"> Сохраняемый статус заказа. </param>
        void SaveOrderStatus(OrderStatus orderStatus);

        /// <summary>
        /// Получает статус заказа по его ID.
        /// </summary>
        /// <param name="id">Код статуса заказа.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Статус заказа, если существует.</returns>
        OrderStatus GetOrderStatus(Guid? id, Guid? userDomainID);

        /// <summary>
        /// Удаляет из хранилища статус заказа по его ID.
        /// </summary>
        /// <param name="id">Код статуса заказа.</param>
        void DeleteOrderStatus(Guid? id);

        /// <summary>
        /// Получает список статусов заказов с фильтром.
        /// </summary>
        /// <param name="name">Строка поиска.</param>
        /// <param name="userDomainID">Код домена пользователя. </param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список статусов заказа.</returns>
        IEnumerable<OrderStatus> GetOrderStatuses(Guid? userDomainID,string name, int page, int pageSize, out int count);

        /// <summary>
        /// Получает код домена пользователя по коду заказа.
        /// </summary>
        /// <param name="repairOrderID">Код заказа.</param>
        /// <returns>Код домена пользователя.</returns>
        Guid? GetRepairOrderUserDomainID(Guid? repairOrderID);

        /// <summary>
        /// Вызывает скрипт развертывания данных в определенном домене.
        /// </summary>
        /// <param name="userDomainID">Код домена.</param>
        void Deploy(Guid? userDomainID);

        /// <summary>
        /// Проверяет наличие логина в системе.
        /// </summary>
        /// <param name="login">Логин.</param>
        /// <returns>Наличие логина.</returns>
        bool UserLoginExists(string login);

        /// <summary>
        /// Проверяет на наличие email в доменах регистрации.
        /// </summary>
        /// <param name="email">Электронный адрес для проверки.</param>
        /// <returns>Признак наличия email.</returns>
        bool UserDomainEmailIsExists(string email);

        /// <summary>
        /// Осуществляет проверку, что логин пользователя существует, но не активирован.
        /// </summary>
        /// <param name="login">Логин пользователя.</param>
        /// <returns>Существование логина и его неактивация.</returns>
        bool UserDomainLoginIsExistsAndNonActivated(string login);

        /// <summary>
        /// Удаляет из хранилища график заказа по ID заказа.
        /// </summary>
        /// <param name="repairOrderID">Код заказа.</param>
        void DeleteOrderTimelineByRepairOrder(Guid? repairOrderID);

        /// <summary>
        /// Получает пункты отчета для работы инженеров.
        /// </summary>
        /// <param name="userDomainID">Код домена.</param>
        /// <param name="engineerID">Код инженера, может быть null, тогда собираются данные по всем инженерам.</param>
        /// <param name="beginDate">Дата начала.</param>
        /// <param name="endDate">Дата окончания периода.</param>
        /// <returns>Списко пунктов отчета.</returns>
        IEnumerable<EngineerWorkReportItem> GetEngineerWorkReportItems(Guid? userDomainID,Guid? engineerID,DateTime beginDate,DateTime endDate);

        /// <summary>
        /// Удаляет из хранилища запчасти заказа по ID заказа.
        /// </summary>
        /// <param name="repairOrderID">Код заказа.</param>
        void DeleteDeviceItemByRepairOrder(Guid? repairOrderID);

        /// <summary>
        /// Удаляет из хранилища работы заказа по ID заказа.
        /// </summary>
        /// <param name="repairOrderID">Код заказа.</param>
        void DeleteWorkItemByRepairOrder(Guid? repairOrderID);

        /// <summary>
        /// Вычисление суммы по выполненным работам конеретного заказа.
        /// </summary>
        /// <param name="repairOrderID">Код заказа.</param>
        /// <returns>Значение суммы.</returns>
        decimal? GetWorkItemsSum(Guid? repairOrderID);

        /// <summary>
        /// Вычисление суммы по установленным запчастям конеретного заказа.
        /// </summary>
        /// <param name="repairOrderID">Код заказа.</param>
        /// <returns>Значение суммы.</returns>
        decimal? GetDeviceItemsSum(Guid? repairOrderID);

        /// <summary>
        /// Получает список финансовых групп с фильтром.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список финансовых групп.</returns>
        IEnumerable<FinancialGroupItem> GetFinancialGroupItems(Guid? userDomainID, string name, int page, int pageSize, out int count);

        /// <summary>
        /// Получает список финансовая групп без фильтра.
        /// </summary>
        /// <returns>Список финансовых группаов.</returns>
        IQueryable<FinancialGroupItem> GetFinancialGroupItems(Guid? userDomainID);

        /// <summary>
        ///   Сохраняет информацию финансовой группе.
        /// </summary>
        /// <param name="financialGroupItem"> Сохраняемая финансовая группа. </param>
        void SaveFinancialGroupItem(FinancialGroupItem financialGroupItem);

        /// <summary>
        /// Получает финансовую группу по его ID.
        /// </summary>
        /// <param name="id">Код описания финансовой группы.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Финансовая группа, если существует.</returns>
        FinancialGroupItem GetFinancialGroupItem(Guid? id, Guid? userDomainID);

        /// <summary>
        /// Удаляет из хранилища финансовую группу по его ID.
        /// </summary>
        /// <param name="id">Код финансовой группы.</param>
        void DeleteFinancialGroupItem(Guid? id);

        /// <summary>
        /// Получает список финансовых статей с фильтром.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список финансовых статей.</returns>
        IEnumerable<FinancialItem> GetFinancialItems(Guid? userDomainID, string name, int page, int pageSize, out int count);

        /// <summary>
        ///   Сохраняет информацию финансовой статье.
        /// </summary>
        /// <param name="financialItem"> Сохраняемая финансовая статья. </param>
        void SaveFinancialItem(FinancialItem financialItem);

        /// <summary>
        /// Получает финансовую статью по его ID.
        /// </summary>
        /// <param name="id">Код описания финансовой статьи.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Финансовая статья, если существует.</returns>
        FinancialItem GetFinancialItem(Guid? id, Guid? userDomainID);

        /// <summary>
        /// Удаляет из хранилища финансовую статью по его ID.
        /// </summary>
        /// <param name="id">Код финансовой статьи.</param>
        void DeleteFinancialItem(Guid? id);

        /// <summary>
        /// Возвращает информацию о связах конкретного финансовой группы с филиалами.
        /// </summary>
        /// <param name="financialGroupID">Код группы.</param>
        /// <returns>Филиалы.</returns>
        IEnumerable<FinancialGroupBranchMapItemDTO> GetFinancialGroupBranchMapItemsByFinancialGroup(Guid? financialGroupID);

        /// <summary>
        /// Удаляет все связанные с финансовой группы филиалы.
        /// </summary>
        /// <param name="financialGroupID">Код финансовой группы.</param>
        void DeleteFinancialGroupBranchMapItems(Guid? financialGroupID);

        /// <summary>
        ///   Сохраняет информацию о соответствии финансовой группы и филиала.
        /// </summary>
        /// <param name="financialGroupBranchMapItem"> Сохраняемое соответствие. </param>
        void SaveFinancialGroupMapBranchItem(FinancialGroupBranchMapItem financialGroupBranchMapItem);

        /// <summary>
        /// Получает список значений статей расходов.
        /// </summary>
        /// <param name="financialGroupID">Код финансовой группы пользователя.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="endDate">Дата окончания. </param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <param name="beginDate">Дата окончания.</param>
        /// <returns>Список значений финансовых статей.</returns>
        IEnumerable<FinancialItemValueDTO> GetFinancialItemValues(Guid? financialGroupID, Guid? userDomainID, string name, DateTime beginDate, DateTime endDate, int page, int pageSize, out int count);

        /// <summary>
        ///   Сохраняет информацию по значению финансовой статьи.
        /// </summary>
        /// <param name="financialItemValue"> Сохраняемое значение финансовой статьи. </param>
        void SaveFinancialItemValue(FinancialItemValue financialItemValue);

        /// <summary>
        /// Получает значение финансовой статьи по ее ID.
        /// </summary>
        /// <param name="id">Код значения финансовой статьи.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Значение финансовой статьи, если существует.</returns>
        FinancialItemValueDTO GetFinancialItemValue(Guid? id, Guid? userDomainID);

        /// <summary>
        /// Удаляет из хранилища финансовую статью по ее ID.
        /// <remarks>Проверять перед удалением на доступ пользователя к значению финансовой статьи.</remarks>
        /// </summary>
        /// <param name="id">Код финансовой статьи.</param>
        void DeleteFinancialItemValue(Guid? id);

        /// <summary>
        /// Получение отчета по пользовательским данным по расходам и доходам.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="financialGroupID">Код финансовой группы филиалов.</param>
        /// <param name="beginDate">Дата начала.</param>
        /// <param name="endDate">Дата завершения.</param>
        /// <returns>Список пунктов отчета.</returns>
        IEnumerable<RevenueAndExpenditureReportItem> GetRevenueAndExpenditureReportItems(Guid? userDomainID, Guid? financialGroupID, DateTime beginDate, DateTime endDate);

        /// <summary>
        /// Получает финансовую статью по его коду типа.
        ///<remarks>Если статей несколько, тогда берется самая последняя.</remarks>
        /// </summary>
        /// <param name="financialItemKindID">Код типа финансовой статьи.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Финансовая статья, если существует.</returns>
        FinancialItem GetFinancialItemByFinancialItemKind(int? financialItemKindID, Guid? userDomainID);

        /// <summary>
        /// Получает общую информацию по установленным запчастям и выполненным работам за определенный период выдачи клиентам для фин группы.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="financialGroupID">Код финансовой группы.</param>
        /// <param name="beginDate">Дата начала.</param>
        /// <param name="endDate">Дата завершения.</param>
        /// <returns>Информация.</returns>
        ItemsInfo GetOrderPaidAmountByOrderIssueDate(Guid? userDomainID, Guid? financialGroupID, DateTime beginDate, DateTime endDate);

        /// <summary>
        /// Получает отчет по используемым запчастям за определенный период времени.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="branchID">Код филиала.</param>
        /// <param name="financialGroupID">Код финансовой группы.</param>
        /// <param name="beginDate">Дата начала.</param>
        /// <param name="endDate">Дата окончания.</param>
        /// <returns>Пункты отчета.</returns>
        IEnumerable<UsedDeviceItemsReportItem> GetUsedDeviceItemsReportItems(Guid? userDomainID, Guid? branchID, Guid? financialGroupID, DateTime beginDate, DateTime endDate);

        /// <summary>
        /// Получает список категорий товара с фильтром.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список категорий товаров.</returns>
        IEnumerable<ItemCategory> GetItemCategories(Guid? userDomainID, string name, int page, int pageSize, out int count);

        /// <summary>
        ///   Сохраняет информацию о кактегории товара.
        /// </summary>
        /// <param name="itemCategory"> Сохраняемая категория товара. </param>
        void SaveItemCategory(ItemCategory itemCategory);

        /// <summary>
        /// Получает категорию товара по его ID.
        /// </summary>
        /// <param name="id">Код категории товара.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Категория товара, если существует.</returns>
        ItemCategory GetItemCategory(Guid? id, Guid? userDomainID);

        /// <summary>
        /// Удаляет из хранилища категорию товара по его ID.
        /// </summary>
        /// <param name="id">Код категории товара.</param>
        void DeleteItemCategory(Guid? id);

        /// <summary>
        /// Получает список складов с фильтром.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список складов товаров.</returns>
        IEnumerable<Warehouse> GetWarehouses(Guid? userDomainID, string name, int page, int pageSize, out int count);

        /// <summary>
        /// Получает склад по его ID.
        /// </summary>
        /// <param name="id">Код склада.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Склад товара, если существует.</returns>
        Warehouse GetWarehouse(Guid? id, Guid? userDomainID);

        /// <summary>
        /// Удаляет из хранилища склад по его ID.
        /// </summary>
        /// <param name="id">Код склада товара.</param>
        void DeleteWarehouse(Guid? id);

        /// <summary>
        /// Получает список номенклатуры товара с фильтром.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список номенклатуры.</returns>
        IEnumerable<GoodsItemDTO> GetGoodsItems(Guid? userDomainID, string name, int page, int pageSize, out int count);

        /// <summary>
        ///   Сохраняет информацию о номенклатуре.
        /// </summary>
        /// <param name="goodsItem"> Сохраняемая номенклатура. </param>
        void SaveGoodsItem(GoodsItem goodsItem);

        /// <summary>
        /// Получает номенклатуру по его ID.
        /// </summary>
        /// <param name="id">Код номенклатуры.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Номенклатура товара, если существует.</returns>
        GoodsItemDTO GetGoodsItem(Guid? id, Guid? userDomainID);

        /// <summary>
        /// Удаляет из хранилища номенклатуры по его ID.
        /// </summary>
        /// <param name="id">Код номенклатуры товара.</param>
        void DeleteGoodsItem(Guid? id);

        /// <summary>
        /// Получает список остатков на складе с фильтром.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="warehouseID">Код склада. </param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список остатков.</returns>
        IEnumerable<WarehouseItemDTO> GetWarehouseItems(Guid? userDomainID, Guid? warehouseID, string name, int page, int pageSize, out int count);

        /// <summary>
        ///   Сохраняет информацию о остатках на складе.
        /// </summary>
        ///<remarks>Проверять домен на наличие склада перед вызовом.</remarks>
        /// <param name="warehouseItem"> Сохраняемые остатки на складе. </param>
        void SaveWarehouseItem(WarehouseItem warehouseItem);

        /// <summary>
        /// Получает остаток на складе по его ID.
        /// </summary>
        /// <param name="id">Код номенклатуры.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Остатки на складе товара, если существует.</returns>
        WarehouseItemDTO GetWarehouseItem(Guid? id, Guid? userDomainID);

        /// <summary>
        /// Удаляет из хранилища остаток на складе по его ID.
        /// </summary>
        /// <param name="id">Код номенклатуры товара.</param>
        void DeleteWarehouseItem(Guid? id);

        /// <summary>
        /// Получает список контрагентов с фильтром.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список контрагентов товаров.</returns>
        IEnumerable<Contractor> GetContractors(Guid? userDomainID, string name, int page, int pageSize, out int count);

        /// <summary>
        ///   Сохраняет информацию о контрагенте.
        /// </summary>
        /// <param name="contractor"> Сохраняемый контрагент. </param>
        void SaveContractor(Contractor contractor);

        /// <summary>
        /// Получает контрагента по его ID.
        /// </summary>
        /// <param name="id">Код контрагента.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Контрагент, если существует.</returns>
        Contractor GetContractor(Guid? id, Guid? userDomainID);

        /// <summary>
        /// Удаляет из хранилища контрагент по его ID.
        /// </summary>
        /// <param name="id">Код контрагента.</param>
        void DeleteContractor(Guid? id);

        /// <summary>
        /// Получает список приходных накладных с фильтром.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="warehouseID">Код склада.</param>
        /// <param name="endDate">Дата окончания накладных.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <param name="beginDate">Дата начала создания накладных.</param>
        /// <returns>Список приходных накладных товаров.</returns>
        IEnumerable<IncomingDocDTO> GetIncomingDocs(Guid? userDomainID, Guid? warehouseID, DateTime beginDate, DateTime endDate, string name, int page, int pageSize, out int count);

        /// <summary>
        ///   Сохраняет информацию о приходная накладной.
        /// </summary>
        /// <param name="incomingDoc"> Сохраняемая приходная накладная. </param>
        void SaveIncomingDoc(IncomingDoc incomingDoc);

        /// <summary>
        /// Получает приходную накладнаую по его ID.
        /// </summary>
        /// <param name="id">Код приходной накладной.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Приходная накладная, если существует.</returns>
        IncomingDocDTO GetIncomingDoc(Guid? id, Guid? userDomainID);

        /// <summary>
        /// Удаляет из хранилища приходную накладную по его ID.
        /// </summary>
        /// <param name="id">Код приходной накладной.</param>
        void DeleteIncomingDoc(Guid? id);

        /// <summary>
        /// Получение кода домена для остатка на складе.
        /// </summary>
        /// <param name="id">Код остатка на складе.</param>
        /// <returns>Код домена, если элемент существует.</returns>
        Guid? GetWarehouseItemUserDomainID(Guid? id);

        /// <summary>
        /// Получение кода домена для приходной накладной.
        /// </summary>
        /// <param name="id">Код приходной накладной.</param>
        /// <returns>Код домена, если элемент существует.</returns>
        Guid? GetIncomingDocUserDomainID(Guid? id);

        /// <summary>
        /// Получает список элемент приходной накладной с фильтром.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="incomingDocID">Код приходной накладной.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список элемент приходной накладнойов товаров.</returns>
        IEnumerable<IncomingDocItemDTO> GetIncomingDocItems(Guid? userDomainID,Guid? incomingDocID, string name, int page, int pageSize, out int count);

        /// <summary>
        ///   Сохраняет информацию об элементе приходной накладной.
        /// </summary>
        /// <remarks>Нужна обязательная проверка перед вызовом на доступ к вмененной накладной пользователя.</remarks>
        /// <param name="incomingDocItem"> Сохраняемый элемент приходной накладной. </param>
        void SaveIncomingDocItem(IncomingDocItem incomingDocItem);

        /// <summary>
        /// Получает элемент приходной накладной по его ID.
        /// </summary>
        /// <param name="id">Код элемента приходной накладной.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Элемент приходной накладной, если существует.</returns>
        IncomingDocItemDTO GetIncomingDocItem(Guid? id, Guid? userDomainID);

        /// <summary>
        /// Получение кода домена для пункта приходной накладной.
        /// </summary>
        /// <param name="id">Код пункта приходной накладной.</param>
        /// <returns>Код домена, если элемент существует.</returns>
        Guid? GetIncomingDocItemUserDomainID(Guid? id);

        /// <summary>
        /// Удаляет из хранилища элемент приходной накладной по его ID.
        /// </summary>
        /// <param name="id">Код элемента приходной накладной товара.</param>
        void DeleteIncomingDocItem(Guid? id);

        /// <summary>
        /// Получает список списаний со склада с фильтром.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="warehouseID">Код склада.</param>
        /// <param name="endDate">Дата окончания накладных.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <param name="beginDate">Дата начала создания накладных.</param>
        /// <returns>Список списаний со склада товаров.</returns>
        IEnumerable<CancellationDocDTO> GetCancellationDocs(Guid? userDomainID, Guid? warehouseID, DateTime beginDate, DateTime endDate, string name, int page, int pageSize, out int count);

        /// <summary>
        ///   Сохраняет информацию о списании со склада .
        /// </summary>
        /// <param name="cancellationDoc"> Сохраняемое списание со склада. </param>
        void SaveCancellationDoc(CancellationDoc cancellationDoc);

        /// <summary>
        /// Получает списание со склада по его ID.
        /// </summary>
        /// <param name="id">Код списания со склада.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Списание со склада, если существует.</returns>
        CancellationDocDTO GetCancellationDoc(Guid? id, Guid? userDomainID);

        /// <summary>
        /// Получение кода домена для списания со склада.
        /// </summary>
        /// <param name="id">Код списания со склада.</param>
        /// <returns>Код домена, если элемент существует.</returns>
        Guid? GetCancellationDocUserDomainID(Guid? id);

        /// <summary>
        /// Удаляет из хранилища списание со склада по ее ID.
        /// </summary>
        /// <param name="id">Код списания со склада.</param>
        void DeleteCancellationDoc(Guid? id);

        /// <summary>
        /// Получает список элемент документа о списании с фильтром.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="cancellationDocID">Код документа о списании.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список элементов документа о списании товаров.</returns>
        IEnumerable<CancellationDocItemDTO> GetCancellationDocItems(Guid? userDomainID, Guid? cancellationDocID, string name, int page, int pageSize, out int count);

        /// <summary>
        ///   Сохраняет информацию об элементе документа о списании.
        /// </summary>
        /// <remarks>Нужна обязательная проверка перед вызовом на доступ к вмененной накладной пользователя.</remarks>
        /// <param name="cancellationDocItem"> Сохраняемый элемент документа о списании. </param>
        void SaveCancellationDocItem(CancellationDocItem cancellationDocItem);

        /// <summary>
        /// Получает элемент документа о списании по его ID.
        /// </summary>
        /// <param name="id">Код элемента документа о списании.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Элемент документа о списании, если существует.</returns>
        CancellationDocItemDTO GetCancellationDocItem(Guid? id, Guid? userDomainID);

        /// <summary>
        /// Получение кода домена для пункта документа о списании.
        /// </summary>
        /// <param name="id">Код пункта документа о списании.</param>
        /// <returns>Код домена, если элемент существует.</returns>
        Guid? GetCancellationDocItemUserDomainID(Guid? id);

        /// <summary>
        /// Удаляет из хранилища элемент документа о списании по его ID.
        /// </summary>
        /// <param name="id">Код элемента документа о списании товара.</param>
        void DeleteCancellationDocItem(Guid? id);

        /// <summary>
        ///   Сохраняет информацию о перемещении со склада .
        /// </summary>
        /// <param name="transferDoc"> Сохраняемое перемещение со склада. </param>
        void SaveTransferDoc(TransferDoc transferDoc);

        /// <summary>
        /// Получает перемещение со склада по его ID.
        /// </summary>
        /// <param name="id">Код перемещения со склада.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Перемещение со склада, если существует.</returns>
        TransferDocDTO GetTransferDoc(Guid? id, Guid? userDomainID);

        /// <summary>
        /// Получение кода домена для перемещения со склада.
        /// </summary>
        /// <param name="id">Код перемещения со склада.</param>
        /// <returns>Код домена, если элемент существует.</returns>
        Guid? GetTransferDocUserDomainID(Guid? id);

        /// <summary>
        /// Удаляет из хранилища перемещение со склада по ее ID.
        /// </summary>
        /// <param name="id">Код перемещения со склада.</param>
        void DeleteTransferDoc(Guid? id);

        /// <summary>
        /// Получает список перемещений со склада на склад с фильтром.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="senderWarehouseID">Код склада с которого делают перемещение.</param>
        /// <param name="endDate">Дата окончания накладных.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <param name="recipientWarehouseID">Код склада на который делают перемещение. </param>
        /// <param name="beginDate">Дата начала создания накладных.</param>
        /// <returns>Список перемещений со склада товаров.</returns>
        IEnumerable<TransferDocDTO> GetTransferDocs(Guid? userDomainID, Guid? senderWarehouseID, Guid? recipientWarehouseID, DateTime beginDate, DateTime endDate, string name, int page, int pageSize, out int count);

        /// <summary>
        /// Получает список элемент документа о перемещении с фильтром.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="transferDocID">Код документа о перемещении.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список элементов документа о перемещении товаров.</returns>
        IEnumerable<TransferDocItemDTO> GetTransferDocItems(Guid? userDomainID, Guid? transferDocID, string name, int page, int pageSize, out int count);

        /// <summary>
        ///   Сохраняет информацию об элементе документа о перемещении.
        /// </summary>
        /// <remarks>Нужна обязательная проверка перед вызовом на доступ к вмененной накладной пользователя.</remarks>
        /// <param name="transferDocItem"> Сохраняемый элемент документа о перемещении. </param>
        void SaveTransferDocItem(TransferDocItem transferDocItem);

        /// <summary>
        /// Получает элемент документа о перемещении по его ID.
        /// </summary>
        /// <param name="id">Код элемента документа о перемещении.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Элемент документа о перемещении, если существует.</returns>
        TransferDocItemDTO GetTransferDocItem(Guid? id, Guid? userDomainID);

        /// <summary>
        /// Получение кода домена для пункта документа о перемещении.
        /// </summary>
        /// <param name="id">Код пункта документа о перемещении.</param>
        /// <returns>Код домена, если элемент существует.</returns>
        Guid? GetTransferDocItemUserDomainID(Guid? id);

        /// <summary>
        /// Удаляет из хранилища элемент документа о перемещении по его ID.
        /// </summary>
        /// <param name="id">Код элемента документа о перемещении товара.</param>
        void DeleteTransferDocItem(Guid? id);

        /// <summary>
        ///   Сохраняет информацию о обработанный документе.
        /// </summary>
        /// <param name="processedWarehouseDoc"> Сохраняемый обработанный документ. </param>
        void SaveProcessedWarehouseDoc(ProcessedWarehouseDoc processedWarehouseDoc);

        /// <summary>
        /// Получает обработанный документ по его ID.
        /// </summary>
        /// <param name="id">Код обработанный документа.</param>
        /// <returns>Обработанный документ, если существует.</returns>
        ProcessedWarehouseDoc GetProcessedWarehouseDoc(Guid? id);

        /// <summary>
        /// Удаляет из хранилища обработанный документ по его ID.
        /// </summary>
        /// <param name="id">Код обработанный документа.</param>
        void DeleteProcessedWarehouseDoc(Guid? id);

        /// <summary>
        ///   Сохраняет информацию о складе.
        /// </summary>
        /// <param name="warehouse"> Сохраняемый склад. </param>
        void SaveWarehouse(Warehouse warehouse);

        /// <summary>
        /// Получение кода домена для склада.
        /// </summary>
        /// <param name="id">Код склада.</param>
        /// <returns>Код домена, если элемент существует.</returns>
        Guid? GetWarehouseUserDomainID(Guid? id);

        /// <summary>
        /// Получает список всех категорий товара для домена.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Список категорий товаров.</returns>
        IQueryable<ItemCategory> GetItemCategories(Guid? userDomainID);

        /// <summary>
        /// Получает список всех складов для домена.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Список складов товаров.</returns>
        IQueryable<Warehouse> GetWarehouses(Guid? userDomainID);

        /// <summary>
        /// Получает список всех контрагентов для домена.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Список контрагентов.</returns>
        IQueryable<Contractor> GetContractors(Guid? userDomainID);

        /// <summary>
        /// Проверяет на признак обработки пользователем конкретного складского документа.
        /// </summary>
        /// <param name="docID">Код складского документа.</param>
        /// <returns>Признак обработки.</returns>
        bool WarehouseDocIsProcessed(Guid? docID);

        /// <summary>
        /// Обрабатывает пункты приходной накладной.
        /// </summary>
        /// <param name="incomingDocID">Код приходной накладной.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="eventDate">Дата обработи документа </param>
        /// <param name="utcEventDateTime">UTC дата и время обработки документа. </param>
        /// <param name="userID">Код обработавшего пользователя.</param>
        ProcessWarehouseDocResult ProcessIncomingDocItems(Guid? incomingDocID, Guid? userDomainID,DateTime eventDate,DateTime utcEventDateTime,Guid? userID);

        /// <summary>
        /// Обрабатывает пункты документа о списании.
        /// </summary>
        /// <param name="cancellationDocID">Код документа о списании.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="eventDate">Дата обработи документа </param>
        /// <param name="utcEventDateTime">UTC дата и время обработки документа. </param>
        /// <param name="userID">Код обработавшего пользователя.</param>
        ProcessWarehouseDocResult ProcessCancellationDocItems(Guid? cancellationDocID, Guid? userDomainID, DateTime eventDate, DateTime utcEventDateTime, Guid? userID);

        /// <summary>
        /// Обрабатывает пункты документов перемещения остатков со склада на склад.
        /// </summary>
        /// <param name="transferDocID">Код документа перемещения.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="eventDate">Дата обработи документа </param>
        /// <param name="utcEventDateTime">UTC дата и время обработки документа. </param>
        /// <param name="userID">Код обработавшего пользователя.</param>
        ProcessWarehouseDocResult ProcessTransferDocItems(Guid? transferDocID, Guid? userDomainID, DateTime eventDate, DateTime utcEventDateTime, Guid? userID);

        /// <summary>
        /// Получает список всех элементов приходной накладной.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="incomingDocID">Код приходной накладной.</param>
        /// <returns>Список элемент приходной накладнойов товаров.</returns>
        IQueryable<IncomingDocItemDTO> GetIncomingDocItems(Guid? userDomainID, Guid? incomingDocID);

        /// <summary>
        /// Получает пункты отчета по движениям на складе.
        /// </summary>
        /// <param name="userDomainID">Код домена.</param>
        /// <param name="warehouseID">Код склада.</param>
        /// <param name="beginDate">Дата начала.</param>
        /// <param name="endDate">Дата окончания.</param>
        /// <returns>Пункты отчета.</returns>
        IEnumerable<WarehouseFlowReportItem> GetWarehouseFlowReportItems(Guid? userDomainID,Guid? warehouseID,DateTime beginDate, DateTime endDate);

        /// <summary>
        /// Отменяет обработку пунктов приходной накладной.
        /// </summary>
        /// <param name="incomingDocID">Код приходной накладной.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        ProcessWarehouseDocResult UnProcessIncomingDocItems(Guid? incomingDocID, Guid? userDomainID);

        /// <summary>
        /// Отменяет обработку пунктов документа о списании.
        /// </summary>
        /// <param name="cancellationDocID">Код документа о списании.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        ProcessWarehouseDocResult UnProcessCancellationDocItems(Guid? cancellationDocID, Guid? userDomainID);

        /// <summary>
        /// Отменяет обработанные пункты пункты документов перемещения остатков со склада на склад.
        /// </summary>
        /// <param name="transferDocID">Код отменяемого документа перемещения.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        ProcessWarehouseDocResult UnProcessTransferDocItems(Guid? transferDocID, Guid? userDomainID);

        /// <summary>
        /// Возвращает информацию о связах конкретного финансовой группы со складами.
        /// </summary>
        /// <param name="financialGroupID">Код группы.</param>
        /// <returns>Склады.</returns>
        IEnumerable<FinancialGroupWarehouseMapItemDTO> GetFinancialGroupWarehouseMapItemsByFinancialGroup(Guid? financialGroupID);

        /// <summary>
        /// Удаляет все связанные с финансовой группы склады.
        /// </summary>
        /// <param name="financialGroupID">Код финансовой группы.</param>
        void DeleteFinancialGroupWarehouseMapItems(Guid? financialGroupID);

        /// <summary>
        ///   Сохраняет информацию о соответствии финансовой группы и склада.
        /// </summary>
        /// <param name="financialGroupWarehouseMapItem"> Сохраняемое соответствие. </param>
        void SaveFinancialGroupMapWarehouseItem(FinancialGroupWarehouseMapItem financialGroupWarehouseMapItem);

        /// <summary>
        /// Получает соответствия финансовой группы и склада по его ID.
        /// </summary>
        /// <param name="id">Код соответствия.</param>
        /// <returns>Соостветствие, если существует.</returns>
        FinancialGroupWarehouseMapItem GetFinancialGroupMapWarehouseItem(Guid? id);

        /// <summary>
        /// Удаляет из хранилища соответствие между финансовой группой и складом по его ID.
        /// </summary>
        /// <param name="id">Код соответствия.</param>
        void DeleteFinancialGroupWarehouseMapItem(Guid? id);

        /// <summary>
        /// Получение пункты отчета по завершенным приходным накладным для финансовой группы за определенный период.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="financialGroupID">Код финансовой группы.</param>
        /// <param name="beginDate">Дата начала.</param>
        /// <param name="endDate">Дата окончания.</param>
        /// <returns>Пункты отчета.</returns>
        IEnumerable<WarehouseDocTotalItem> GetWarehouseDocTotalItems(Guid? userDomainID, Guid? financialGroupID, DateTime beginDate, DateTime endDate);

        /// <summary>
        /// Получает списко заказаов за определенный период.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя. </param>
        /// <param name="beginDate">Дата начала.</param>
        /// <param name="endDate">Дата окончания.</param>
        /// <returns>Заказы.</returns>
        IEnumerable<RepairOrder> GetRepairOrders(Guid? userDomainID,DateTime beginDate, DateTime endDate);

        /// <summary>
        ///   Сохраняет информацию о пункте восстановления пароля.
        /// </summary>
        /// <param name="recoveryLoginItem"> Сохраняемый пункт восставноления пароля. </param>
        void SaveRecoveryLoginItem(RecoveryLoginItem recoveryLoginItem);

        /// <summary>
        /// Получает пункт восстановления пароля по его ID.
        /// </summary>
        /// <param name="id">Код восстановления пароля.</param>
        /// <returns>Пункт восстановления пароля, если существует.</returns>
        RecoveryLoginItem GetRecoveryLoginItem(Guid? id);

        /// <summary>
        /// Удаляет из хранилища пункт восстановления пароя по его ID.
        /// </summary>
        /// <param name="id">Код пункта восстановления пароля.</param>
        void DeleteRecoveryLoginItem(Guid? id);

        /// <summary>
        /// Произвоит поиск пункта восстановления пароля по номеру восстановления.
        /// </summary>
        /// <param name="number">Номер восстановления.</param>
        /// <returns>Пункт для восстановления.</returns>
        RecoveryLoginItem GetRecoveryLoginItem(string number);

        /// <summary>
        ///   Сохраняет информацию о публичном ключе пользователя.
        /// </summary>
        /// <param name="userPublicKey"> Сохраняемый ключ пользователя. </param>
        void SaveUserPublicKey(UserPublicKey userPublicKey);

        /// <summary>
        /// Получает публичный ключ пользователя по его ID.
        /// </summary>
        /// <param name="id">Код публичного ключа пользователя.</param>
        /// <param name="userDomainID">Код домена пользователя. </param>
        /// <returns>Публичный ключ пользователя, если существует.</returns>
        UserPublicKeyDTO GetUserPublicKey(Guid? id, Guid? userDomainID);

        /// <summary>
        /// Удаляет из хранилища публичный ключ пользователя по его ID.
        /// </summary>
        /// <param name="id">Код публичного ключа пользователя.</param>
        void DeleteUserPublicKey(Guid? id);

        /// <summary>
        ///   Сохраняет информацию о запросе публичного ключа пользователя.
        /// </summary>
        /// <param name="userPublicKeyRequest"> Сохраняемый закпрос публичного ключа пользователя. </param>
        void SaveUserPublicKeyRequest(UserPublicKeyRequest userPublicKeyRequest);

        /// <summary>
        /// Получает список запросов на регистрацию публичных ключей с фильтром.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список запросов на публичные ключи.</returns>
        IEnumerable<UserPublicKeyRequestDTO> GetUserPublicKeyRequests(Guid? userDomainID, string name, int page, int pageSize, out int count);

        /// <summary>
        /// Получает запрос публичного ключа пользователя по его ID.
        /// </summary>
        /// <param name="id">Код запроса публичного ключа пользователя.</param>
        /// <param name="userDomainID">Код домена пользователя. </param>
        /// <returns>Запрос публичного ключа пользователя, если существует.</returns>
        UserPublicKeyRequestDTO GetUserPublicKeyRequest(Guid? id, Guid? userDomainID);

        /// <summary>
        /// Удаляет из хранилища запрос публичного ключа пользователя по его ID.
        /// </summary>
        /// <param name="id">Код запроса публичного ключа пользователя.</param>
        void DeleteUserPublicKeyRequest(Guid? id);

        /// <summary>
        /// Получает список публичных ключей с фильтром.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список публичных ключей.</returns>
        IEnumerable<UserPublicKeyDTO> GetUserPublicKeys(Guid? userDomainID, string name, int page, int pageSize, out int count);

        /// <summary>
        /// Получает публичный ключ по номеру для домена.
        /// </summary>
        /// <param name="domainID">Код домена.</param>
        /// <param name="number">Номер ключа.</param>
        /// <returns>Публичный ключ.</returns>
        UserPublicKey GetPublicKey(Guid? domainID, string number);

        /// <summary>
        /// Получает запрос на регистрацию ключа по домену и номеру.
        /// </summary>
        /// <param name="domainID">Код домена.</param>
        /// <param name="number">Номер ключа.</param>
        /// <returns>Запрос на регистрацию публичного ключа.</returns>
        UserPublicKeyRequest GetUserPublicKeyRequest(Guid? domainID, string number);

        /// <summary>
        /// Получает текущий публичный ключ для пользователя.
        /// </summary>
        /// <param name="userID">Код домена.</param>
        /// <returns>Публичный ключ.</returns>
        UserPublicKey GetCurrentPublicKey(Guid? userID);

        /// <summary>
        /// Получает код домен пользователя по ID пользователя.
        /// </summary>
        /// <param name="userID">Код пользователя.</param>
        /// <returns>Код домена пользователя, если существует.</returns>
        Guid? GetUserDomainByUserID(Guid? userID);

        /// <summary>
        /// Возвращает информацию о связах филиалов и пользователей.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Связи филииалов и пользователей.</returns>
        IEnumerable<UserBranchMapItem> GetUserBranchMapItems(Guid? userDomainID);

        /// <summary>
        /// Возвращает информацию о связах домена финансовых групп с филиалами.
        /// </summary>
        /// <param name="userDomainID">Код домена.</param>
        /// <returns>Связи филиалов и фингрупп.</returns>
        IEnumerable<FinancialGroupBranchMapItem> GetFinancialGroupBranchMapItems(Guid? userDomainID);

        /// <summary>
        /// Возвращает информацию о связах домена финансовых групп с филиалами.
        /// </summary>
        /// <param name="userDomainID">Код домена.</param>
        /// <returns>Связи филиалов и фингрупп.</returns>
        IEnumerable<FinancialGroupWarehouseMapItem> GetFinancialGroupWarehouseMapItems(Guid? userDomainID);

        /// <summary>
        /// Получает весь список номенклатуры для домена.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Список номенклатуры.</returns>
        IQueryable<GoodsItem> GetGoodsItems(Guid? userDomainID);

        /// <summary>
        /// Получает список остатков на складе по домену пользователя.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Список остатков.</returns>
        IQueryable<WarehouseItemDTO> GetWarehouseItems(Guid? userDomainID);

        /// <summary>
        /// Получает следующие 40 хэшей заказов.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя. </param>
        /// <param name="lastRepairOrderID">Код заказа за которым необходимо взять все хэши.</param>
        /// <param name="totalCount">Общее количество заказов. </param>
        /// <returns>Список хэшей.</returns>
        IEnumerable<RepairOrderHash> GetRepairOrderHashes(Guid? userDomainID, Guid? lastRepairOrderID, out int totalCount);

        /// <summary>
        /// Получает заказа по его ID.
        /// </summary>
        /// <param name="id">Код заказа.</param>
        /// <param name="userDomainID">Код домена пользователя</param>
        /// <returns>Заказ, если существует.</returns>
        RepairOrder GetRepairOrderLight(Guid? id, Guid? userDomainID);

        /// <summary>
        /// Получает признак существования пункта графика заказа.
        /// </summary>
        /// <param name="orderTimeLineID">Код пункта графика заказа.</param>
        /// <returns>Признак существования.</returns>
        bool OrderTimeLineExists(Guid? orderTimeLineID);

        /// <summary>
        /// Получает список всех складов для пользователя.
        /// </summary>
        /// <param name="userID">Код пользователя.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Список складов товаров.</returns>
        IQueryable<Warehouse> GetWarehouses(Guid? userID,Guid? userDomainID);

        /// <summary>
        /// Получает список пунктов автозаполнения с фильтром.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователей. </param>
        /// <param name="autocompleteKindID">Код типа пункта автозаполнения.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список пунктов автозаполнения.</returns>
        IEnumerable<AutocompleteItem> GetAutocompleteItems(Guid? userDomainID, byte? autocompleteKindID, string name, int page, int pageSize, out int count);

        /// <summary>
        /// Получает полный список пунктов автозаполнения.
        /// </summary>
        /// <returns>Список пунктов автозаполнения.</returns>
        IQueryable<AutocompleteItem> GetAutocompleteItems(Guid? userDomainID);

        /// <summary>
        /// Получает полный список пунктов автозаполнения.
        /// </summary>
        /// <param name="userDomainID">Домен пользователя.</param>
        /// <param name="autocompleteKindID">Код типа пункта автозаполнения.</param>
        /// <returns>Список пунктов автозаполнения.</returns>
        IEnumerable<AutocompleteItem> GetAutocompleteItems(Guid? userDomainID, byte? autocompleteKindID);

        /// <summary>
        ///   Сохраняет информацию о пункте автозаполнения.
        /// </summary>
        /// <param name="autocompleteItem"> Сохраняемый пункт автозаполнения. </param>
        void SaveAutocompleteItem(AutocompleteItem autocompleteItem);

        /// <summary>
        /// Получает пункт автозаполнения по его ID.
        /// </summary>
        /// <param name="id">Код пункта автозаполнения.</param>
        /// <param name="userDomainID">Код домена пользователя. </param>
        /// <returns>Пункт автозаполнения, если существует.</returns>
        AutocompleteItem GetAutocompleteItem(Guid? id, Guid? userDomainID);

        /// <summary>
        /// Удаляет из хранилища пункт автозаполнения по его ID.
        /// </summary>
        /// <param name="id">Код пункта автозаполнения.</param>
        void DeleteAutocompleteItem(Guid? id);

        /// <summary>
        ///   Сохраняет информацию о пункте вознаграждения.
        /// </summary>
        /// <param name="userInterest"> Сохраняемый пункт вознаграждения. </param>
        void SaveUserInterest(UserInterest userInterest);

        /// <summary>
        /// Получает пункт вознаграждения по его ID.
        /// </summary>
        /// <param name="id">Код пункта вознаграждения.</param>
        /// <param name="userDomainID">Код домена пользователя. </param>
        /// <returns>Пункт вознаграждения, если существует.</returns>
        UserInterestDTO GetUserInterest(Guid? id, Guid? userDomainID);

        /// <summary>
        /// Удаляет из хранилища пункт вознаграждения по его ID.
        /// </summary>
        /// <param name="id">Код пункта вознаграждения.</param>
        void DeleteUserInterest(Guid? id);

        /// <summary>
        /// Удаляет из хранилища пункт вознаграждения по его ID.
        /// </summary>
        /// <param name="id">Код пункта вознаграждения.</param>
        /// <param name="userDomainID">Код домена пользователя. </param>
        void DeleteUserInterest(Guid? id,Guid? userDomainID);

        /// <summary>
        /// Получает список пунктов вознаграждения с фильтром.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователей. </param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список пунктов вознаграждения.</returns>
        IEnumerable<UserInterestDTO> GetUserInterests(Guid? userDomainID, string name, int page, int pageSize, out int count);

        /// <summary>
        /// Получает отчет по вознаграждениям пользователей за определенный период.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="beginDate">Дата начала.</param>
        /// <param name="endDate">Дата окончания.</param>
        /// <returns>Пункты отчета.</returns>
        IEnumerable<InterestReportItem> GetUserInterestReportItems(Guid? userDomainID, DateTime beginDate, DateTime endDate);

        /// <summary>
        /// Получает домен по номеру домена.
        /// </summary>
        /// <param name="number">Номер домена.</param>
        /// <returns>Домен пользователя, если существует.</returns>
        UserDomain GetUserDomain(int number);

        /// <summary>
        /// Получает заказа по его его номеру и номеру домена.
        /// </summary>
        /// <param name="number">Номер заказа.</param>
        /// <param name="userDomainNumber">Номер домена пользователя</param>
        /// <returns>Заказ, если существует.</returns>
        RepairOrderDTO GetRepairOrder(string number, int userDomainNumber);

        /// <summary>
        /// Получает пользователя по его ID.
        /// </summary>
        /// <param name="id">Код пользователя.</param>
        /// <returns>Пользователь, если существует.</returns>
        User GetUser(Guid? id);

        /// <summary>
        /// Возвращает список заказов для определенного домена.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Список заказов.</returns>
        IQueryable<RepairOrderDTO> GetRepairOrders(Guid? userDomainID);

        /// <summary>
        /// Получает информацию по состоянию грида.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="gridName"></param>
        /// <returns></returns>
        string GetGridUserState(Guid? userID, string gridName);

        /// <summary>
        ///   Сохраняет информацию состояние гриде.
        /// </summary>
        /// <param name="userID">Код пользователя.</param>
        /// <param name="gridName">Имя грида.</param>
        /// <param name="state">Состояние грида.</param>
        void SaveUserGridState(Guid? userID,string gridName, string state);

        /// <summary>
        ///   Сохраняет информацию фильтр гриде.
        /// </summary>
        /// <param name="userGridFilter"> Сохраняемый фильтр грида. </param>
        void SaveUserGridFilter(UserGridFilter userGridFilter);

        /// <summary>
        /// Получает пользовательский фильтр грида по его ID.
        /// </summary>
        /// <param name="userID">Код пользователя.</param>
        /// <param name="id">Код описания фильтр грида.</param>
        /// <returns>Фильтр грида, если существует.</returns>
        UserGridFilter GetUserGridFilter(Guid? userID, Guid? id);

        /// <summary>
        /// Получает список пользовательских фильтров для определенного грида.
        /// </summary>
        /// <param name="userID">Код пользователя.</param>
        /// <param name="gridName">Название грида.</param>
        /// <returns>Фильтры грида.</returns>
        IEnumerable<UserGridFilter> GetUserGridFilters(Guid? userID, string gridName);

        /// <summary>
        /// Получает список финансовых статей.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Список финансовых статей.</returns>
        IQueryable<FinancialItem> GetFinancialItems(Guid? userDomainID);

        /// <summary>
        /// Получает список значений статей расходов.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Список значений финансовых статей.</returns>
        IQueryable<FinancialItemValue> GetFinancialItemValues(Guid? userDomainID);

        /// <summary>
        /// Получает список пунктов вознаграждения.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователей. </param>
        /// <returns>Список пунктов вознаграждения.</returns>
        IQueryable<UserInterest> GetUserInterests(Guid? userDomainID);

        /// <summary>
        /// Получает список запросов на регистрацию публичных ключей с фильтром.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Список запросов на публичные ключи.</returns>
        IQueryable<UserPublicKeyRequestDTO> GetUserPublicKeyRequests(Guid? userDomainID);

        /// <summary>
        /// Получает список публичных ключей.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Список публичных ключей.</returns>
        IQueryable<UserPublicKeyDTO> GetUserPublicKeys(Guid? userDomainID);

        /// <summary>
        /// Получает список приходных накладных.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Список приходных накладных товаров.</returns>
        IQueryable<IncomingDocDTO> GetIncomingDocs(Guid? userDomainID);

        /// <summary>
        /// Получает список списаний со склада.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Список списаний со склада товаров.</returns>
        IQueryable<CancellationDocDTO> GetCancellationDocs(Guid? userDomainID);

        /// <summary>
        /// Получает список элемент документа о списании.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="cancellationDocID">Код документа о списании.</param>
        /// <returns>Список элементов документа о списании товаров.</returns>
        IQueryable<CancellationDocItemDTO> GetCancellationDocItems(Guid? userDomainID, Guid? cancellationDocID);

        /// <summary>
        /// Получает список перемещений со склада на склад.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Список перемещений со склада товаров.</returns>
        IQueryable<TransferDocDTO> GetTransferDocs(Guid? userDomainID);

        /// <summary>
        /// Получает список элементов документа о перемещении.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="transferDocID">Код документа о перемещении.</param>
        /// <returns>Список элементов документа о перемещении товаров.</returns>
        IQueryable<TransferDocItemDTO> GetTransferDocItems(Guid? userDomainID, Guid? transferDocID);

        /// <summary>
        ///   Сохраняет информацию о настройке пользователя.
        /// </summary>
        /// <param name="login">Логин пользователя.</param>
        /// <param name="number">Номер настройки.</param>
        /// <param name="data">Значение настройки.</param>
        void SaveUserSettingsItem(string login, string number, string data);

        /// <summary>
        /// Получает значение настройки пользователя.
        /// </summary>
        /// <param name="login">Логин пользователя.</param>
        /// <param name="number">Номер настройки.</param>
        /// <returns></returns>
        string GetUserSettingsItem(string login, string number);


        /// <summary>
        /// Подчищает все ссылки на пользователя.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="userID">Код пользователя.</param>
        void CleanUpUser(Guid? userDomainID,Guid? userID);
    }
}
