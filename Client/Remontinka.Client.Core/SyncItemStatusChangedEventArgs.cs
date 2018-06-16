using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Remontinka.Client.Core.Models;

namespace Remontinka.Client.Core
{
    /// <summary>
    /// Агументы для события смены статуса.
    /// </summary>
    public class SyncItemStatusChangedEventArgs:EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public SyncItemStatusChangedEventArgs(SyncItemStatus newStatus, SyncItemContainer container)
        {
            NewStatus = newStatus;
            ItemContainer = container;
        }

        /// <summary>
        /// Получает новый статус для обрабатываемого метода.
        /// </summary>
        public SyncItemStatus NewStatus { get; private set; }

        /// <summary>
        /// Получает пункт, статус которого был обновлен.
        /// </summary>
        public SyncItemContainer ItemContainer { get; private set; }
    }
}
