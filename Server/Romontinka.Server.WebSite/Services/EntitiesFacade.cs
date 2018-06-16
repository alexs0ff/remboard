using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.DataLayer.Entities.ReportItems;
using log4net;

namespace Romontinka.Server.WebSite.Services
{
    /// <summary>
    /// Реализация сервиса доступа к данным пользователей.
    /// </summary>
    public class EntitiesFacade : IEntitiesFacade
    {
        /// <summary>
        ///   Текущий логер.
        /// </summary>
        private static readonly ILog _logger = LogManager.GetLogger(typeof(EntitiesFacade));

         /// <summary>
        /// Возвращает список заказов в работе определенных пользователей с фильтром.
        /// </summary>
        /// <param name="token">Контекст безопасности. </param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список заказов.</returns>
        public IEnumerable<RepairOrderDTO> GetWorkRepairOrders(SecurityToken token, string name, int page, int pageSize, out int count)
         {
             _logger.InfoFormat("Получение для пользователя {0} рабочих заказов", token.LoginName);
             if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
             {
                 return RemontinkaServer.Instance.DataStore.GetWorkRepairOrders(token.User.UserDomainID,null, null, name, page, pageSize,
                                                                                out count);
             } //if

             if (token.User.ProjectRoleID == ProjectRoleSet.Engineer.ProjectRoleID)
             {
                 return RemontinkaServer.Instance.DataStore.GetWorkRepairOrders(token.User.UserDomainID,token.User.UserID, null, name, page, pageSize,
                                                                                out count);
             } //if

             if (token.User.ProjectRoleID == ProjectRoleSet.Manager.ProjectRoleID)
             {
                 return RemontinkaServer.Instance.DataStore.GetWorkRepairOrders(token.User.UserDomainID,null, token.User.UserID, name, page, pageSize,
                                                                                out count);
             } //if

             count = 0;
             return new RepairOrderDTO[0];
         }

        /// <summary>
        /// Сохранение заказа определенным пользователем.
        /// </summary>
        /// <param name="token">Токен пользователя.</param>
        /// <param name="order">Заказ.</param>
        public void SaveRepairOrder(SecurityToken token, RepairOrder order)
        {
            _logger.InfoFormat("Старт сохранения заказа {0} пользователем {1}", order.Number, token.LoginName);
            order.UserDomainID = token.User.UserDomainID;

            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                RemontinkaServer.Instance.DataStore.SaveRepairOrder(order);
            } //if
            else if (token.User.ProjectRoleID == ProjectRoleSet.Engineer.ProjectRoleID || token.User.ProjectRoleID == ProjectRoleSet.Manager.ProjectRoleID)
            {
                if (RemontinkaServer.Instance.DataStore.UserHasBranch(token.User.UserID,order.BranchID))
                {
                    RemontinkaServer.Instance.DataStore.SaveRepairOrder(order);    
                } //if
            } //else
        }

        /// <summary>
        /// Получает заказ руководствуясь привелегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен пользователя.</param>
        /// <param name="orderID">Код заказа.</param>
        /// <returns>Заказ.</returns>
        public RepairOrderDTO GetOrder(SecurityToken token,Guid? orderID)
        {
            _logger.InfoFormat("Старт получения заказа {0} пользователем {1}",orderID,token.LoginName);
            var result = RemontinkaServer.Instance.DataStore.GetRepairOrder(orderID, token.User.UserDomainID);
            
            if (result==null)
            {
                return null;
            } //if

            _logger.InfoFormat("Старт получения заказа {0} пользователем {1}", result.Number, token.LoginName);

            byte? projectRoleId = token.User.ProjectRoleID;
            RepairOrder repairOrder = result;

            var userID = token.User.UserID;

            if (UserHasAccessToRepairOrder(userID, repairOrder, projectRoleId))
            {
                return result;
            }

            return null;
        }

        /// <summary>
        /// Определяет есть ли у пользователя доступ к определенному заказу.
        /// </summary>
        /// <param name="userID">Код пользователя.</param>
        /// <param name="repairOrder">Заказ.</param>
        /// <param name="projectRoleId">Код роли.</param>
        /// <returns>Признак наличия доступа.</returns>
        public bool UserHasAccessToRepairOrder(Guid? userID, RepairOrder repairOrder, byte? projectRoleId)
        {
            if (projectRoleId == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return true;
            } //if

            if (projectRoleId == ProjectRoleSet.Manager.ProjectRoleID)
            {
                if (RemontinkaServer.Instance.DataStore.UserHasBranch(userID, repairOrder.BranchID))
                {
                    return true;
                 }
                
                _logger.ErrorFormat("Пользователь {0} не имеет доступа к филиалу {1}", userID, repairOrder.BranchID);

                return false;
            } //if

            if (projectRoleId == ProjectRoleSet.Engineer.ProjectRoleID)
            {
                if (RemontinkaServer.Instance.DataStore.UserHasBranch(userID, repairOrder.BranchID) &&
                    (userID == repairOrder.EngineerID || repairOrder.EngineerID == null))
                {
                    
                        return true;
                    
                } //if

                _logger.ErrorFormat("На инженера {0} не назначен Заказ {1}", userID, repairOrder.Number);
            } //if
            return false;
        }

        /// <summary>
        /// Получает новый id для заказа.
        /// </summary>
        /// <param name="token">Токен безопасности </param>
        /// <returns>Новый id.</returns>
        public long GetNewOrderNumber(SecurityToken token)
        {
            _logger.InfoFormat("Получение нового номера для заказа для пользователя {0}", token.LoginName);

            return RemontinkaServer.Instance.DataStore.GetNewOrderNumber(token.User.UserDomainID);
        }

        /// <summary>
        /// Удаляет из хранилища заказ руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="orderID">Код заказа.</param>
        public void DeleteOrder(SecurityToken token, Guid? orderID)
        {
            _logger.InfoFormat("Удаление заказа {0} пользователем {1}", token.LoginName, orderID);

            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                var order = RemontinkaServer.Instance.DataStore.GetRepairOrder(orderID, token.User.UserDomainID);

                if (order != null)
                {
                    RemontinkaServer.Instance.DataStore.DeleteOrderTimelineByRepairOrder(orderID);
                    RemontinkaServer.Instance.DataStore.DeleteWorkItemByRepairOrder(orderID);
                    RemontinkaServer.Instance.DataStore.DeleteDeviceItemByRepairOrder(orderID);
                    RemontinkaServer.Instance.DataStore.DeleteRepairOrder(orderID);
                } //if
            }
            else
            {
                ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            } //else
        }

        /// <summary>
        /// Проверяет имеет ли пользователь доступ к изменению конкретного заказа.
        /// </summary>
        /// <param name="user">Пользователь для проверки.</param>
        /// <param name="repairOrderID">Код заказа.</param>
        /// <returns>Признак возможности доступа.</returns>
        private bool UserHasAccessToOrder(User user, Guid? repairOrderID)
        {
            var domainId = RemontinkaServer.Instance.DataStore.GetRepairOrderUserDomainID(repairOrderID);

            if (domainId==null)
            {
                return false;
            }

            if (domainId!=user.UserDomainID)
            {
                return false;
            }

            if (user.ProjectRoleID==ProjectRoleSet.Admin.ProjectRoleID)
            {
                return true;
            } //if
            
            if (user.ProjectRoleID==ProjectRoleSet.Manager.ProjectRoleID)
            {
                var branchID = RemontinkaServer.Instance.DataStore.GetRepairOrderBranchID(repairOrderID);
                if (branchID==null)
                {
                    return false;
                } //if

                return RemontinkaServer.Instance.DataStore.UserHasBranch(user.UserID, branchID);
            } //if

            if (user.ProjectRoleID == ProjectRoleSet.Engineer.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.GetRepairOrderEngineerID(repairOrderID)==user.UserID;
            } //if

            return false;
        }

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
        public IEnumerable<RepairOrderDTO> GetRepairOrders(SecurityToken token, Guid? orderStatusId, bool? isUrgent, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение заказов по строке поиска {0} для пользователя {1}", name,token.LoginName);
            return RemontinkaServer.Instance.DataStore.GetRepairOrders(token.User.UserDomainID, orderStatusId, null,
                                                                       name, page, pageSize,
                                                                       out count);
        }

        /// <summary>
        /// Возвращает список заказов без фильтра.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список заказов.</returns>
        public IQueryable<RepairOrderDTO> GetRepairOrders(SecurityToken token)
        {
            _logger.InfoFormat("Получение заказов без строки поиска  для пользователя {0}", token.LoginName);
            return RemontinkaServer.Instance.DataStore.GetRepairOrders(token.User.UserDomainID);
        }

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
        public IEnumerable<RepairOrderDTO> GetRepairOrdersUserBranch(SecurityToken token, Guid? orderStatusId, bool? isUrgent, Guid? userId, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение заказов по строке поиска {0} пользователем {1}", name, token.LoginName);
            
            var items = RemontinkaServer.Instance.DataStore.GetRepairOrdersUserBranch(token.User.UserDomainID, orderStatusId,
                                                                                 null,
                                                                                 token.User.UserID, name,
                                                                                 page,
                                                                                 pageSize,
                                                                                 out count);

            if (token.User.ProjectRoleID == ProjectRoleSet.Engineer.ProjectRoleID)
            {
                items = items.Where(i => i.EngineerID == token.User.UserID);
            } //if

            return items;
        }

        /// <summary>
        /// Получает списко заказов за определенный период.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="beginDate">Дата начала.</param>
        /// <param name="endDate">Дата окончания.</param>
        /// <returns>Заказы.</returns>
        public IEnumerable<RepairOrder> GetRepairOrders(SecurityToken token, DateTime beginDate, DateTime endDate)
        {
            _logger.InfoFormat("Получение заказов пользователем {0} с {1} по {2}", token.LoginName, beginDate, endDate);

            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {

                return RemontinkaServer.Instance.DataStore.GetRepairOrders(token.User.UserDomainID, beginDate, endDate);
            } //if

            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }

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
        public IEnumerable<RepairOrderDTO> GetRepairOrdersUser(SecurityToken token, Guid? orderStatusId, bool? isUrgent, Guid? userId, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение заказов по строке поиска {0}", name);
            return RemontinkaServer.Instance.DataStore.GetRepairOrdersUser(token.User.UserDomainID, orderStatusId, null,
                                                                           token.User.UserID, name, page, pageSize,
                                                                           out count);
        }

        #region OrderStatus

        /// <summary>
        /// Получает список статусов заказа с фильтром.
        /// </summary>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список статусов заказа.</returns>
        public IEnumerable<OrderStatus> GetOrderStatuses(SecurityToken token, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение списка статусов заказов для пользователя {0} {1}", token.LoginName, name);

            return RemontinkaServer.Instance.DataStore.GetOrderStatuses(token.User.UserDomainID, name, page, pageSize, out count);
        }

        /// <summary>
        /// Возвращает с хранилища статус заказа.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="orderStatusID">Код статуса заказа.</param>
        /// <returns>Статус заказа</returns>
        public OrderStatus GetOrderStatus(SecurityToken token, Guid? orderStatusID)
        {
            _logger.InfoFormat("Получение статуса заказа для пользователя {0} {1}", token.LoginName, orderStatusID);

            return RemontinkaServer.Instance.DataStore.GetOrderStatus(orderStatusID, token.User.UserDomainID);
        }

        /// <summary>
        /// Сохраняет в хранилище статус заказа.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="orderStatus">Статус заказа.</param>
        public void SaveOrderStatus(SecurityToken token, OrderStatus orderStatus)
        {
            _logger.InfoFormat("Сохранение статуса заказа пользователем {0} {1}", token.LoginName, orderStatus.OrderStatusID);
            orderStatus.UserDomainID = token.User.UserDomainID;
            RemontinkaServer.Instance.DataStore.SaveOrderStatus(orderStatus);
        }

        /// <summary>
        /// Удаляет из хранилища статус заказа руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="orderStatusID">Код статсуса заказа.</param>
        public void DeleteOrderStatus(SecurityToken token, Guid? orderStatusID)
        {
            _logger.InfoFormat("Удаление статуса заказа {0} пользователем {1}", token.LoginName, orderStatusID);

            var orderStatus = RemontinkaServer.Instance.DataStore.GetOrderStatus(orderStatusID, token.User.UserDomainID);
            if (orderStatus != null)
            {
                RemontinkaServer.Instance.DataStore.DeleteOrderStatus(orderStatusID);
            } //if
        }

        /// <summary>
        /// Получает список всех статусов заказа для пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список статусов заказа.</returns>
        public IQueryable<OrderStatus> GetOrderStatuses(SecurityToken token)
        {
            _logger.InfoFormat("Получение всех статусов заказа для пользователя {0}",token.LoginName);
            return RemontinkaServer.Instance.DataStore.GetOrderStatuses(token.User.UserDomainID);
        }

         /// <summary>
        /// Получение статусов заказа по его типам.
        /// </summary>
        /// <param name="token">Токен безопасности. </param>
        /// <param name="kindId">Тип статуса.</param>
        /// <returns>Если не находит пытается найти ближайший по смыслу.</returns>
        public OrderStatus GetOrderStatusByKind(SecurityToken token, byte? kindId)
        {
            _logger.InfoFormat("Получение статусов заказов по его id {0} для пользователя {1}", kindId, token.LoginName);
             return RemontinkaServer.Instance.DataStore.GetOrderStatusByKind(token.User.UserDomainID, kindId);
        }

        #endregion OrderStatus

        #region OrderKind

        /// <summary>
        /// Получает список типов заказа с фильтром.
        /// </summary>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список типов заказа.</returns>
        public IEnumerable<OrderKind> GetOrderKinds(SecurityToken token, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение списка типов заказов для пользователя {0} {1}", token.LoginName, name);

            return RemontinkaServer.Instance.DataStore.GetOrderKinds(token.User.UserDomainID, name, page, pageSize, out count);
        }

        /// <summary>
        /// Получает список всех типов заказа для пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список статусов заказа.</returns>
        public IQueryable<OrderKind> GetOrderKinds(SecurityToken token)
        {
            _logger.InfoFormat("Получение всех типов заказа для пользователя {0}", token.LoginName);
            return RemontinkaServer.Instance.DataStore.GetOrderKinds(token.User.UserDomainID);
        }

        /// <summary>
        /// Возвращает с хранилища тип заказа.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="orderKindID">Код статуса заказа.</param>
        /// <returns>Тип заказа</returns>
        public OrderKind GetOrderKind(SecurityToken token, Guid? orderKindID)
        {
            _logger.InfoFormat("Получение типа заказа для пользователя {0} {1}", token.LoginName, orderKindID);

            return RemontinkaServer.Instance.DataStore.GetOrderKind(orderKindID, token.User.UserDomainID);
        }

        /// <summary>
        /// Сохраняет в хранилище тип заказа.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="orderKind">Тип заказа.</param>
        public void SaveOrderKind(SecurityToken token, OrderKind orderKind)
        {
            _logger.InfoFormat("Сохранение тип заказа пользователем {0} {1}", token.LoginName, orderKind.OrderKindID);
            orderKind.UserDomainID = token.User.UserDomainID;
            RemontinkaServer.Instance.DataStore.SaveOrderKind(orderKind);
        }

        /// <summary>
        /// Удаляет из хранилища тип заказа руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="orderKindID">Код типа заказа.</param>
        public void DeleteOrderKind(SecurityToken token, Guid? orderKindID)
        {
            _logger.InfoFormat("Удаление типа заказа {0} пользователем {1}", token.LoginName, orderKindID);

            var orderKind = RemontinkaServer.Instance.DataStore.GetOrderKind(orderKindID, token.User.UserDomainID);
            if (orderKind != null)
            {
                RemontinkaServer.Instance.DataStore.DeleteOrderKind(orderKindID);
            } //if
        }

        #endregion OrderKind

        #region Work item

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
        public IEnumerable<WorkItemDTO> GetWorkItems(SecurityToken token, Guid? repairOrderID, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение списка пункта выполненных работ для пользователя {0} {1}", token.LoginName, repairOrderID);

            if (!UserHasAccessToOrder(token.User, repairOrderID))
            {
                ThrowSecurityException(token, string.Format("Пользователь {0} не имеет доступа к списку пунктов выполненных работ {1}", token.LoginName, repairOrderID));
            } //if

            return RemontinkaServer.Instance.DataStore.GetWorkItems(repairOrderID, name, page, pageSize, out count);
        }

        /// <summary>
        /// Получает список пунктов выполненных работ.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="repairOrderID">Код заказа.</param>
        /// <returns>Список пунктов выполненных работ.</returns>
        public IQueryable<WorkItemDTO> GetWorkItems(SecurityToken token, Guid? repairOrderID)
        {
            _logger.InfoFormat("Получение списка пункта выполненных работ для пользователя {0} {1}", token.LoginName, repairOrderID);

            if (!UserHasAccessToOrder(token.User, repairOrderID))
            {
                ThrowSecurityException(token, string.Format("Пользователь {0} не имеет доступа к списку пунктов выполненных работ {1}", token.LoginName, repairOrderID));
            } //if

            return RemontinkaServer.Instance.DataStore.GetWorkItems(repairOrderID);
        }

        /// <summary>
        /// Возвращает с хранилища пункт выполненных работ.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="workItemID">Код пункта выполненных работ.</param>
        /// <returns>Пункт выполненных работ</returns>
        public WorkItemDTO GetWorkItem(SecurityToken token, Guid? workItemID)
        {
            _logger.InfoFormat("Получение пункта выполненных работ для пользователя {0} {1}",token.LoginName,workItemID);

            var workItem = RemontinkaServer.Instance.DataStore.GetWorkItem(workItemID);
            if (workItem==null)
            {
                return null;
            } //if

            if (UserHasAccessToOrder(token.User,workItem.RepairOrderID))
            {
                return workItem;
            } //if
            ThrowSecurityException(token, string.Format("Пользователь {0} не имеет доступа к пункту выполненных работ {1}", token.LoginName, workItemID));
            return null;
        }

        /// <summary>
        /// Возвращает сохраняет в хранилище пункт выполненных работ.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="workItem">Пункт выполненных работ.</param>
        public void SaveWorkItem(SecurityToken token, WorkItem workItem)
        {
            _logger.InfoFormat("Сохранение пункта выполненных работ пользователем {0} {1}", token.LoginName, workItem.WorkItemID);
            
            if (UserHasAccessToOrder(token.User, workItem.RepairOrderID))
            {
                RemontinkaServer.Instance.DataStore.SaveWorkItem(workItem);
            } //if
            else
            {
                ThrowSecurityException(token,
                                       string.Format(
                                           "Пользователь {0} не имеет доступа к пункту выполненных работ {1}",
                                           token.LoginName, workItem.RepairOrderID));
            } //else
        }
        
        /// <summary>
        /// Удаляет из хранилища пункт выполненных работ руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="workItemID">Код пункта выполненных работ.</param>
        public void DeleteWorkItem(SecurityToken token, Guid? workItemID)
        {
            _logger.InfoFormat("Удаление пункта выполненных работ {0} пользователем {1}",token.LoginName,workItemID);

            var workItem = RemontinkaServer.Instance.DataStore.GetWorkItem(workItemID);
            if (workItem!=null)
            {
                if (UserHasAccessToOrder(token.User,workItem.RepairOrderID))
                {
                    RemontinkaServer.Instance.DataStore.DeleteWorkItem(workItemID);
                } //if
                else
                {
                    ThrowSecurityException(token, string.Format("Пользователь {0} не имеет доступа к пункту выполненных работ {1}", token.LoginName, workItemID));
                } //else
            } //if
        }

        #endregion Work item

        #region DeviceItem 

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
        public IEnumerable<DeviceItem> GetDeviceItems(SecurityToken token, Guid? repairOrderID, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение списка запчастей для пользователя {0} {1}", token.LoginName, repairOrderID);

            if (!UserHasAccessToOrder(token.User, repairOrderID))
            {
                ThrowSecurityException(token, string.Format("Пользователь {0} не имеет доступа к запчастей {1}", token.LoginName, repairOrderID));
            } //if

            return RemontinkaServer.Instance.DataStore.GetDeviceItems(repairOrderID, name, page, pageSize, out count);
        }

        /// <summary>
        /// Получает список запчастей.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="repairOrderID">Код заказа.</param>
        /// <returns>Список запчастей.</returns>
        public IQueryable<DeviceItemDTO> GetDeviceItems(SecurityToken token, Guid? repairOrderID)
        {
            _logger.InfoFormat("Получение списка запчастей для пользователя {0} {1}", token.LoginName, repairOrderID);

            if (!UserHasAccessToOrder(token.User, repairOrderID))
            {
                ThrowSecurityException(token, string.Format("Пользователь {0} не имеет доступа к запчастей {1}", token.LoginName, repairOrderID));
            } //if

            return RemontinkaServer.Instance.DataStore.GetDeviceItems(repairOrderID);
        }

        /// <summary>
        /// Возвращает с хранилища запчасти.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="deviceItemID">Код запчасти.</param>
        /// <returns>Запчасть.</returns>
        public DeviceItem GetDeviceItem(SecurityToken token, Guid? deviceItemID)
        {
            _logger.InfoFormat("Получение запчасти для пользователя {0} {1}", token.LoginName, deviceItemID);

            var deviceItem = RemontinkaServer.Instance.DataStore.GetDeviceItem(deviceItemID);
            if (deviceItem == null)
            {
                return null;
            } //if

            if (UserHasAccessToOrder(token.User, deviceItem.RepairOrderID))
            {
                return deviceItem;
            } //if
            ThrowSecurityException(token, string.Format("Пользователь {0} не имеет доступа к запчасти {1}", token.LoginName, deviceItemID));
            return null;
        }

        /// <summary>
        /// Возвращает сохраняет в хранилище запчасть заказа.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="deviceItem">Запчасть заказа.</param>
        public void SaveDeviceItem(SecurityToken token, DeviceItem deviceItem)
        {
            _logger.InfoFormat("Сохранение запчасти заказа работ пользователем {0} {1}", token.LoginName, deviceItem.DeviceItemID);

            if (UserHasAccessToOrder(token.User, deviceItem.RepairOrderID))
            {
                RemontinkaServer.Instance.DataStore.SaveDeviceItem(deviceItem);
            } //if
            else
            {
                ThrowSecurityException(token,
                                       string.Format(
                                           "Пользователь {0} не имеет доступа к запчасти заказа {1}",
                                           token.LoginName, deviceItem.RepairOrderID));
            } //else
        }

        /// <summary>
        /// Удаляет из хранилища пункт выполненных работ руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="deviceItemID">Код пункта выполненных работ.</param>
        public void DeleteDeviceItem(SecurityToken token, Guid? deviceItemID)
        {
            _logger.InfoFormat("Удаление запчасти заказа {0} пользователем {1}", token.LoginName, deviceItemID);

            var deviceItem = RemontinkaServer.Instance.DataStore.GetDeviceItem(deviceItemID);
            if (deviceItem != null)
            {
                if (UserHasAccessToOrder(token.User, deviceItem.RepairOrderID))
                {
                    RemontinkaServer.Instance.DataStore.DeleteDeviceItem(deviceItemID);
                } //if
                else
                {
                    ThrowSecurityException(token, string.Format("Пользователь {0} не имеет доступа к запчасти заказа {1}", token.LoginName, deviceItemID));
                } //else
            } //if
        }

        #endregion DeviceItem

        #region Repair Order Reports 

        /// <summary>
        /// Создает отчет по заказу руководствуясь привелегиями текущего пользователя.
        /// </summary>
        /// <param name="token">Текущий токен пользователя.</param>
        /// <param name="customReportID">Код отчета.</param>
        /// <param name="repairOrderID">Код пользователя.</param>
        /// <returns>Созданный отчет.</returns>
        public string CreateRepairOrderReport(SecurityToken token, Guid? customReportID, Guid? repairOrderID)
        {
            _logger.InfoFormat("Начало создание отчета пользователем {0} {1}", token.LoginName, repairOrderID);

            if (!UserHasAccessToOrder(token.User,repairOrderID))
            {
                ThrowSecurityException(token, string.Format("У пользователя {0} нет доступа к заказу {1}", token.LoginName, repairOrderID));
            } //if

            return RemontinkaServer.Instance.HTMLReportService.CreateRepairOrderReport(token, customReportID,
                                                                                       repairOrderID);
        }

        #endregion Repair Order Reports

        #region CustomReportItem

        /// <summary>
        /// Получает список настраеваемых отчетов с фильтром по типу документа.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="documentKindID">Код типа документа. </param>
        /// <returns>Список настраеваемых отчетов.</returns>
        public IEnumerable<CustomReportItem> GetCustomReportItems(SecurityToken token, byte? documentKindID)
        {
            _logger.InfoFormat("Получение списка настраеваемых отчетов для пользователя {0} {1}", token.LoginName, documentKindID);

            return RemontinkaServer.Instance.DataStore.GetCustomReportItems(token.User.UserDomainID, documentKindID);
        }

        /// <summary>
        /// Получает список всех настраеваемых отчетов для пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список настраеваемых отчетов.</returns>
        public IQueryable<CustomReportItem> GetCustomReportItems(SecurityToken token)
        {
            _logger.InfoFormat("Получение всех настраеваемых отчетов для пользователя {0}", token.LoginName);
            return RemontinkaServer.Instance.DataStore.GetCustomReportItems(token.User.UserDomainID);
        }

        /// <summary>
        /// Возвращает с хранилища настраеваемых отчет.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="customReportItemID">Код настраеваемого отчета.</param>
        /// <returns>Настраеваемый отчет</returns>
        public CustomReportItem GetCustomReportItem(SecurityToken token, Guid? customReportItemID)
        {
            _logger.InfoFormat("Получение настраеваемого отчета для пользователя {0} {1}", token.LoginName, customReportItemID);

            return RemontinkaServer.Instance.DataStore.GetCustomReportItem(customReportItemID, token.User.UserDomainID);
        }

        /// <summary>
        /// Сохраняет в хранилище настраеваемый отчет.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="customReportItem">Настраеваемый отчет.</param>
        public void SaveCustomReportItem(SecurityToken token, CustomReportItem customReportItem)
        {
            _logger.InfoFormat("Сохранение тип заказа пользователем {0} {1}", token.LoginName, customReportItem.CustomReportID);
            customReportItem.UserDomainID = token.User.UserDomainID;
            RemontinkaServer.Instance.DataStore.SaveCustomReportItem(customReportItem);
        }

        /// <summary>
        /// Удаляет из хранилища настраеваемый отчет руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="customReportItemID">Код настраеваемого отчета.</param>
        public void DeleteCustomReportItem(SecurityToken token, Guid? customReportItemID)
        {
            _logger.InfoFormat("Удаление настраеваемого отчета {0} пользователем {1}", token.LoginName, customReportItemID);

            var customReportItem = RemontinkaServer.Instance.DataStore.GetCustomReportItem(customReportItemID, token.User.UserDomainID);
            if (customReportItem != null)
            {
                RemontinkaServer.Instance.DataStore.DeleteCustomReportItem(customReportItemID);
            } //if
        }

        #endregion CustomReportItem

        #region OrderTimeline

        /// <summary>
        /// Получает пункты истории изменений по конкретному заказу.
        /// </summary>
        /// <param name="token">Текущий токен пользователя.</param>
        /// <param name="repairOrderID">Код заказа.</param>
        /// <returns>Список пунктов истории.</returns>
        public IQueryable<OrderTimeline> GetOrderTimelines(SecurityToken token,Guid? repairOrderID)
        {
            _logger.InfoFormat("Получение информации по истории заказа {0} пользователем {1}",repairOrderID,token.LoginName);
            if (!UserHasAccessToOrder(token.User,repairOrderID))
            {
                ThrowSecurityException(token,string.Format("Пользователь {0} не имеет прав доступа к заказу {1}",token.LoginName,repairOrderID));
            }

            return RemontinkaServer.Instance.DataStore.GetOrderTimelines(repairOrderID);
        }

        /// <summary>
        /// Добавляет комментарий в заказ.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="repairOrderID">Код заказа.</param>
        /// <param name="text">Текст комментария.</param>
        public void AddRepairOrderComment(SecurityToken token, Guid? repairOrderID, string text)
        {
            _logger.InfoFormat("Добавление комментария в заказ {0} пользователем {1}", repairOrderID, token.LoginName);
            if (!UserHasAccessToOrder(token.User, repairOrderID))
            {
                ThrowSecurityException(token, string.Format("Пользователь {0} не имеет прав доступа к заказу {1}", token.LoginName, repairOrderID));
            }

            RemontinkaServer.Instance.OrderTimelineManager.AddNewComment(token, repairOrderID, text);
        }

        #endregion

        #region Branch

        /// <summary>
        /// Получает список филиалов с фильтром.
        /// </summary>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список филиалов.</returns>
        public IEnumerable<Branch> GetBranches(SecurityToken token, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение списка филиалов для пользователя {0} {1}", token.LoginName, name);

            return RemontinkaServer.Instance.DataStore.GetBranches(token.User.UserDomainID, name, page, pageSize, out count);
        }

        /// <summary>
        /// Получает список всех филиалов для пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список филиалов.</returns>
        public IQueryable<Branch> GetBranches(SecurityToken token)
        {
            _logger.InfoFormat("Получение всех филиалов для пользователя {0}", token.LoginName);
            return RemontinkaServer.Instance.DataStore.GetBranches(token.User.UserDomainID);
        }

        /// <summary>
        /// Возвращает с хранилища филиал.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="branchID">Код филиала.</param>
        /// <returns>Филиал</returns>
        public Branch GetBranch(SecurityToken token, Guid? branchID)
        {
            _logger.InfoFormat("Получение филиала для пользователя {0} {1}", token.LoginName, branchID);

            return RemontinkaServer.Instance.DataStore.GetBranch(branchID, token.User.UserDomainID);
        }

        /// <summary>
        /// Сохраняет в хранилище филиал.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="branch">Филиала.</param>
        public void SaveBranch(SecurityToken token, Branch branch)
        {
            _logger.InfoFormat("Сохранение филиала пользователем {0} {1}", token.LoginName, branch.BranchID);
            branch.UserDomainID = token.User.UserDomainID;
            RemontinkaServer.Instance.DataStore.SaveBranch(branch);
        }

        /// <summary>
        /// Удаляет из хранилища филиал руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="branchID">Код филиала.</param>
        public void DeleteBranch(SecurityToken token, Guid? branchID)
        {
            _logger.InfoFormat("Удаление филиала {0} пользователем {1}", token.LoginName, branchID);

            var branch = RemontinkaServer.Instance.DataStore.GetBranch(branchID, token.User.UserDomainID);
            if (branch != null)
            {
                RemontinkaServer.Instance.DataStore.DeleteBranch(branchID);
            } //if
        }

        #endregion Branch

        #region User 

        /// <summary>
        /// Получает список пользователей с фильтром.
        /// </summary>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список пользовательов.</returns>
        public IEnumerable<User> GetUsers(SecurityToken token, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение списка пользователей для пользователя {0} {1}", token.LoginName, name);

            return RemontinkaServer.Instance.DataStore.GetUsers(token.User.UserDomainID, name, page, pageSize, out count);
        }

        /// <summary>
        /// Получает список всех пользователей для пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список пользователей.</returns>
        public IQueryable<User> GetUsers(SecurityToken token)
        {
            _logger.InfoFormat("Получение всех пользователей для пользователя {0}", token.LoginName);
            return RemontinkaServer.Instance.DataStore.GetUsers(token.User.UserDomainID);
        }

        /// <summary>
        /// Получает список всех пользователей с определенной ролью.
        /// </summary>
        /// <param name="projectRoleId">Код роли в проекте. </param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список пользователей.</returns>
        public IEnumerable<User> GetUsers(SecurityToken token, byte? projectRoleId)
        {
            _logger.InfoFormat("Получение всех пользователей с ролью {0} для пользователя {1}", projectRoleId,
                               token.LoginName);

            return RemontinkaServer.Instance.DataStore.GetUsers(projectRoleId, token.User.UserDomainID);
        }

        /// <summary>
        /// Возвращает с хранилища пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="userID">Код пользователя.</param>
        /// <returns>Пользователь</returns>
        public User GetUser(SecurityToken token, Guid? userID)
        {
            _logger.InfoFormat("Получение пользователя для пользователя {0} {1}", token.LoginName, userID);

            return RemontinkaServer.Instance.DataStore.GetUser(userID, token.User.UserDomainID);
        }

        /// <summary>
        /// Сохраняет в хранилище пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="user">Пользователь.</param>
        public void SaveUser(SecurityToken token, User user)
        {
            _logger.InfoFormat("Сохранение пользователя пользователем {0} {1}", token.LoginName, user.UserID);
            user.UserDomainID = token.User.UserDomainID;
            RemontinkaServer.Instance.DataStore.SaveUser(user);
        }

        /// <summary>
        /// Удаляет из хранилища пользователя руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="userID">Код пользователя.</param>
        public void DeleteUser(SecurityToken token, Guid? userID)
        {
            _logger.InfoFormat("Удаление пользователя {0} пользователем {1}", token.LoginName, userID);

            var user = RemontinkaServer.Instance.DataStore.GetUser(userID, token.User.UserDomainID);
            if (user != null)
            {
                RemontinkaServer.Instance.DataStore.CleanUpUser(token.User.UserDomainID, userID);
                RemontinkaServer.Instance.DataStore.DeleteUser(userID);
            } //if
        }

        #endregion User

        #region UserDomain 

        /// <summary>
        /// Получает текущий домен пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Домен.</returns>
        public UserDomain GetUserDomain(SecurityToken token)
        {
            _logger.InfoFormat("Получение домена пользователя {0}",token.LoginName);

            return RemontinkaServer.Instance.DataStore.GetUserDomain(token.User.UserDomainID);
        }

        /// <summary>
        /// Обновляет информацию по домену пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="updatedEntity">Обновляемый домен.</param>
        public void UpdateUserDomain(SecurityToken token, UserDomain updatedEntity)
        {
            _logger.InfoFormat("Обновление доменой информации для пользователя {0}:{1}",token.LoginName,token.User.UserID);

            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                updatedEntity.UserDomainID = token.User.UserDomainID;

                var currentDomain = RemontinkaServer.Instance.DataStore.GetUserDomain(token.User.UserDomainID);

                currentDomain.LegalName = updatedEntity.LegalName;
                currentDomain.RegistredEmail = updatedEntity.RegistredEmail;
                currentDomain.Address = updatedEntity.Address;
                currentDomain.Trademark = updatedEntity.Trademark;

                RemontinkaServer.Instance.DataStore.SaveUserDomain(currentDomain);
                return;
            } //if
            
            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
        }

        #endregion UserDomain

        #region FinancialGroupItem

        /// <summary>
        /// Получает список финансовая группаов с фильтром.
        /// </summary>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список финансовая группаов.</returns>
        public IEnumerable<FinancialGroupItem> GetFinancialGroupItems(SecurityToken token, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение списка финансовая групп для пользователя {0} {1}", token.LoginName, name);
            if (token.User.ProjectRoleID ==ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.GetFinancialGroupItems(token.User.UserDomainID, name, page, pageSize, out count);    
            } //if

            count = 0;
            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }

        /// <summary>
        /// Получает список всех финансовых групп для пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список финансовых групп.</returns>
        public IQueryable<FinancialGroupItem> GetFinancialGroupItems(SecurityToken token)
        {
            _logger.InfoFormat("Получение всех финансовых групп для пользователя {0}", token.LoginName);
            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.GetFinancialGroupItems(token.User.UserDomainID);
            }
            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }

        /// <summary>
        /// Возвращает с хранилища финансовую группу.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="financialGroupID">Код финансовай группы.</param>
        /// <returns>Филиал</returns>
        public FinancialGroupItem GetFinancialGroupItem(SecurityToken token, Guid? financialGroupID)
        {
            _logger.InfoFormat("Получение финансовой группы для пользователя {0} {1}", token.LoginName, financialGroupID);

            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.GetFinancialGroupItem(financialGroupID,
                                                                                 token.User.UserDomainID);
            }
            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }

        /// <summary>
        /// Сохраняет в хранилище финансовая группа.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="financialGroupItem">Финансовая группа.</param>
        public void SaveFinancialGroupItem(SecurityToken token, FinancialGroupItem financialGroupItem)
        {
            _logger.InfoFormat("Сохранение финансовая группаа пользователем {0} {1}", token.LoginName, financialGroupItem.FinancialGroupID);
            financialGroupItem.UserDomainID = token.User.UserDomainID;
            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                RemontinkaServer.Instance.DataStore.SaveFinancialGroupItem(financialGroupItem);
                return;
            }

            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
        }

        /// <summary>
        /// Удаляет из хранилища финансовая группа руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="financialGroupID">Код финансовая группаа.</param>
        public void DeleteFinancialGroupItem(SecurityToken token, Guid? financialGroupID)
        {
            _logger.InfoFormat("Удаление финансовая группа {0} пользователем {1}", token.LoginName, financialGroupID);

            if (token.User.ProjectRoleID != ProjectRoleSet.Admin.ProjectRoleID)
            {
                ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);    
            } //if

            var financialGroupItem = RemontinkaServer.Instance.DataStore.GetFinancialGroupItem(financialGroupID, token.User.UserDomainID);
            if (financialGroupItem != null)
            {
                RemontinkaServer.Instance.DataStore.DeleteFinancialGroupItem(financialGroupID);
            } //if
        }

        #endregion FinancialGroupItem

        #region FinancialItem

        /// <summary>
        /// Получает список финансовых статей с фильтром.
        /// </summary>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список финансовых статей.</returns>
        public IEnumerable<FinancialItem> GetFinancialItems(SecurityToken token, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение списка финансовых статей для пользователя {0} {1}", token.LoginName, name);
            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.GetFinancialItems(token.User.UserDomainID, name, page, pageSize, out count);
            } //if

            count = 0;
            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }

        /// <summary>
        /// Получает список финансовых статей.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список финансовых статей.</returns>
        public IQueryable<FinancialItem> GetFinancialItems(SecurityToken token)
        {
            _logger.InfoFormat("Получение списка финансовых статей для пользователя {0}", token.LoginName);
            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.GetFinancialItems(token.User.UserDomainID);
            } //if
            
            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }

        /// <summary>
        /// Возвращает с хранилища финансовую статью.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="financialID">Код финансовой статьи.</param>
        /// <returns>Филиал</returns>
        public FinancialItem GetFinancialItem(SecurityToken token, Guid? financialID)
        {
            _logger.InfoFormat("Получение финансовой статьи для пользователя {0} {1}", token.LoginName, financialID);

            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.GetFinancialItem(financialID,
                                                                                 token.User.UserDomainID);
            }
            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }

        /// <summary>
        /// Сохраняет в хранилище финансовую статью.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="financialItem">Финансовая статья.</param>
        public void SaveFinancialItem(SecurityToken token, FinancialItem financialItem)
        {
            _logger.InfoFormat("Сохранение финансовая статья пользователем {0} {1}", token.LoginName, financialItem.FinancialItemID);
            financialItem.UserDomainID = token.User.UserDomainID;
            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                RemontinkaServer.Instance.DataStore.SaveFinancialItem(financialItem);
                return;
            }

            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
        }

        /// <summary>
        /// Удаляет из хранилища финансовлй статьи руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="financialID">Код финансовой статьи.</param>
        public void DeleteFinancialItem(SecurityToken token, Guid? financialID)
        {
            _logger.InfoFormat("Удаление финансовая статья {0} пользователем {1}", token.LoginName, financialID);

            if (token.User.ProjectRoleID != ProjectRoleSet.Admin.ProjectRoleID)
            {
                ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            } //if

            var financialItem = RemontinkaServer.Instance.DataStore.GetFinancialItem(financialID, token.User.UserDomainID);
            if (financialItem != null)
            {
                RemontinkaServer.Instance.DataStore.DeleteFinancialItem(financialID);
            } //if
        }

        #endregion FinancialItem

        #region FinancialItemValue

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
        public IEnumerable<FinancialItemValueDTO> GetFinancialItemValues(SecurityToken token,Guid? financialGroupID, string name, DateTime beginDate, DateTime endDate, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение списка значей финансовых статей для пользователя {0} {1}", token.LoginName, name);
            if (token.User.ProjectRoleID != ProjectRoleSet.Admin.ProjectRoleID)
            {
                ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);    
            }

            return RemontinkaServer.Instance.DataStore.GetFinancialItemValues(financialGroupID, token.User.UserDomainID,
                                                                              name, beginDate, endDate, page, pageSize,
                                                                              out count);
        }

        /// <summary>
        /// Получает список значений статей расходов.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список значений финансовых статей.</returns>
        public IQueryable<FinancialItemValue> GetFinancialItemValues(SecurityToken token)
        {
            _logger.InfoFormat("Получение списка значей финансовых статей для пользователя {0}", token.LoginName);
            if (token.User.ProjectRoleID != ProjectRoleSet.Admin.ProjectRoleID)
            {
                ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            }

            return RemontinkaServer.Instance.DataStore.GetFinancialItemValues(token.User.UserDomainID);
        }

        /// <summary>
        /// Возвращает с хранилища значение финансовой статьи.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="financialItemValueID">Код значения финансовой статьи.</param>
        /// <returns>Значение финансовой статьи.</returns>
        public FinancialItemValueDTO GetFinancialItemValue(SecurityToken token, Guid? financialItemValueID)
        {
            _logger.InfoFormat("Получение значения финансовой статьи для пользователя {0} {1}", token.LoginName, financialItemValueID);

            if (token.User.ProjectRoleID != ProjectRoleSet.Admin.ProjectRoleID)
            {
                ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
                
            }
            return RemontinkaServer.Instance.DataStore.GetFinancialItemValue(financialItemValueID, token.User.UserDomainID);
        }

        /// <summary>
        /// Сохраняет в хранилище значение финансовой статьи.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="financialItemValue">Значение финансовой статьи.</param>
        public void SaveFinancialItemValue(SecurityToken token, FinancialItemValue financialItemValue)
        {
            _logger.InfoFormat("Сохранение значения финансовой статьи пользователем {0} {1}", token.LoginName, financialItemValue.FinancialItemID);
            if (token.User.ProjectRoleID != ProjectRoleSet.Admin.ProjectRoleID)
            {
                ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            }

            RemontinkaServer.Instance.DataStore.SaveFinancialItemValue(financialItemValue);
        }

        /// <summary>
        /// Удаляет из хранилища значение финансовой статьи руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="financialID">Код значения финансовой статьи.</param>
        public void DeleteFinancialItemValue(SecurityToken token, Guid? financialID)
        {
            _logger.InfoFormat("Удаление значения финансовой статьи {0} пользователем {1}", token.LoginName, financialID);

            if (token.User.ProjectRoleID != ProjectRoleSet.Admin.ProjectRoleID)
            {
                ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            } //if

            var financialItem = RemontinkaServer.Instance.DataStore.GetFinancialItemValue(financialID, token.User.UserDomainID);
            if (financialItem != null)
            {
                RemontinkaServer.Instance.DataStore.DeleteFinancialItemValue(financialID);
            } //if
        }

        #endregion FinancialItemValue

        #region ItemCategory

        /// <summary>
        /// Получает список категорий товара с фильтром.
        /// </summary>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список категорий товара.</returns>
        public IEnumerable<ItemCategory> GetItemCategories(SecurityToken token, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение списка категорий товара для пользователя {0} {1}", token.LoginName, name);
            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.GetItemCategories(token.User.UserDomainID, name, page, pageSize, out count);
            } //if

            count = 0;
            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }

        /// <summary>
        /// Получает список всех категорий товара для пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список категорий товара.</returns>
        public IQueryable<ItemCategory> GetItemCategories(SecurityToken token)
        {
            _logger.InfoFormat("Получение списка категорий товара для пользователя {0}", token.LoginName);

            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.GetItemCategories(token.User.UserDomainID);
            } //if
            
            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }

        /// <summary>
        /// Возвращает с хранилища категорию товара.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="itemCategoryID">Код категории товара.</param>
        /// <returns>Категория товара.</returns>
        public ItemCategory GetItemCategory(SecurityToken token, Guid? itemCategoryID)
        {
            _logger.InfoFormat("Получение категории товара для пользователя {0} {1}", token.LoginName, itemCategoryID);

            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.GetItemCategory(itemCategoryID,
                                                                                 token.User.UserDomainID);
            }
            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }

        /// <summary>
        /// Сохраняет в хранилище категорию товара.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="itemCategory">Категория товара.</param>
        public void SaveItemCategory(SecurityToken token, ItemCategory itemCategory)
        {
            _logger.InfoFormat("Сохранение категории товара пользователем {0} {1}", token.LoginName, itemCategory.ItemCategoryID);
            itemCategory.UserDomainID = token.User.UserDomainID;
            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                RemontinkaServer.Instance.DataStore.SaveItemCategory(itemCategory);
                return;
            }

            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
        }

        /// <summary>
        /// Удаляет из хранилища категорию товара руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="itemCategoryID">Код категории товара.</param>
        public void DeleteItemCategory(SecurityToken token, Guid? itemCategoryID)
        {
            _logger.InfoFormat("Удаление категории товара {0} пользователем {1}", token.LoginName, itemCategoryID);

            if (token.User.ProjectRoleID != ProjectRoleSet.Admin.ProjectRoleID)
            {
                ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            } //if

            var itemCategory = RemontinkaServer.Instance.DataStore.GetItemCategory(itemCategoryID, token.User.UserDomainID);
            if (itemCategory != null)
            {
                RemontinkaServer.Instance.DataStore.DeleteItemCategory(itemCategoryID);
            } //if
        }

        #endregion ItemCategory

        #region Warehouse

        /// <summary>
        /// Получает список складов с фильтром.
        /// </summary>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список складов.</returns>
        public IEnumerable<Warehouse> GetWarehouses(SecurityToken token, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение списка складов для пользователя {0} {1}", token.LoginName, name);

            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.GetWarehouses(token.User.UserDomainID, name, page, pageSize, out count);
            } //if

            count = 0;
            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }

        /// <summary>
        /// Получает список складов для пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список складов.</returns>
        public IQueryable<Warehouse> GetWarehouses(SecurityToken token)
        {
            _logger.InfoFormat("Получение списка складов для пользователя {0}", token.LoginName);

            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.GetWarehouses(token.User.UserDomainID);
            } //if
            return RemontinkaServer.Instance.DataStore.GetWarehouses(token.User.UserID,token.User.UserDomainID);
            
        }

        /// <summary>
        /// Возвращает с хранилища склад.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="warehouseID">Код склада.</param>
        /// <returns>Склад.</returns>
        public Warehouse GetWarehouse(SecurityToken token, Guid? warehouseID)
        {
            _logger.InfoFormat("Получение склада для пользователя {0} {1}", token.LoginName, warehouseID);

            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.GetWarehouse(warehouseID,
                                                                                 token.User.UserDomainID);
            }
            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }

        /// <summary>
        /// Сохраняет в хранилище склад.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="warehouse">Склад.</param>
        public void SaveWarehouse(SecurityToken token, Warehouse warehouse)
        {
            _logger.InfoFormat("Сохранение склада пользователем {0} {1}", token.LoginName, warehouse.WarehouseID);
            warehouse.UserDomainID = token.User.UserDomainID;
            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                RemontinkaServer.Instance.DataStore.SaveWarehouse(warehouse);
                return;
            }

            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
        }

        /// <summary>
        /// Удаляет из хранилища склад руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="warehouseID">Код склада.</param>
        public void DeleteWarehouse(SecurityToken token, Guid? warehouseID)
        {
            _logger.InfoFormat("Удаление склада {0} пользователем {1}", token.LoginName, warehouseID);

            if (token.User.ProjectRoleID != ProjectRoleSet.Admin.ProjectRoleID)
            {
                ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            } //if

            var warehouse = RemontinkaServer.Instance.DataStore.GetWarehouse(warehouseID, token.User.UserDomainID);
            if (warehouse != null)
            {
                RemontinkaServer.Instance.DataStore.DeleteWarehouse(warehouseID);
            } //if
        }

        #endregion Warehouse

        #region GoodsItem

        /// <summary>
        /// Получает список номенклатуры с фильтром.
        /// </summary>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список номенклатуры.</returns>
        public IEnumerable<GoodsItemDTO> GetGoodsItems(SecurityToken token, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение списка номенклатуры для пользователя {0} {1}", token.LoginName, name);

            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.GetGoodsItems(token.User.UserDomainID, name, page, pageSize, out count);
            } //if

            count = 0;
            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }

        /// <summary>
        /// Получает список номенклатуры.
        /// </summary>
     
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список номенклатуры.</returns>
        public IQueryable<GoodsItem> GetGoodsItems(SecurityToken token)
        {
            _logger.InfoFormat("Получение списка номенклатуры для пользователя {0}", token.LoginName);

            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.GetGoodsItems(token.User.UserDomainID);
            } //if
            
            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }

        /// <summary>
        /// Возвращает с хранилища номенклатуру.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="goodsItemID">Код номенклатуры.</param>
        /// <returns>Номенклатура.</returns>
        public GoodsItemDTO GetGoodsItem(SecurityToken token, Guid? goodsItemID)
        {
            _logger.InfoFormat("Получение номенклатуры для пользователя {0} {1}", token.LoginName, goodsItemID);

            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.GetGoodsItem(goodsItemID,
                                                                                 token.User.UserDomainID);
            }
            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }

        /// <summary>
        /// Сохраняет в хранилище номенклатуру.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="goodsItem">Номенклатура.</param>
        public void SaveGoodsItem(SecurityToken token, GoodsItem goodsItem)
        {
            _logger.InfoFormat("Сохранение номенклатуры пользователем {0} {1}", token.LoginName, goodsItem.GoodsItemID);
            goodsItem.UserDomainID = token.User.UserDomainID;
            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                RemontinkaServer.Instance.DataStore.SaveGoodsItem(goodsItem);
                return;
            }

            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
        }

        /// <summary>
        /// Удаляет из хранилища номенклатуру руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="goodsItemID">Код номенклатуры.</param>
        public void DeleteGoodsItem(SecurityToken token, Guid? goodsItemID)
        {
            _logger.InfoFormat("Удаление номенклатураа {0} пользователем {1}", token.LoginName, goodsItemID);

            if (token.User.ProjectRoleID != ProjectRoleSet.Admin.ProjectRoleID)
            {
                ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            } //if

            var goodsItem = RemontinkaServer.Instance.DataStore.GetGoodsItem(goodsItemID, token.User.UserDomainID);
            if (goodsItem != null)
            {
                RemontinkaServer.Instance.DataStore.DeleteGoodsItem(goodsItemID);
            } //if
        }

        #endregion GoodsItem

        #region WarehouseItem

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
        public IEnumerable<WarehouseItemDTO> GetWarehouseItems(SecurityToken token,Guid? warehouseID, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение списка остатоки на складе для пользователя {0} {1}", token.LoginName, name);
            
            return RemontinkaServer.Instance.DataStore.GetWarehouseItems(token.User.UserDomainID,warehouseID, name, page, pageSize, out count);
        }

        /// <summary>
        /// Получает список остатков на складах.
        /// </summary>
        /// <returns>Список остатов на складе.</returns>
        public IQueryable<WarehouseItemDTO> GetWarehouseItems(SecurityToken token)
        {
            _logger.InfoFormat("Получение списка остатков на складах для пользователя {0}", token.LoginName);

            return RemontinkaServer.Instance.DataStore.GetWarehouseItems(token.User.UserDomainID);
        }

        /// <summary>
        /// Возвращает с хранилища остатки на складе.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="warehouseItemID">Код остатков на складе.</param>
        /// <returns>Остатки на складе.</returns>
        public WarehouseItemDTO GetWarehouseItem(SecurityToken token, Guid? warehouseItemID)
        {
            _logger.InfoFormat("Получение остатки на складе для пользователя {0} {1}", token.LoginName, warehouseItemID);

            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.GetWarehouseItem(warehouseItemID,
                                                                                 token.User.UserDomainID);
            }
            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }

        /// <summary>
        /// Сохраняет в хранилище остатки на складе.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="warehouseItem">Остатки на складе.</param>
        public void SaveWarehouseItem(SecurityToken token, WarehouseItem warehouseItem)
        {
            _logger.InfoFormat("Сохранение остатки на складе пользователем {0} {1}", token.LoginName, warehouseItem.WarehouseItemID);
            

            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                if (RemontinkaServer.Instance.DataStore.GetWarehouseUserDomainID(warehouseItem.WarehouseID)==token.User.UserDomainID)
                {
                    RemontinkaServer.Instance.DataStore.SaveWarehouseItem(warehouseItem);    
                } //if
                return;
            }

            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
        }

        /// <summary>
        /// Удаляет из хранилища остатки на складе руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="warehouseItemID">Код остатов на складе.</param>
        public void DeleteWarehouseItem(SecurityToken token, Guid? warehouseItemID)
        {
            _logger.InfoFormat("Удаление остатков на складе {0} пользователем {1}", token.LoginName, warehouseItemID);

            if (token.User.ProjectRoleID != ProjectRoleSet.Admin.ProjectRoleID)
            {
                ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            } //if


            if (RemontinkaServer.Instance.DataStore.GetWarehouseItemUserDomainID(warehouseItemID) ==
                token.User.UserDomainID)
            {
                RemontinkaServer.Instance.DataStore.DeleteWarehouseItem(warehouseItemID);
            } //if
        }

        #endregion WarehouseItem

        #region Contractor

        /// <summary>
        /// Получает список контрагентов с фильтром.
        /// </summary>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список контрагентов.</returns>
        public IEnumerable<Contractor> GetContractors(SecurityToken token, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение списка контрагентов для пользователя {0} {1}", token.LoginName, name);

            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.GetContractors(token.User.UserDomainID, name, page, pageSize, out count);
            } //if

            count = 0;
            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }

        /// <summary>
        /// Получает список всех контрагентов для пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список контрагентов.</returns>
        public IQueryable<Contractor> GetContractors(SecurityToken token)
        {
            _logger.InfoFormat("Получение списка всех контрагентов для пользователя {0}", token.LoginName);

            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.GetContractors(token.User.UserDomainID);
            } //if

            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }

        /// <summary>
        /// Возвращает с хранилища контрагента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="contractorID">Код контрагента.</param>
        /// <returns>Контрагент.</returns>
        public Contractor GetContractor(SecurityToken token, Guid? contractorID)
        {
            _logger.InfoFormat("Получение контрагента для пользователя {0} {1}", token.LoginName, contractorID);

            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.GetContractor(contractorID,
                                                                                 token.User.UserDomainID);
            }
            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }

        /// <summary>
        /// Сохраняет в хранилище контрагент.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="contractor">Контрагент.</param>
        public void SaveContractor(SecurityToken token, Contractor contractor)
        {
            _logger.InfoFormat("Сохранение контрагента пользователем {0} {1}", token.LoginName, contractor.ContractorID);
            contractor.UserDomainID = token.User.UserDomainID;
            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                RemontinkaServer.Instance.DataStore.SaveContractor(contractor);
                return;
            }

            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
        }

        /// <summary>
        /// Удаляет из хранилища контрагента руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="contractorID">Код контрагента.</param>
        public void DeleteContractor(SecurityToken token, Guid? contractorID)
        {
            _logger.InfoFormat("Удаление контрагента {0} пользователем {1}", token.LoginName, contractorID);

            if (token.User.ProjectRoleID != ProjectRoleSet.Admin.ProjectRoleID)
            {
                ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            } //if

            var contractor = RemontinkaServer.Instance.DataStore.GetContractor(contractorID, token.User.UserDomainID);
            if (contractor != null)
            {
                RemontinkaServer.Instance.DataStore.DeleteContractor(contractorID);
            } //if
        }

        #endregion Contractor

        #region IncomingDoc

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
        public IEnumerable<IncomingDocDTO> GetIncomingDocs(SecurityToken token, Guid? warehouseID, DateTime beginDate, DateTime endDate, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение списка приходных накладных для пользователя {0} {1}", token.LoginName, name);

            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.GetIncomingDocs(token.User.UserDomainID, warehouseID,
                                                                           beginDate, endDate, name, page, pageSize,
                                                                           out count);
            } //if

            count = 0;
            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }

        /// <summary>
        /// Получает список приходных накладных.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список приходных накладных.</returns>
        public IQueryable<IncomingDocDTO> GetIncomingDocs(SecurityToken token)
        {
            _logger.InfoFormat("Получение списка приходных накладных для пользователя {0}", token.LoginName);

            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.GetIncomingDocs(token.User.UserDomainID);
            } //if

            
            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }

        /// <summary>
        /// Возвращает с хранилища приходную накладную.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="incomingDocID">Код приходной накладной.</param>
        /// <returns>Приходная накладная.</returns>
        public IncomingDocDTO GetIncomingDoc(SecurityToken token, Guid? incomingDocID)
        {
            _logger.InfoFormat("Получение приходной накладной для пользователя {0} {1}", token.LoginName, incomingDocID);

            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.GetIncomingDoc(incomingDocID,
                                                                                 token.User.UserDomainID);
            }
            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }

        /// <summary>
        /// Сохраняет в хранилище приходную накладную.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="incomingDoc">Приходная накладная.</param>
        public void SaveIncomingDoc(SecurityToken token, IncomingDoc incomingDoc)
        {
            _logger.InfoFormat("Сохранение приходной накладной пользователем {0} {1}", token.LoginName, incomingDoc.IncomingDocID);
            incomingDoc.UserDomainID = token.User.UserDomainID;
            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                RemontinkaServer.Instance.DataStore.SaveIncomingDoc(incomingDoc);
                return;
            }

            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
        }

        /// <summary>
        /// Удаляет из хранилища приходную накладную руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="incomingDocID">Код приходной накладной.</param>
        public void DeleteIncomingDoc(SecurityToken token, Guid? incomingDocID)
        {
            _logger.InfoFormat("Удаление приходной накладной {0} пользователем {1}", token.LoginName, incomingDocID);

            if (token.User.ProjectRoleID != ProjectRoleSet.Admin.ProjectRoleID)
            {
                ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            } //if
            
            if (RemontinkaServer.Instance.DataStore.GetIncomingDocUserDomainID(incomingDocID)==token.User.UserDomainID)
            {
                RemontinkaServer.Instance.DataStore.DeleteIncomingDoc(incomingDocID);
            } //if
        }

        #endregion IncomingDoc

        #region IncomingDocItem
        
         /// <summary>
        /// Обрабатывает пункты приходной накладной.
        /// </summary>
        /// <param name="incomingDocID">Код приходной накладной.</param>
        /// <param name="token">Токен безопастности.</param>
        /// <param name="eventDate">Дата обработи документа </param>
        /// <param name="utcEventDateTime">UTC дата и время обработки документа. </param>
        public ProcessWarehouseDocResult ProcessIncomingDocItems(SecurityToken token,Guid? incomingDocID, DateTime eventDate, DateTime utcEventDateTime)
        {
            _logger.InfoFormat("Старт обработки пользователем документа {0} {1}", token.LoginName, incomingDocID);
             if (token.User.ProjectRoleID==ProjectRoleSet.Admin.ProjectRoleID)
             {
                 return RemontinkaServer.Instance.DataStore.ProcessIncomingDocItems(incomingDocID, token.User.UserDomainID,
                                                                             eventDate, utcEventDateTime,
                                                                             token.User.UserID);
             } //if

             ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
             return null;
        }

        /// <summary>
        /// Отменяет обработку пунктов приходной накладной.
        /// </summary>
        /// <param name="incomingDocID">Код приходной накладной.</param>
        /// <param name="token">Токен безопастности.</param>
        public ProcessWarehouseDocResult UnProcessIncomingDocItems(SecurityToken token, Guid? incomingDocID)
        {
            _logger.InfoFormat("Старт отмены обработки пользователем приходной накладной {0} {1}", token.LoginName, incomingDocID);
            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.UnProcessIncomingDocItems(incomingDocID, token.User.UserDomainID);
            } //if

            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }

        /// <summary>
        /// Получает список всех элементов приходной накладной.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="incomingDocID">Код приходной накладной.</param>
        /// <returns>Список элемент приходной накладной товаров.</returns>
        public IQueryable<IncomingDocItemDTO> GetIncomingDocItems(SecurityToken token, Guid? incomingDocID)
        {
            _logger.InfoFormat(
                "Получение списка всех элементов приходной накладной всех товаров пользователя {0} документ {1}",
                token.LoginName, incomingDocID);

            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.GetIncomingDocItems(token.User.UserDomainID, incomingDocID);
            } //if
            
            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }

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
        public IEnumerable<IncomingDocItemDTO> GetIncomingDocItems(SecurityToken token, Guid? incomingDocID, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение списка элементов приходной накладной для пользователя {0} {1}", token.LoginName, name);

            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.GetIncomingDocItems(token.User.UserDomainID, incomingDocID,name, page, pageSize,
                                                                           out count);
            } //if

            count = 0;
            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }

        /// <summary>
        /// Возвращает с хранилища элемент приходной накладной.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="incomingDocItemID">Код приходной накладной.</param>
        /// <returns>Приходная накладная.</returns>
        public IncomingDocItemDTO GetIncomingDocItem(SecurityToken token, Guid? incomingDocItemID)
        {
            _logger.InfoFormat("Получение элемента приходной накладной для пользователя {0} {1}", token.LoginName, incomingDocItemID);

            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.GetIncomingDocItem(incomingDocItemID,
                                                                                 token.User.UserDomainID);
            }
            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }

        /// <summary>
        /// Сохраняет в хранилище элемент приходной накладной.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="incomingDocItem">Элемент приходной накладной.</param>
        public void SaveIncomingDocItem(SecurityToken token, IncomingDocItem incomingDocItem)
        {
            _logger.InfoFormat("Сохранение элемента приходной накладной пользователем {0} {1}", token.LoginName, incomingDocItem.IncomingDocItemID);
            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                if (RemontinkaServer.Instance.DataStore.GetIncomingDocUserDomainID(incomingDocItem.IncomingDocID)==token.User.UserDomainID)
                {
                    RemontinkaServer.Instance.DataStore.SaveIncomingDocItem(incomingDocItem);    
                } //if
                else
                {
                    _logger.WarnFormat("Пользователь {0} записывает элемент накладной не в свой документ домена {1}",
                                       token.LoginName, incomingDocItem.IncomingDocID);
                } //else
                return;
            }

            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
        }

        /// <summary>
        /// Удаляет из хранилища элемент приходной накладной руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="incomingDocItemID">Код элемента приходной накладной.</param>
        public void DeleteIncomingDocItem(SecurityToken token, Guid? incomingDocItemID)
        {
            _logger.InfoFormat("Удаление элемента приходной накладной {0} пользователем {1}", token.LoginName, incomingDocItemID);

            if (token.User.ProjectRoleID != ProjectRoleSet.Admin.ProjectRoleID)
            {
                ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            } //if

            if (RemontinkaServer.Instance.DataStore.GetIncomingDocItemUserDomainID(incomingDocItemID) == token.User.UserDomainID)
            {
                RemontinkaServer.Instance.DataStore.DeleteIncomingDocItem(incomingDocItemID);
            } //if
        }

        #endregion IncomingDocItem

        #region CancellationDoc

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
        public IEnumerable<CancellationDocDTO> GetCancellationDocs(SecurityToken token, Guid? warehouseID, DateTime beginDate, DateTime endDate, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение списка документов списаний для пользователя {0} {1}", token.LoginName, name);

            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.GetCancellationDocs(token.User.UserDomainID, warehouseID,
                                                                           beginDate, endDate, name, page, pageSize,
                                                                           out count);
            } //if

            count = 0;
            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }

        /// <summary>
        /// Получает список документов списаний.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список списаний.</returns>
        public IQueryable<CancellationDocDTO> GetCancellationDocs(SecurityToken token)
        {
            _logger.InfoFormat("Получение списка документов списаний для пользователя {0}", token.LoginName);

            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.GetCancellationDocs(token.User.UserDomainID);
            } //if
            
            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }

        /// <summary>
        /// Возвращает с хранилища документ списаний.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="cancellationDocID">Код документа списаний.</param>
        /// <returns>Документ списания.</returns>
        public CancellationDocDTO GetCancellationDoc(SecurityToken token, Guid? cancellationDocID)
        {
            _logger.InfoFormat("Получение документ списаний для пользователя {0} {1}", token.LoginName, cancellationDocID);

            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.GetCancellationDoc(cancellationDocID,
                                                                                 token.User.UserDomainID);
            }
            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }

        /// <summary>
        /// Сохраняет в хранилище документ списаний.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="cancellationDoc">Документ списаний.</param>
        public void SaveCancellationDoc(SecurityToken token, CancellationDoc cancellationDoc)
        {
            _logger.InfoFormat("Сохранение документ списаний пользователем {0} {1}", token.LoginName, cancellationDoc.CancellationDocID);
            cancellationDoc.UserDomainID = token.User.UserDomainID;
            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                RemontinkaServer.Instance.DataStore.SaveCancellationDoc(cancellationDoc);
                return;
            }

            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
        }

        /// <summary>
        /// Удаляет из хранилища документ списаний руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="cancellationDocID">Код документа списаний.</param>
        public void DeleteCancellationDoc(SecurityToken token, Guid? cancellationDocID)
        {
            _logger.InfoFormat("Удаление документ списаний {0} пользователем {1}", token.LoginName, cancellationDocID);

            if (token.User.ProjectRoleID != ProjectRoleSet.Admin.ProjectRoleID)
            {
                ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            } //if

            if (RemontinkaServer.Instance.DataStore.GetCancellationDocUserDomainID(cancellationDocID) == token.User.UserDomainID)
            {
                RemontinkaServer.Instance.DataStore.DeleteCancellationDoc(cancellationDocID);
            } //if
        }

        #endregion CancellationDoc

        #region CancellationDocItem

        /// <summary>
        /// Обрабатывает пункты документа о списании.
        /// </summary>
        /// <param name="cancellationDocID">Код документа о списании.</param>
        /// <param name="token">Токен безопастности.</param>
        /// <param name="eventDate">Дата обработи документа </param>
        /// <param name="utcEventDateTime">UTC дата и время обработки документа. </param>
        public ProcessWarehouseDocResult ProcessCancellationDocItems(SecurityToken token, Guid? cancellationDocID, DateTime eventDate, DateTime utcEventDateTime)
        {
            _logger.InfoFormat("Старт обработки пользователем документа {0} {1}", token.LoginName, cancellationDocID);
            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.ProcessCancellationDocItems(cancellationDocID, token.User.UserDomainID,
                                                                            eventDate, utcEventDateTime,
                                                                            token.User.UserID);
            } //if

            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }

        /// <summary>
        /// Отменяет обрабатку пунктов документа о списании.
        /// </summary>
        /// <param name="cancellationDocID">Код документа о списании.</param>
        /// <param name="token">Токен безопастности.</param>
        public ProcessWarehouseDocResult UnProcessCancellationDocItems(SecurityToken token, Guid? cancellationDocID)
        {
            _logger.InfoFormat("Старт отмены обработки пользователем документа {0} {1}", token.LoginName, cancellationDocID);
            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.UnProcessCancellationDocItems(cancellationDocID, token.User.UserDomainID);
            } //if

            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }


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
        public IEnumerable<CancellationDocItemDTO> GetCancellationDocItems(SecurityToken token, Guid? cancellationDocID, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение списка элементов документов списания для пользователя {0} {1}", token.LoginName, name);

            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.GetCancellationDocItems(token.User.UserDomainID, cancellationDocID, name, page, pageSize,
                                                                           out count);
            } //if

            count = 0;
            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }

        /// <summary>
        /// Получает список элементов документов списания.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="cancellationDocID">Код документа списания. </param>
        /// <returns>Список элементов документов списания.</returns>
        public IQueryable<CancellationDocItemDTO> GetCancellationDocItems(SecurityToken token, Guid? cancellationDocID)
        {
            _logger.InfoFormat("Получение списка элементов документов списания для пользователя {0} {1}", token.LoginName, cancellationDocID);

            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.GetCancellationDocItems(token.User.UserDomainID, cancellationDocID);
            } //if
            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }

        /// <summary>
        /// Возвращает с хранилища элемент документа списания.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="cancellationDocItemID">Код документа списания.</param>
        /// <returns>Документа списания.</returns>
        public CancellationDocItemDTO GetCancellationDocItem(SecurityToken token, Guid? cancellationDocItemID)
        {
            _logger.InfoFormat("Получение элемента документа списания для пользователя {0} {1}", token.LoginName, cancellationDocItemID);

            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.GetCancellationDocItem(cancellationDocItemID,
                                                                                 token.User.UserDomainID);
            }
            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }

        /// <summary>
        /// Сохраняет в хранилище элемент документа списания.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="cancellationDocItem">Элемент документа списания.</param>
        public void SaveCancellationDocItem(SecurityToken token, CancellationDocItem cancellationDocItem)
        {
            _logger.InfoFormat("Сохранение элемента документа списания пользователем {0} {1}", token.LoginName, cancellationDocItem.CancellationDocItemID);
            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                if (RemontinkaServer.Instance.DataStore.GetCancellationDocUserDomainID(cancellationDocItem.CancellationDocID) == token.User.UserDomainID)
                {
                    RemontinkaServer.Instance.DataStore.SaveCancellationDocItem(cancellationDocItem);
                } //if
                else
                {
                    _logger.WarnFormat("Пользователь {0} записывает элемент документа списания не в свой документ домена {1}",
                                       token.LoginName, cancellationDocItem.CancellationDocID);
                } //else
                return;
            }

            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
        }

        /// <summary>
        /// Удаляет из хранилища элемент документа списания руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="cancellationDocItemID">Код элемента документа списания.</param>
        public void DeleteCancellationDocItem(SecurityToken token, Guid? cancellationDocItemID)
        {
            _logger.InfoFormat("Удаление элемента списания {0} пользователем {1}", token.LoginName, cancellationDocItemID);

            if (token.User.ProjectRoleID != ProjectRoleSet.Admin.ProjectRoleID)
            {
                ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            } //if

            if (RemontinkaServer.Instance.DataStore.GetCancellationDocItemUserDomainID(cancellationDocItemID) == token.User.UserDomainID)
            {
                RemontinkaServer.Instance.DataStore.DeleteCancellationDocItem(cancellationDocItemID);
            } //if
        }

        #endregion CancellationDocItem

        #region TransferDoc

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
        public IEnumerable<TransferDocDTO> GetTransferDocs(SecurityToken token, Guid? senderWarehouseID, Guid? recipientWarehouseID, DateTime beginDate, DateTime endDate, string name, int page, int pageSize, out int count)
        
        {
            _logger.InfoFormat("Получение списка документов перемещений для пользователя {0} {1}", token.LoginName, name);

            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.GetTransferDocs(token.User.UserDomainID, senderWarehouseID,recipientWarehouseID,
                                                                           beginDate, endDate, name, page, pageSize,
                                                                           out count);
            } //if

            count = 0;
            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }

        /// <summary>
        /// Получает список перемещений со склада на склад.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список перемещений со склада товаров.</returns>
        public IQueryable<TransferDocDTO> GetTransferDocs(SecurityToken token)

        {
            _logger.InfoFormat("Получение списка документов перемещений для пользователя {0}", token.LoginName);

            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.GetTransferDocs(token.User.UserDomainID);
            } //if
            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }

        /// <summary>
        /// Возвращает с хранилища документ перемещений.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="transferDocID">Код документа перемещений.</param>
        /// <returns>Документ перемещения.</returns>
        public TransferDocDTO GetTransferDoc(SecurityToken token, Guid? transferDocID)
        {
            _logger.InfoFormat("Получение документ перемещений для пользователя {0} {1}", token.LoginName, transferDocID);

            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.GetTransferDoc(transferDocID,
                                                                                 token.User.UserDomainID);
            }
            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }

        /// <summary>
        /// Сохраняет в хранилище документ перемещений.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="transferDoc">Документ перемещений.</param>
        public void SaveTransferDoc(SecurityToken token, TransferDoc transferDoc)
        {
            _logger.InfoFormat("Сохранение документ перемещений пользователем {0} {1}", token.LoginName, transferDoc.TransferDocID);
            transferDoc.UserDomainID = token.User.UserDomainID;
            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                RemontinkaServer.Instance.DataStore.SaveTransferDoc(transferDoc);
                return;
            }

            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
        }

        /// <summary>
        /// Удаляет из хранилища документ перемещений руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="transferDocID">Код документа перемещений.</param>
        public void DeleteTransferDoc(SecurityToken token, Guid? transferDocID)
        {
            _logger.InfoFormat("Удаление документа перемещений {0} пользователем {1}", token.LoginName, transferDocID);

            if (token.User.ProjectRoleID != ProjectRoleSet.Admin.ProjectRoleID)
            {
                ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            } //if

            if (RemontinkaServer.Instance.DataStore.GetTransferDocUserDomainID(transferDocID) == token.User.UserDomainID)
            {
                RemontinkaServer.Instance.DataStore.DeleteTransferDoc(transferDocID);
            } //if
        }

        #endregion TransferDoc

        #region TransferDocItem

        /// <summary>
        /// Обрабатывает пункты документа о перемещении со склада на склад.
        /// </summary>
        /// <param name="transferDocID">Код документа о перемещении.</param>
        /// <param name="token">Токен безопастности.</param>
        /// <param name="eventDate">Дата обработи документа </param>
        /// <param name="utcEventDateTime">UTC дата и время обработки документа. </param>
        public ProcessWarehouseDocResult ProcessTransferDocItems(SecurityToken token, Guid? transferDocID, DateTime eventDate, DateTime utcEventDateTime)
        {
            _logger.InfoFormat("Старт обработки пользователем документа о перемещении {0} {1}", token.LoginName, transferDocID);
            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.ProcessTransferDocItems(transferDocID, token.User.UserDomainID,
                                                                            eventDate, utcEventDateTime,
                                                                            token.User.UserID);
            } //if

            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }

        /// <summary>
        /// Отменяет обработанные пункты документа о перемещении со склада на склад.
        /// </summary>
        /// <param name="transferDocID">Код отменяемого документа о перемещении.</param>
        /// <param name="token">Токен безопастности.</param>
        public ProcessWarehouseDocResult UnProcessTransferDocItems(SecurityToken token, Guid? transferDocID)
        {
            _logger.InfoFormat("Старт отмены обработки пользователем документа о перемещении {0} {1}", token.LoginName, transferDocID);
            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.UnProcessTransferDocItems(transferDocID, token.User.UserDomainID);
            } //if

            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }

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
        public IEnumerable<TransferDocItemDTO> GetTransferDocItems(SecurityToken token, Guid? transferDocID, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение списка элементов документов перемещения для пользователя {0} {1}", token.LoginName, name);

            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.GetTransferDocItems(token.User.UserDomainID, transferDocID, name, page, pageSize,
                                                                           out count);
            } //if

            count = 0;
            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }

        /// <summary>
        /// Получает список элементов документов перемещения.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="transferDocID">Код документа перемещения. </param>
        /// <returns>Список элементов документов перемещения.</returns>
        public IQueryable<TransferDocItemDTO> GetTransferDocItems(SecurityToken token, Guid? transferDocID)
        {
            _logger.InfoFormat("Получение списка элементов документов перемещения для пользователя {0} {1}", token.LoginName, transferDocID);

            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.GetTransferDocItems(token.User.UserDomainID, transferDocID);
            } //if
            
            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }

        /// <summary>
        /// Возвращает с хранилища элемент документа перемещения.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="transferDocItemID">Код документа перемещения.</param>
        /// <returns>Документа перемещения.</returns>
        public TransferDocItemDTO GetTransferDocItem(SecurityToken token, Guid? transferDocItemID)
        {
            _logger.InfoFormat("Получение элемента документа перемещения для пользователя {0} {1}", token.LoginName, transferDocItemID);

            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.GetTransferDocItem(transferDocItemID,
                                                                                 token.User.UserDomainID);
            }
            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }

        /// <summary>
        /// Сохраняет в хранилище элемент документа перемещения.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="transferDocItem">Элемент документа перемещения.</param>
        public void SaveTransferDocItem(SecurityToken token, TransferDocItem transferDocItem)
        {
            _logger.InfoFormat("Сохранение элемента документа перемещения пользователем {0} {1}", token.LoginName, transferDocItem.TransferDocItemID);
            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                if (RemontinkaServer.Instance.DataStore.GetTransferDocUserDomainID(transferDocItem.TransferDocID) == token.User.UserDomainID)
                {
                    RemontinkaServer.Instance.DataStore.SaveTransferDocItem(transferDocItem);
                } //if
                else
                {
                    _logger.WarnFormat("Пользователь {0} записывает элемент документа перемещения не в свой документ домена {1}",
                                       token.LoginName, transferDocItem.TransferDocID);
                } //else
                return;
            }

            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
        }

        /// <summary>
        /// Удаляет из хранилища элемент документа перемещения руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="transferDocItemID">Код элемента документа перемещения.</param>
        public void DeleteTransferDocItem(SecurityToken token, Guid? transferDocItemID)
        {
            _logger.InfoFormat("Удаление элемента перемещения {0} пользователем {1}", token.LoginName, transferDocItemID);

            if (token.User.ProjectRoleID != ProjectRoleSet.Admin.ProjectRoleID)
            {
                ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            } //if

            if (RemontinkaServer.Instance.DataStore.GetTransferDocItemUserDomainID(transferDocItemID) == token.User.UserDomainID)
            {
                RemontinkaServer.Instance.DataStore.DeleteTransferDocItem(transferDocItemID);
            } //if
        }

        #endregion TransferDocItem

        #region ProcessedWarehouseDoc

        /// <summary>
        /// Проверяет документ на признак обработки.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="docID">Код документа.</param>
        /// <returns>Признак обработки.</returns>
        public bool WarehouseDocIsProcessed(SecurityToken token, Guid? docID)
        {
            _logger.InfoFormat("Проверка на обработку документа пользователем {0}; {1}", docID, token.LoginName);

            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                var processedDoc = RemontinkaServer.Instance.DataStore.GetProcessedWarehouseDoc(docID);
                if (processedDoc==null)
                {
                    return false;
                } //if

                var warehouse = RemontinkaServer.Instance.DataStore.GetWarehouse(processedDoc.WarehouseID,token.User.UserDomainID);
                if (warehouse!=null)
                {
                    return true;
                } //if

                return false;
            }
            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return false;
        }

        #endregion ProcessedWarehouseDoc

        #region UserPublicKeyRequest

        /// <summary>
        /// Получает список запросов на активацию публичных ключей с фильтром.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список запросов на активацию.</returns>
        public IQueryable<UserPublicKeyRequestDTO> GetUserPublicKeyRequests(SecurityToken token)
        {
            _logger.InfoFormat("Получение списка запросов на активацию ключей для пользователя {0}", token.LoginName);

            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.GetUserPublicKeyRequests(token.User.UserDomainID);
            } //if
            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }

        /// <summary>
        /// Получает список запросов на активацию публичных ключей с фильтром.
        /// </summary>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список запросов на активацию.</returns>
        public IEnumerable<UserPublicKeyRequestDTO> GetUserPublicKeyRequests(SecurityToken token, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение списка запросов на активацию ключей для пользователя {0} {1}", token.LoginName, name);

            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.GetUserPublicKeyRequests(token.User.UserDomainID, name, page, pageSize, out count);
            } //if

            count = 0;
            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }

        /// <summary>
        /// Возвращает с хранилища запрос на активацию ключа.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="userPublicKeyRequestID">Код запроса на активацию публичного ключа.</param>
        /// <returns>Запрос на активацию.</returns>
        public UserPublicKeyRequestDTO GetUserPublicKeyRequest(SecurityToken token, Guid? userPublicKeyRequestID)
        {
            _logger.InfoFormat("Получение номенклатуры для пользователя {0} {1}", token.LoginName, userPublicKeyRequestID);

            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.GetUserPublicKeyRequest(userPublicKeyRequestID,
                                                                                   token.User.UserDomainID);
            }
            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }
        
        /// <summary>
        /// Удаляет из хранилища запрос на регистрацию публичного ключа руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="userPublicKeyRequestID">Код запроса на регистрацию публичного ключа.</param>
        public void DeleteUserPublicKeyRequest(SecurityToken token, Guid? userPublicKeyRequestID)
        {
            _logger.InfoFormat("Удаление запроса на регистрацию публичного ключа {0} пользователем {1}", token.LoginName, userPublicKeyRequestID);

            if (token.User.ProjectRoleID != ProjectRoleSet.Admin.ProjectRoleID)
            {
                ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            } //if

            var userPublicKeyRequest = RemontinkaServer.Instance.DataStore.GetUserPublicKeyRequest(userPublicKeyRequestID, token.User.UserDomainID);
            if (userPublicKeyRequest != null)
            {
                RemontinkaServer.Instance.DataStore.DeleteUserPublicKeyRequest(userPublicKeyRequestID);
            } //if
        }

        #endregion UserPublicKeyRequest

        #region UserPublicKey

        /// <summary>
        /// Получает список публичных ключей с фильтром.
        /// </summary>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список публичных ключей.</returns>
        public IEnumerable<UserPublicKeyDTO> GetUserPublicKeys(SecurityToken token, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение списка публичных ключей для пользователя {0} {1}", token.LoginName, name);

            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.GetUserPublicKeys(token.User.UserDomainID, name, page, pageSize, out count);
            } //if

            count = 0;
            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }

        /// <summary>
        /// Получает список публичных ключей.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список публичных ключей.</returns>
        public IQueryable<UserPublicKeyDTO> GetUserPublicKeys(SecurityToken token)
        {
            _logger.InfoFormat("Получение списка публичных ключей для пользователя {0}", token.LoginName);

            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.GetUserPublicKeys(token.User.UserDomainID);
            } //if
            
            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }

        /// <summary>
        /// Возвращает с хранилища публичный ключ.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="userPublicKeyID">Код публичных ключей.</param>
        /// <returns>Публичный ключ.</returns>
        public UserPublicKeyDTO GetUserPublicKey(SecurityToken token, Guid? userPublicKeyID)
        {
            _logger.InfoFormat("Получение публичного ключа для пользователя {0} {1}", token.LoginName, userPublicKeyID);

            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.GetUserPublicKey(userPublicKeyID,
                                                                                 token.User.UserDomainID);
            }
            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            return null;
        }

        /// <summary>
        /// Сохраняет в хранилище публичный ключ.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="userPublicKey">Публичный ключ.</param>
        public void SaveUserPublicKey(SecurityToken token, UserPublicKey userPublicKey)
        {
            _logger.InfoFormat("Сохранение публичных ключей пользователем {0} {1}", token.LoginName, userPublicKey.UserPublicKeyID);
            
            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                var user = RemontinkaServer.Instance.DataStore.GetUser(userPublicKey.UserID, token.User.UserDomainID);
                if (user!=null)
                {
                    RemontinkaServer.Instance.DataStore.SaveUserPublicKey(userPublicKey);    
                } //if
                return;
            }

            ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
        }

        /// <summary>
        /// Удаляет из хранилища публичный ключ руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="userPublicKeyID">Код публичного ключа.</param>
        public void DeleteUserPublicKey(SecurityToken token, Guid? userPublicKeyID)
        {
            _logger.InfoFormat("Удаление публичного ключа {0} пользователем {1}", token.LoginName, userPublicKeyID);

            if (token.User.ProjectRoleID != ProjectRoleSet.Admin.ProjectRoleID)
            {
                ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            } //if

            var userPublicKey = RemontinkaServer.Instance.DataStore.GetUserPublicKey(userPublicKeyID, token.User.UserDomainID);
            if (userPublicKey != null)
            {
                RemontinkaServer.Instance.DataStore.DeleteUserPublicKey(userPublicKeyID);
            } //if
        }

        #endregion UserPublicKey

        #region AutocompleteItem

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
        public IEnumerable<AutocompleteItem> GetAutocompleteItems(SecurityToken token, byte? autocompleteKindID, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение списка пунктов автозаполнения для пользователя {0} {1}", token.LoginName, name);

            return RemontinkaServer.Instance.DataStore.GetAutocompleteItems(token.User.UserDomainID, autocompleteKindID,name, page, pageSize, out count);
        }

        /// <summary>
        /// Получает список всех пунктов автозаполнения для пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список пунктов автозаполнения.</returns>
        public IQueryable<AutocompleteItem> GetAutocompleteItems(SecurityToken token)
        {
            _logger.InfoFormat("Получение всех пунктов автозаполнения для пользователя {0}", token.LoginName);
            return RemontinkaServer.Instance.DataStore.GetAutocompleteItems(token.User.UserDomainID);
        }

        /// <summary>
        /// Получает список всех пунктов автозаполнения для пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="autocompleteKindID">Код типа пункта автозаполнения.</param>
        /// <returns>Список пунктов автозаполнений.</returns>
        public IEnumerable<AutocompleteItem> GetAutocompleteItems(SecurityToken token, byte? autocompleteKindID)
        {
            _logger.InfoFormat("Получение всех пунктов автозаполнения для пользователя {0} типа {1}", token.LoginName, autocompleteKindID);
            return RemontinkaServer.Instance.DataStore.GetAutocompleteItems(token.User.UserDomainID, autocompleteKindID);
        }

        /// <summary>
        /// Возвращает с хранилища пункт автозаполнения.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="autocompleteItemID">Код статуса заказа.</param>
        /// <returns>Пункт автозаполнения</returns>
        public AutocompleteItem GetAutocompleteItem(SecurityToken token, Guid? autocompleteItemID)
        {
            _logger.InfoFormat("Получение пункта автозаполнения для пользователя {0} {1}", token.LoginName, autocompleteItemID);

            return RemontinkaServer.Instance.DataStore.GetAutocompleteItem(autocompleteItemID, token.User.UserDomainID);
        }

        /// <summary>
        /// Сохраняет в хранилище пункт автозаполнения.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="autocompleteItem">Пункт автозаполнения.</param>
        public void SaveAutocompleteItem(SecurityToken token, AutocompleteItem autocompleteItem)
        {
            _logger.InfoFormat("Сохранение пункт автозаполнения пользователем {0} {1}", token.LoginName, autocompleteItem.AutocompleteItemID);
            autocompleteItem.UserDomainID = token.User.UserDomainID;
            RemontinkaServer.Instance.SecurityService.AdminRightsEvaluate(token);
            RemontinkaServer.Instance.DataStore.SaveAutocompleteItem(autocompleteItem);
        }

        /// <summary>
        /// Удаляет из хранилища пункт автозаполнения руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="autocompleteItemID">Код пункта автозаполнения.</param>
        public void DeleteAutocompleteItem(SecurityToken token, Guid? autocompleteItemID)
        {
            _logger.InfoFormat("Удаление пункта автозаполнения {0} пользователем {1}", token.LoginName,
                               autocompleteItemID);
            RemontinkaServer.Instance.SecurityService.AdminRightsEvaluate(token);

            var autocompleteItem = RemontinkaServer.Instance.DataStore.GetAutocompleteItem(autocompleteItemID,
                                                                                           token.User.UserDomainID);
            if (autocompleteItem != null)
            {
                RemontinkaServer.Instance.DataStore.DeleteAutocompleteItem(autocompleteItemID);
            } //if
        }

        #endregion AutocompleteItem

        #region UserInterest

        /// <summary>
        /// Получает список пунктов вознаграждения с фильтром.
        /// </summary>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список пунктов вознаграждения.</returns>
        public IEnumerable<UserInterestDTO> GetUserInterests(SecurityToken token, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение списка вознаграждений для пользователя {0} {1}", token.LoginName, name);

            return RemontinkaServer.Instance.DataStore.GetUserInterests(token.User.UserDomainID, name, page, pageSize, out count);
        }

        /// <summary>
        /// Получает список пунктов вознаграждения.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список пунктов вознаграждения.</returns>
        public IQueryable<UserInterest> GetUserInterests(SecurityToken token)
        {
            _logger.InfoFormat("Получение списка вознаграждений без фильтра для пользователя {0}", token.LoginName);

            return RemontinkaServer.Instance.DataStore.GetUserInterests(token.User.UserDomainID);
        }

        /// <summary>
        /// Возвращает с хранилища пункт вознаграждения.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="userInterestID">Код статуса заказа.</param>
        /// <returns>Пункт вознаграждения</returns>
        public UserInterestDTO GetUserInterest(SecurityToken token, Guid? userInterestID)
        {
            _logger.InfoFormat("Получение пункта вознаграждения для пользователя {0} {1}", token.LoginName, userInterestID);

            return RemontinkaServer.Instance.DataStore.GetUserInterest(userInterestID, token.User.UserDomainID);
        }

        /// <summary>
        /// Сохраняет в хранилище пункт вознаграждения.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="userInterest">Пункт вознаграждения.</param>
        public void SaveUserInterest(SecurityToken token, UserInterest userInterest)
        {
            _logger.InfoFormat("Сохранение пункт вознаграждения пользователем {0} {1}", token.LoginName, userInterest.UserInterestID);
            RemontinkaServer.Instance.SecurityService.AdminRightsEvaluate(token);
            RemontinkaServer.Instance.DataStore.SaveUserInterest(userInterest);
        }

        /// <summary>
        /// Удаляет из хранилища пункт вознаграждения руководствуясь привилегиями данного пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="userInterestID">Код пункта вознаграждения.</param>
        public void DeleteUserInterest(SecurityToken token, Guid? userInterestID)
        {
            _logger.InfoFormat("Удаление пункта вознаграждения {0} пользователем {1}", token.LoginName,
                               userInterestID);
            RemontinkaServer.Instance.SecurityService.AdminRightsEvaluate(token);

            RemontinkaServer.Instance.DataStore.DeleteUserInterest(userInterestID,token.User.UserDomainID);
            
        }

        #endregion UserInterest

        #region Reports

        /// <summary>
        /// Получает пункты отчета для работы инженеров.
        /// </summary>
        /// <param name="token">Код домена.</param>
        /// <param name="engineerID">Код инженера, может быть null, тогда собираются данные по всем инженерам.</param>
        /// <param name="beginDate">Дата начала.</param>
        /// <param name="endDate">Дата окончания периода.</param>
        /// <returns>Списко пунктов отчета.</returns>
        public IEnumerable<EngineerWorkReportItem> GetEngineerWorkReportItems(SecurityToken token, Guid? engineerID, DateTime beginDate, DateTime endDate)
        {
            _logger.InfoFormat("Получение отчета по проделанной работе инженера пользователем {0}", token.User.LoginName);
            if (token.User.ProjectRoleID == ProjectRoleSet.Manager.ProjectRoleID)
            {
                _logger.WarnFormat("Получение отчета по проделанной работе менеджером не предусмотрена {0}",token.User.LoginName);
                return new EngineerWorkReportItem[0];//Получение пунктов работы менеджером пока не предусмотренно.
            }

            if (token.User.ProjectRoleID == ProjectRoleSet.Engineer.ProjectRoleID)
            {
                engineerID = token.User.UserID;
               return RemontinkaServer.Instance.DataStore.GetEngineerWorkReportItems(token.User.UserDomainID, engineerID,
                                                                               beginDate, endDate);
            }

            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return RemontinkaServer.Instance.DataStore.GetEngineerWorkReportItems(token.User.UserDomainID, engineerID,
                                                                               beginDate, endDate);
            }

            return new EngineerWorkReportItem[0];
        }

        /// <summary>
        /// Получение отчета по пользовательским данным по расходам и доходам.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="financialGroupID">Код финансовой группы филиалов.</param>
        /// <param name="beginDate">Дата начала.</param>
        /// <param name="endDate">Дата завершения.</param>
        /// <returns>Список пунктов отчета.</returns>
        public IEnumerable<RevenueAndExpenditureReportItem> GetRevenueAndExpenditureReportItems(SecurityToken token, Guid? financialGroupID, DateTime beginDate, DateTime endDate)
        {
            _logger.InfoFormat("Получение пунктов отчета по доходам и расходам группы {0} пользователем {1}",
                               financialGroupID, token.LoginName);
            if (token.User.ProjectRoleID!=ProjectRoleSet.Admin.ProjectRoleID)
            {
                ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            }
            
            return RemontinkaServer.Instance.DataStore.GetRevenueAndExpenditureReportItems(token.User.UserDomainID,
                                                                                           financialGroupID, beginDate,
                                                                                           endDate);
        }

        /// <summary>
        /// Получает общую информацию по установленным запчастям и выполненным работам за определенный период выдачи клиентам для фин группы.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="financialGroupID">Код финансовой группы.</param>
        /// <param name="beginDate">Дата начала.</param>
        /// <param name="endDate">Дата завершения.</param>
        /// <returns>Информация.</returns>
        public ItemsInfo GetOrderPaidAmountByOrderIssueDate(SecurityToken token, Guid? financialGroupID, DateTime beginDate, DateTime endDate)
        {
            _logger.InfoFormat("Получение общей информации по работам и устройствам группы {0} пользователем {1}",
                               financialGroupID, token.LoginName);

            if (token.User.ProjectRoleID != ProjectRoleSet.Admin.ProjectRoleID)
            {
                ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            }
            return RemontinkaServer.Instance.DataStore.GetOrderPaidAmountByOrderIssueDate(token.User.UserDomainID,
                                                                                           financialGroupID, beginDate,
                                                                                           endDate);
        }

        /// <summary>
        /// Получение пункты отчета по завершенным приходным накладным для финансовой группы за определенный период.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="financialGroupID">Код финансовой группы.</param>
        /// <param name="beginDate">Дата начала.</param>
        /// <param name="endDate">Дата окончания.</param>
        /// <returns>Пункты отчета.</returns>
        public IEnumerable<WarehouseDocTotalItem> GetWarehouseDocTotalItems(SecurityToken token, Guid? financialGroupID, DateTime beginDate, DateTime endDate)
        {
            _logger.InfoFormat("Получение пунктов отчета по завершенным прикладным накладным фин группы {0} пользователем {1}",
                               financialGroupID, token.LoginName);

            if (token.User.ProjectRoleID != ProjectRoleSet.Admin.ProjectRoleID)
            {
                ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            }
            return RemontinkaServer.Instance.DataStore.GetWarehouseDocTotalItems(token.User.UserDomainID,
                                                                                           financialGroupID, beginDate,
                                                                                           endDate);
        }

        /// <summary>
        /// Получает отчет по используемым запчастям за определенный период времени.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="branchID">Код филиала.</param>
        /// <param name="financialGroupID">Код финансовой группы.</param>
        /// <param name="beginDate">Дата начала.</param>
        /// <param name="endDate">Дата окончания.</param>
        /// <returns>Пункты отчета.</returns>
        public IEnumerable<UsedDeviceItemsReportItem> GetUsedDeviceItemsReportItems(SecurityToken token, Guid? branchID, Guid? financialGroupID, DateTime beginDate, DateTime endDate)
        {
            _logger.InfoFormat("Получение отчета по используемым запчастям пользователем {0} филиала {1} группы {2}", token.LoginName, branchID, financialGroupID);

            if (token.User.ProjectRoleID != ProjectRoleSet.Admin.ProjectRoleID)
            {
                ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            }
            return RemontinkaServer.Instance.DataStore.GetUsedDeviceItemsReportItems(token.User.UserDomainID, branchID,financialGroupID, beginDate,endDate);
        }


        /// <summary>
        /// Получает отчет по вознаграждениям пользователей за определенный период.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="beginDate">Дата начала.</param>
        /// <param name="endDate">Дата окончания.</param>
        /// <returns>Пункты отчета.</returns>
        public IEnumerable<InterestReportItem> GetUserInterestReportItems(SecurityToken token, DateTime beginDate, DateTime endDate)
        {
            _logger.InfoFormat("Получение отчета по вознаграждениям пользователей для пользователя {0} с {1} по {2}", token.LoginName, beginDate, endDate);

            if (token.User.ProjectRoleID != ProjectRoleSet.Admin.ProjectRoleID)
            {
                ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            }
            return RemontinkaServer.Instance.DataStore.GetUserInterestReportItems(token.User.UserDomainID, beginDate, endDate);
        }

        /// <summary>
        /// Получает пункты отчета по движениям на складе.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="warehouseID">Код склада.</param>
        /// <param name="beginDate">Дата начала.</param>
        /// <param name="endDate">Дата окончания.</param>
        /// <returns>Пункты отчета.</returns>
        public IEnumerable<WarehouseFlowReportItem> GetWarehouseFlowReportItems(SecurityToken token, Guid? warehouseID, DateTime beginDate, DateTime endDate)
        {
            _logger.InfoFormat("Получение отчета по движению для пользователя {0} на складе {1} с {2} по {3}", token.LoginName, warehouseID, beginDate,
                              endDate);
            if (token.User.ProjectRoleID != ProjectRoleSet.Admin.ProjectRoleID)
            {
                ThrowSecurityExceptionOnRoles(token, ProjectRoleSet.Admin);
            }
            return RemontinkaServer.Instance.DataStore.GetWarehouseFlowReportItems(token.User.UserDomainID, warehouseID, beginDate, endDate);
        }

        #endregion Reports

        #region UserGridState

        /// <summary>
        ///   Сохраняет информацию состояние гриде.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="gridName">Имя грида.</param>
        /// <param name="state">Состояние грида.</param>
        public void SaveUserGridState(SecurityToken token, string gridName, string state)
        {
            RemontinkaServer.Instance.DataStore.SaveUserGridState(token.User.UserID, gridName, state);
        }

        /// <summary>
        /// Получает информацию по состоянию грида.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="gridName"></param>
        /// <returns></returns>
        public string GetGridUserState(SecurityToken token, string gridName)
        {
            return RemontinkaServer.Instance.DataStore.GetGridUserState(token.User.UserID, gridName);
        }

        #endregion
        
        #region UserGridFilter

        /// <summary>
        ///   Сохраняет информацию о фильтре грида.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="filter">Фильтр грида.</param>
        public void SaveUserGridFilter(SecurityToken token, UserGridFilter filter)
        {
            _logger.InfoFormat("Сохранение фильтра пользователя {0} {1}", token.LoginName,filter.Title);
            filter.UserID = token.User.UserID;
            RemontinkaServer.Instance.DataStore.SaveUserGridFilter(filter);
        }

        /// <summary>
        /// Получает информацию по состоянию грида.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="userGridFilterID">Код фильтра пользователя.</param>
        /// <returns></returns>
        public UserGridFilter GetUserGridFilter(SecurityToken token, Guid? userGridFilterID)
        {
            _logger.InfoFormat("Получение фильтра пользователем {0} {1}",token.LoginName,userGridFilterID);
            return RemontinkaServer.Instance.DataStore.GetUserGridFilter(token.User.UserID, userGridFilterID);
        }

        /// <summary>
        /// Получает список пользовательских фильтров для определенного грида.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="gridName">Название грида.</param>
        /// <returns>Фильтры грида.</returns>
        public IEnumerable<UserGridFilter> GetUserGridFilters(SecurityToken token, string gridName)
        {
            _logger.InfoFormat("Получение всех фильтров пользователем {0}  для грида {1}", token.LoginName, gridName);
            return RemontinkaServer.Instance.DataStore.GetUserGridFilters(token.User.UserID, gridName);
        }

        #endregion


            #region Misc

            /// <summary>
            /// Выбрасывает исключение по безопасности.
            /// </summary>
            /// <param name="token">Текущий токен пользователя.</param>
            /// <param name="message">Сообщение.</param>
        private void ThrowSecurityException(SecurityToken token, string message)
        {
            _logger.ErrorFormat("Исключение по безопасности для пользователя {0}:{1}",token.LoginName,message);
            throw new SecurityException(message);
        }

        /// <summary>
        /// Выбрасывает исключение по безопасности на необходимость соответвующих прав.
        /// </summary>
        /// <param name="token">Текущий токен пользователя.</param>
        /// <param name="roles">Сообщение.</param>
        private void ThrowSecurityExceptionOnRoles(SecurityToken token,params ProjectRole[] roles)
        {
            var result = new StringBuilder();
            result.Append("Отсутствуют права: ");
            foreach (var projectRole in roles)
            {
                result.AppendFormat(projectRole.Title);
            } //foreach
            ThrowSecurityException(token, result.ToString());
        }

        #endregion Misc



    }
}