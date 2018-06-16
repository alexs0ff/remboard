using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol.SynchronizationMessages
{
    /// <summary>
    /// DTO объект для категории товара.
    /// </summary>
    public class ItemCategoryDTO
    {
        /// <summary>
        /// Задает или получает код категории.
        /// </summary>
        public Guid? ItemCategoryID { get; set; }

        /// <summary>
        /// Задает или название категории.
        /// </summary>
        public string Title { get; set; }
    }
}
