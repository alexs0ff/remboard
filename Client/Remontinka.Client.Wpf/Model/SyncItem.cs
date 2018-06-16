using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Remontinka.Client.Core.Interception;
using Remontinka.Client.Core.Models;

namespace Remontinka.Client.Wpf.Model
{
    /// <summary>
    /// Пункт процесса синхронизации.
    /// </summary>
    public class SyncItem : BindableModelObject
    {
        /// <summary>
        /// Задает или получает название пункта.
        /// </summary>
        [NotifyPropertyChanged]
        public virtual string Title { get; set; }

        /// <summary>
        /// Задает или получает описание процесса.
        /// </summary>
        [NotifyPropertyChanged]
        public virtual string Description { get; set; }

        /// <summary>
        /// Задает или получает обработку статуса.
        /// </summary>
        [NotifyPropertyChanged]
        public virtual SyncItemStatus Status { get; set; }

    }
}
