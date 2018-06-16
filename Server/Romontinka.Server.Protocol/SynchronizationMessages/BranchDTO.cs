using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol.SynchronizationMessages
{
    /// <summary>
    /// DTO объект филиала.
    /// </summary>
    public class BranchDTO
    {
        /// <summary>
        /// Задает или получает код филиала.
        /// </summary>
        public Guid? BranchID { get; set; }

        /// <summary>
        /// Задает или получает название.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает адрес филиала.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Задает или получает юр название филиала.
        /// </summary>
        public string LegalName { get; set; }
    }
}
