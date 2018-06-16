using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
#if CLIENT
using Remontinka.Client.DataLayer.Entities;
#else
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Helpers;

#endif
#if CLIENT
namespace Remontinka.Client.Core.Services
{
#else
namespace Romontinka.Server.WebSite.Services
{
#endif

    /// <summary>
    /// Менеджер фиксации изменений для сущностей заказов.
    /// </summary>
    public class OrderTimelineManager : IOrderTimelineManager
    {
        /// <summary>
        ///   Текущий логер.
        /// </summary>
        private static readonly ILog _logger = LogManager.GetLogger(typeof(OrderTimelineManager));

        /// <summary>
        /// Создает объект изменения.
        /// </summary>
        /// <param name="orderID">Заказ.</param>
        /// <param name="timelineKind">Тип изменения.</param>
        /// <returns>Созданный объект.</returns>
        private OrderTimeline CreateTimeline(object orderID,TimelineKind timelineKind)
        {
            var timeLine = new OrderTimeline();
#if CLIENT
            if (orderID is string)
            {
                timeLine.RepairOrderID = (string)orderID;    
            } //if
            else
            {
                timeLine.RepairOrderID = FormatUtils.GuidToString((Guid?) orderID);
            } //else
            
            timeLine.EventDateTimeDateTime = DateTime.Now;
#else
            timeLine.RepairOrderID = (Guid?)orderID;
            timeLine.EventDateTime = DateTime.Now;
#endif
            timeLine.TimelineKindID = timelineKind.TimelineKindID;

            return timeLine;
        }

        /// <summary>
        /// Сохраняет пункт графика заказа.
        /// </summary>
        /// <param name="orderTimeline">Пункт графика.</param>
        private void SaveOrderTimeline(OrderTimeline orderTimeline)
        {
#if CLIENT
            ClientCore.Instance.DataStore.SaveOrderTimeline(orderTimeline);
#else
            RemontinkaServer.Instance.DataStore.SaveOrderTimeline(orderTimeline);
#endif
        }

        /// <summary>
        /// Получает пользователя.
        /// </summary>
        /// <param name="token">Токен.</param>
        /// <param name="userID">Код пользователя.</param>
        /// <returns>Пользователь.</returns>
        private User GetUser(SecurityToken token, object userID)
        {
            #if CLIENT
            return ClientCore.Instance.DataStore.GetUser(FormatUtils.StringToGuid((string) userID));
#else
            return RemontinkaServer.Instance.EntitiesFacade.GetUser(token, (Guid?) userID);
#endif
        }

        /// <summary>
        /// Переводит дату в строковое представление.
        /// </summary>
        /// <param name="dateTime">Дата.</param>
        /// <returns>Строковое представление.</returns>
        private string DateTimeToString(object dateTime)
        {
#if CLIENT
            if (dateTime is string)
            {
                var dt = FormatUtils.StringToDateTime((string) dateTime);
                dateTime = dt;
            }
            return ((DateTime)dateTime).ToString("dd.MM.yyyy");
#else
            return Utils.DateTimeToString((DateTime) dateTime);
#endif
        }

        /// <summary>
        /// Переводит число в строковое представление.
        /// </summary>
        /// <param name="value">Число.</param>
        /// <returns>Строковое представление.</returns>
        private string DecimalToString(object value)
        {
#if CLIENT
            return ((double)value).ToString("0.00");
#else
            return Utils.DecimalToString((decimal) value);
#endif
        }

        /// <summary>
        /// Записывает информацию о созданном заказе.
        /// </summary>
        /// <param name="token">Текущие токен.</param>
        /// <param name="order">Созданный заказ.</param>
        public void TrackNewOrder(SecurityToken token, RepairOrder order )
        {
            _logger.InfoFormat("Записываем в историю создание нового заказа {0} пользователем {1}", order.RepairOrderID,
                               token.LoginName);
            var timeLine = CreateTimeline(order.RepairOrderID, TimelineKindSet.StatusChanged);
            
            string engineerName = string.Empty;
            string managerName = string.Empty;

            var user = GetUser(token,order.EngineerID);
            if (user!=null)
            {
                engineerName = user.ToString();
            } //if

            user = GetUser(token,order.ManagerID);
            if (user != null)
            {
                managerName = user.ToString();
            } //if

            timeLine.Title = string.Format("Новый заказ назначен на менеджера \"{0}\" и инженера \"{1}\" пользователем \"{2}\"", managerName,
                                           engineerName,token.User);

            SaveOrderTimeline(timeLine);
        }

        /// <summary>
        /// Производит фиксацию изменений при добавлении выполненных работ.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="workItem">Добавляемая работа.</param>
        public void TrackNewWorkItem(SecurityToken token,WorkItem workItem)
        {
            _logger.InfoFormat("Записываем добавление пункта выполненных работ {0} пользователем {1}",workItem.WorkItemID,token.User);

            var timeLine = CreateTimeline(workItem.RepairOrderID,TimelineKindSet.WorkAdded);
            
            timeLine.Title = string.Format("Пользователь \"{0}\" добавил выполненную работу \"{1}\"", token.User,
                                           workItem.Title);
            SaveOrderTimeline(timeLine);
        }

        /// <summary>
        /// Производит фиксацию изменений между двумя версиями выполненных работ.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="oldWorkItem">Старая версия пункта выполненных работ.</param>
        /// <param name="newWorkItem">Новая версия выполненных работ.</param>
        public void TrackWorkItemChanges(SecurityToken token, WorkItem oldWorkItem, WorkItem newWorkItem)
        {
            _logger.InfoFormat("Записываем изменения в заказе {0} пользователем {1}",oldWorkItem.WorkItemID,token.LoginName);

            CheckFieldsDiff(DateTimeToString(oldWorkItem.EventDate), DateTimeToString(newWorkItem.EventDate),
                           string.Format("Дата работы {0} \"{1}\" был изменена на \"{2}\" пользователем \"{3}\"",
                                         oldWorkItem.Title, DateTimeToString(oldWorkItem.EventDate), DateTimeToString(newWorkItem.EventDate), token.User),
                           TimelineKindSet.StatusChanged, newWorkItem.RepairOrderID);

            CheckFieldsDiff(oldWorkItem.Title, newWorkItem.Title,
                            string.Format("Наименование работы \"{0}\" было изменено на \"{1}\" пользователем \"{2}\"",
                                          oldWorkItem.Title, newWorkItem.Title, token.User),
                            TimelineKindSet.StatusChanged, newWorkItem.RepairOrderID);

            CheckFieldsDiff(oldWorkItem.Price, newWorkItem.Price,
                            string.Format("Стоимость работы \"{0}\" было изменено на \"{1}\" пользователем \"{2}\"",
                                          DecimalToString(oldWorkItem.Price),DecimalToString(newWorkItem.Price), token.User),
                            TimelineKindSet.StatusChanged, newWorkItem.RepairOrderID);
#if CLIENT
#else
            CheckFieldsDiff(oldWorkItem.Notes, newWorkItem.Notes,
                            string.Format("Описание работы было изменено пользователем \"{0}\"",token.User),
                            TimelineKindSet.StatusChanged, newWorkItem.RepairOrderID);
#endif

        }

        /// <summary>
        /// Производит фиксацию изменений между двумя версиями запчастей.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="oldDeviceItem">Старая версия запчасти.</param>
        /// <param name="newDeviceItem">Новая версия запчасти.</param>
        public void TrackDeviceItemChanges(SecurityToken token, DeviceItem oldDeviceItem, DeviceItem newDeviceItem)
        {
            _logger.InfoFormat("Записываем изменения в запчасти {0} пользователем {1}", oldDeviceItem.DeviceItemID, token.LoginName);

            CheckFieldsDiff(DateTimeToString(oldDeviceItem.EventDate), DateTimeToString(newDeviceItem.EventDate),
                           string.Format("Дата запчасти {0} \"{1}\" был изменена на \"{2}\" пользователем \"{3}\"",
                                         oldDeviceItem.Title, DateTimeToString(oldDeviceItem.EventDate), DateTimeToString(newDeviceItem.EventDate), token.User),
                           TimelineKindSet.StatusChanged, newDeviceItem.RepairOrderID);

            CheckFieldsDiff(oldDeviceItem.Title, newDeviceItem.Title,
                            string.Format("Наименование запчасти \"{0}\" было изменено на \"{1}\" пользователем \"{2}\"",
                                          oldDeviceItem.Title, newDeviceItem.Title, token.User),
                            TimelineKindSet.StatusChanged, newDeviceItem.RepairOrderID);

            CheckFieldsDiff(oldDeviceItem.Price, newDeviceItem.Price,
                            string.Format("Стоимость запчасти \"{0}\" было изменено на \"{1}\" пользователем \"{2}\"",
                                          DecimalToString(oldDeviceItem.Price), DecimalToString(newDeviceItem.Price), token.User),
                            TimelineKindSet.StatusChanged, newDeviceItem.RepairOrderID);

            CheckFieldsDiff(oldDeviceItem.CostPrice, newDeviceItem.CostPrice,
                            string.Format("Себестоимость запчасти \"{0}\" было изменено на \"{1}\" пользователем \"{2}\"",
                                          DecimalToString(oldDeviceItem.CostPrice), DecimalToString(newDeviceItem.CostPrice), token.User),
                            TimelineKindSet.StatusChanged, newDeviceItem.RepairOrderID);

            CheckFieldsDiff(oldDeviceItem.Count, newDeviceItem.Count,
                            string.Format("Количество запчастей \"{0}\" было изменено на \"{1}\" пользователем \"{2}\"",
                                          oldDeviceItem.Count, newDeviceItem.Count, token.User),
                            TimelineKindSet.StatusChanged, newDeviceItem.RepairOrderID);
        }

        /// <summary>
        /// Производит фиксацию удаления работы из заказа.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="workItemID">Код проделанной работы.</param>
        public void TrackWorkItemDelete(SecurityToken token, Guid? workItemID)
        {
            _logger.InfoFormat("Записываем удаление работы из заказа {0} пользователем {1}",workItemID,token.LoginName);

#if CLIENT
            var workItem = ClientCore.Instance.DataStore.GetWorkItem(workItemID);
#else
            var workItem = RemontinkaServer.Instance.EntitiesFacade.GetWorkItem(token, workItemID);
#endif

            if (workItem != null)
            {
                var timeLine = CreateTimeline(workItem.RepairOrderID,TimelineKindSet.StatusChanged);
                
                timeLine.Title = string.Format("Пользователь \"{0}\" удалил работу \"{1}\"", token.User,
                                               workItem.Title);
                SaveOrderTimeline(timeLine);
            } //if
        }

        /// <summary>
        /// Производит фиксацию удаления запчасти из заказа.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="deviceItemID">Код запчасти.</param>
        public void TrackDeviceItemDelete(SecurityToken token, Guid? deviceItemID)
        {
            _logger.InfoFormat("Записываем удаление запчасти из заказа {0} пользователем {1}", deviceItemID, token.LoginName);
#if CLIENT
            var deviceItem = ClientCore.Instance.DataStore.GetDeviceItem(deviceItemID);
#else
            var deviceItem = RemontinkaServer.Instance.EntitiesFacade.GetDeviceItem(token, deviceItemID);
#endif
            if (deviceItem != null)
            {
                var timeLine =CreateTimeline(deviceItem.RepairOrderID,TimelineKindSet.StatusChanged);
                timeLine.Title = string.Format("Пользователь \"{0}\" удалил запчасть \"{1}\"", token.User,
                                               deviceItem.Title);
                SaveOrderTimeline(timeLine);
            } //if
        }

        /// <summary>
        /// Производит фиксацию изменений при добавлении запчастей.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="deviceItem">Добавляемая запчасть.</param>
        public void TrackNewDeviceItem(SecurityToken token, DeviceItem deviceItem)
        {
            _logger.InfoFormat("Записываем добавление запчати {0} пользователем {1}", deviceItem.DeviceItemID, token.User);

            var timeLine = CreateTimeline(deviceItem.RepairOrderID,TimelineKindSet.DeviceItemAdded);
            
            timeLine.Title = string.Format("Пользователь \"{0}\" добавил запчасть \"{1}\"", token.User,
                                           deviceItem.Title);
            SaveOrderTimeline(timeLine);
        }

        /// <summary>
        /// Добавляет комментарий в заказ.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="repairOrderID">Код заказа.</param>
        /// <param name="text">Текст комментария.</param>
        public void AddNewComment(SecurityToken token,Guid? repairOrderID, string text)
        {
            _logger.InfoFormat("Записываем добавление комментария для заказа {0} пользователем {1}", repairOrderID, token.User);

            var timeLine = CreateTimeline(repairOrderID,TimelineKindSet.CommentAdded);
            
            timeLine.Title = string.Format("Комментарий от \"{0}\": \"{1}\"", token.User,
                                           text);
            SaveOrderTimeline(timeLine);
        }

        /// <summary>
        /// Производит фиксацию изменений между двумя версиями заказов.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="oldOrder">Старый заказ.</param>
        /// <param name="newOrder">Обновленный заказ.</param>
        public void TrackOrderChange(SecurityToken token, RepairOrder oldOrder, RepairOrder newOrder)
        {
            _logger.InfoFormat("Записываем изменения заказа {0} пользователем {1}",oldOrder.RepairOrderID,token.LoginName);

            if (oldOrder.BranchID!=newOrder.BranchID)
            {
                var timeLine = CreateTimeline(oldOrder.RepairOrderID,TimelineKindSet.StatusChanged);
                
#if CLIENT
                var oldBranch = ClientCore.Instance.DataStore.GetBranch(oldOrder.BranchIDGuid);
                var newBranch = ClientCore.Instance.DataStore.GetBranch(newOrder.BranchIDGuid);
#else
                var oldBranch = RemontinkaServer.Instance.EntitiesFacade.GetBranch(token,oldOrder.BranchID);
                var newBranch = RemontinkaServer.Instance.EntitiesFacade.GetBranch(token,newOrder.BranchID);
#endif


                timeLine.Title = string.Format("У заказа изменен филиал с \"{0}\" на \"{1}\" пользователем \"{2}\"",
                                               oldBranch.Title, newBranch.Title, token.User);
                SaveOrderTimeline(timeLine);
            } //if

            if (oldOrder.ManagerID!=newOrder.ManagerID)
            {
                var timeLine = CreateTimeline(oldOrder.RepairOrderID,TimelineKindSet.ManagerAssigned);
                
                var oldManager = GetUser(token,oldOrder.ManagerID);
                var newManager = GetUser(token, newOrder.ManagerID);
                timeLine.Title = string.Format("Изменен менеджер с \"{0}\" на \"{1}\" пользователем \"{2}\"",
                                               oldManager, newManager, token.User);
                SaveOrderTimeline(timeLine);
            } //if

            if (oldOrder.EngineerID != newOrder.EngineerID)
            {
                var timeLine = CreateTimeline(oldOrder.RepairOrderID,TimelineKindSet.EngineerAssigned);
                

                var oldEngineer = GetUser(token,oldOrder.EngineerID);
                var newEngineer = GetUser(token,newOrder.EngineerID);
                
                var oldFullName = string.Empty;
                if (oldEngineer!=null)
                {
                    oldFullName = oldEngineer.ToString();
                } //if

                var newFullName = string.Empty;

                if (newEngineer != null)
                {
                    newFullName = newEngineer.ToString();
                } //if

                timeLine.Title = string.Format("Изменен инженер с \"{0}\" на \"{1}\" пользователем \"{2}\"",
                                               oldFullName, newFullName, token.User);
                SaveOrderTimeline(timeLine);
            } //if

            if (oldOrder.OrderStatusID != newOrder.OrderStatusID)
            {
                var timeLine = CreateTimeline(oldOrder.RepairOrderID,TimelineKindSet.StatusChanged);
                
#if CLIENT
                var oldStatus = ClientCore.Instance.DataStore.GetOrderStatus(oldOrder.OrderStatusIDGuid);
                var newStatus = ClientCore.Instance.DataStore.GetOrderStatus(newOrder.OrderStatusIDGuid);
#else
                var oldStatus = RemontinkaServer.Instance.EntitiesFacade.GetOrderStatus(token,oldOrder.OrderStatusID);
                var newStatus = RemontinkaServer.Instance.EntitiesFacade.GetOrderStatus(token, newOrder.OrderStatusID);
#endif
                
                timeLine.Title = string.Format("Изменен статус с \"{0}\" на \"{1}\" пользователем \"{2}\"",
                                               oldStatus.Title, newStatus.Title, token.User);

                SaveOrderTimeline(timeLine);
            } //if

            if (oldOrder.IssueDate ==null && newOrder.IssueDate!=null)
            {
                var timeLine = CreateTimeline(oldOrder.RepairOrderID,TimelineKindSet.Completed);
                

                timeLine.Title = string.Format("Товар был выдан \"{0}\" пользователем \"{1}\"",
                    DateTimeToString(newOrder.IssueDate), token.User);

                SaveOrderTimeline(timeLine);
            } //if
            if (oldOrder.IssueDate != null)
            {

#if CLIENT
                var oldIssueDate = oldOrder.IssueDate;
                var newIssueDate = newOrder.IssueDate;
#else
            var oldIssueDate = oldOrder.IssueDate.Value;
            var newIssueDate = DateTime.MinValue;
            if (newOrder.IssueDate != null)
            {
                newIssueDate = newOrder.IssueDate.Value;
            } //if
#endif

                if (newOrder.IssueDate != null && oldIssueDate != newIssueDate)
                {
                    var timeLine = CreateTimeline(oldOrder.RepairOrderID, TimelineKindSet.Completed);


                    timeLine.Title = string.Format(
                        "Дата выдачи товара была перенесена на \"{0}\" пользователем \"{1}\"",
                        DateTimeToString(newOrder.IssueDate), token.User);

                    SaveOrderTimeline(timeLine);
                } //if
            }

            if (oldOrder.DateOfBeReady != newOrder.DateOfBeReady)
            {
                var timeLine = CreateTimeline(oldOrder.RepairOrderID,TimelineKindSet.StatusChanged);
                

                timeLine.Title = string.Format("Дата предполагаемой готовности товара была перенесена на \"{0}\" пользователем \"{1}\"",
                    DateTimeToString(newOrder.DateOfBeReady), token.User);

                SaveOrderTimeline(timeLine);
            } //if

            CheckFieldsDiff(oldOrder.ClientFullName, newOrder.ClientFullName,
                            string.Format("ФИО клиента \"{0}\" было изменено на \"{1}\" пользователем \"{2}\"",
                                          oldOrder.ClientFullName, newOrder.ClientFullName, token.User),
                            TimelineKindSet.StatusChanged, newOrder.RepairOrderID);
            CheckFieldsDiff(oldOrder.ClientPhone, newOrder.ClientPhone,
                            string.Format("Телефон клиента \"{0}\" был изменен на \"{1}\" пользователем \"{2}\"",
                                          oldOrder.ClientPhone, newOrder.ClientPhone, token.User),
                            TimelineKindSet.StatusChanged, newOrder.RepairOrderID);

            CheckFieldsDiff(oldOrder.ClientEmail, newOrder.ClientEmail,
                            string.Format("Email клиента \"{0}\" был изменен на \"{1}\" пользователем \"{2}\"",
                                          oldOrder.ClientEmail, newOrder.ClientEmail, token.User),
                            TimelineKindSet.StatusChanged, newOrder.RepairOrderID);

            CheckFieldsDiff(oldOrder.PrePayment, newOrder.PrePayment,
                            string.Format("Аванс клиента \"{0}\" был изменен на \"{1}\" пользователем \"{2}\"",
                                          oldOrder.PrePayment, newOrder.PrePayment, token.User),
                            TimelineKindSet.StatusChanged, newOrder.RepairOrderID);

            CheckFieldsDiff(oldOrder.GuidePrice, newOrder.GuidePrice,
                            string.Format("Предварительная цена \"{0}\" была изменена на \"{1}\" пользователем \"{2}\"",
                                          oldOrder.GuidePrice, newOrder.GuidePrice, token.User),
                            TimelineKindSet.StatusChanged, newOrder.RepairOrderID);
        }

        /// <summary>
        /// Проверяет разность значений полей и записывает в историю определенные данные, если они были изменены.
        /// </summary>
        /// <param name="value1">Первое значение.</param>
        /// <param name="value2">Второе значение.</param>
        /// <param name="messageToTrack">Сообщение для записи.</param>
        /// <param name="timelineKind">Тип сообщения.</param>
        /// <param name="repairOrderID">Код привязываемого заказа.</param>
        private void CheckFieldsDiff(string value1, string value2, string messageToTrack,TimelineKind timelineKind,object repairOrderID)
        {
            if (!StringComparer.OrdinalIgnoreCase.Equals(value1,value2))
            {
                SaveOrderTimeline(messageToTrack, timelineKind, repairOrderID);
            } //if
        }

        /// <summary>
        /// Проверяет разность значений полей и записывает в историю определенные данные, если они были изменены.
        /// </summary>
        /// <param name="value1">Первое значение.</param>
        /// <param name="value2">Второе значение.</param>
        /// <param name="messageToTrack">Сообщение для записи.</param>
        /// <param name="timelineKind">Тип сообщения.</param>
        /// <param name="repairOrderID">Код привязываемого заказа.</param>
        private void CheckFieldsDiff(decimal? value1, decimal? value2, string messageToTrack, TimelineKind timelineKind, object repairOrderID)
        {
            if (value1!=value2)
            {
                SaveOrderTimeline(messageToTrack, timelineKind, repairOrderID);
            } //if
        }

        /// <summary>
        /// Проверяет разность значений полей и записывает в историю определенные данные, если они были изменены.
        /// </summary>
        /// <param name="value1">Первое значение.</param>
        /// <param name="value2">Второе значение.</param>
        /// <param name="messageToTrack">Сообщение для записи.</param>
        /// <param name="timelineKind">Тип сообщения.</param>
        /// <param name="repairOrderID">Код привязываемого заказа.</param>
        private void CheckFieldsDiff(double? value1, double? value2, string messageToTrack, TimelineKind timelineKind, object repairOrderID)
        {
            if (value1 != value2)
            {
                SaveOrderTimeline(messageToTrack, timelineKind, repairOrderID);
            } //if
        }

        /// <summary>
        /// Сохраняет сообщение о изменении свойств заказа.
        /// </summary>
        /// <param name="messageToTrack">Сообщение для сохранения.</param>
        /// <param name="timelineKind">Тип записи.</param>
        /// <param name="repairOrderID">Код заказа.</param>
        private void SaveOrderTimeline(string messageToTrack, TimelineKind timelineKind, object repairOrderID)
        {
            var timeLine = CreateTimeline(repairOrderID,timelineKind);
            
            timeLine.Title = messageToTrack;

            SaveOrderTimeline(timeLine);
        }
    }
}