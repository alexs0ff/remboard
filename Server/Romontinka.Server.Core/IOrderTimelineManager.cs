using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if CLIENT
using Remontinka.Client.DataLayer.Entities;

#else
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;
#endif

#if CLIENT
namespace Remontinka.Client.Core
{
#else
namespace Romontinka.Server.Core
{
#endif

    /// <summary>
    /// Интерфейс менеджера пунктов истории работы с ордером.
    /// </summary>
    public interface IOrderTimelineManager
    {
        /// <summary>
        /// Записывает информацию о созданном заказе.
        /// </summary>
        /// <param name="token">Текущие токен.</param>
        /// <param name="order">Созданный заказ.</param>
        void TrackNewOrder(SecurityToken token, RepairOrder order );

        /// <summary>
        /// Производит фиксацию изменений при добавлении выполненных работ.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="workItem">Добавляемая работа.</param>
        void TrackNewWorkItem(SecurityToken token,WorkItem workItem);

        /// <summary>
        /// Производит фиксацию изменений при добавлении запчастей.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="deviceItem">Добавляемая запчасть.</param>
        void TrackNewDeviceItem(SecurityToken token, DeviceItem deviceItem);

        /// <summary>
        /// Производит фиксацию изменений между двумя версиями заказов.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="oldOrder">Старый заказ.</param>
        /// <param name="newOrder">Обновленный заказ.</param>
        void TrackOrderChange(SecurityToken token, RepairOrder oldOrder, RepairOrder newOrder);

        /// <summary>
        /// Добавляет комментарий в заказ.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="repairOrderID">Код заказа.</param>
        /// <param name="text">Текст комментария.</param>
        void AddNewComment(SecurityToken token,Guid? repairOrderID, string text);

        /// <summary>
        /// Производит фиксацию изменений между двумя версиями выполненных работ.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="oldWorkItem">Старая версия пункта выполненных работ.</param>
        /// <param name="newWorkItem">Новая версия выполненных работ.</param>
        void TrackWorkItemChanges(SecurityToken token, WorkItem oldWorkItem, WorkItem newWorkItem);

        /// <summary>
        /// Производит фиксацию изменений между двумя версиями запчастей.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="oldDeviceItem">Старая версия запчасти.</param>
        /// <param name="newDeviceItem">Новая версия запчасти.</param>
        void TrackDeviceItemChanges(SecurityToken token, DeviceItem oldDeviceItem, DeviceItem newDeviceItem);

        /// <summary>
        /// Производит фиксацию удаления работы из заказа.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="workItemID">Код проделанной работы.</param>
        void TrackWorkItemDelete(SecurityToken token, Guid? workItemID);

        /// <summary>
        /// Производит фиксацию удаления запчасти из заказа.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="deviceItemID">Код запчасти.</param>
        void TrackDeviceItemDelete(SecurityToken token, Guid? deviceItemID);
    }
}
