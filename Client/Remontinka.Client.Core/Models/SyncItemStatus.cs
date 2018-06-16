using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remontinka.Client.Core.Models
{
    /// <summary>
    /// Перечесление статусов обработку пункта синхронизации.
    /// </summary>
    public enum SyncItemStatus
    {
        /// <summary>
        /// Подготовка
        /// </summary>
        Preparing,

        /// <summary>
        /// Обработка.
        /// </summary>
        Processing,

        /// <summary>
        /// Успех.
        /// </summary>
        Success,

        /// <summary>
        /// Неудача
        /// </summary>
        Failed
    }
}
