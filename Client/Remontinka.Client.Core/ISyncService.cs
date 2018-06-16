using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Remontinka.Client.Core.Models;

namespace Remontinka.Client.Core
{
    /// <summary>
    /// Интерфейс синхронизации данных.
    /// </summary>
    public interface ISyncService
    {
        /// <summary>
        /// Событие изменения статуса.
        /// </summary>
        event EventHandler<SyncItemStatusChangedEventArgs> SyncItemStatusChangedEvent;

        /// <summary>
        /// Происходит по завершению обработки процесса синхронизации.
        /// </summary>
        event EventHandler<SyncProcessFinishedEventArgs> SyncProcessFinished;

        /// <summary>
        /// Происходит в момент наличия информации об ошибки.
        /// </summary>
        event EventHandler<ErrorEventArgs> Error;

        /// <summary>
        /// Происходит в момент наличия дополнительной информации.
        /// </summary>
        event EventHandler<InfoEventArgs> Info;

        /// <summary>
        /// Происходит во время смены описания для пункта синхронизации.
        /// </summary>
        event EventHandler<SyncItemDescriptionChangedEventArgs> SyncItemDescriptionChanged;

        /// <summary>
        /// Получает текущую модель.
        /// </summary>
        SyncModelDescriptor CurrentModel { get; }

        /// <summary>
        /// Стартует процесс синхронизации.
        /// </summary>
        void StartProcess(SyncModelDescriptor model);
    }
}
