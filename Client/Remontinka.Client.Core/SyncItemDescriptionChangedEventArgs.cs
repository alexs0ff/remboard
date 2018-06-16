using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Remontinka.Client.Core.Models;

namespace Remontinka.Client.Core
{
    /// <summary>
    /// Аргументы для события смены описания.
    /// </summary>
    public class SyncItemDescriptionChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.EventArgs"/> class.
        /// </summary>
        public SyncItemDescriptionChangedEventArgs(string newDescription, SyncItemContainer itemContainer)
        {
            NewDescription = newDescription;
            ItemContainer = itemContainer;
        }

        /// <summary>
        /// Получает новый описание для обрабатываемого метода.
        /// </summary>
        public string NewDescription { get; private set; }

        /// <summary>
        /// Получает пункт, статус которого был обновлен.
        /// </summary>
        public SyncItemContainer ItemContainer { get; private set; }
    }
}
