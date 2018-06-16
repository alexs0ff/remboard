using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remontinka.Client.Core.Models
{
    /// <summary>
    /// Описатель для дескриптора модели.
    /// </summary>
    public class SyncModelDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public SyncModelDescriptor()
        {
            Items = new Dictionary<SyncItemModelKind, SyncItemContainer>();
        }

        /// <summary>
        /// Получает пункты для обновлений.
        /// </summary>
        public IDictionary<SyncItemModelKind, SyncItemContainer> Items { get; set; }

        /// <summary>
        /// Задает или получает модель обновления.
        /// </summary>
        public object Model { get; set; }
    }
}
