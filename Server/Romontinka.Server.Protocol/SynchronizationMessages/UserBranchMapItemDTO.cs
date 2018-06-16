using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol.SynchronizationMessages
{
    /// <summary>
    /// DTO объекты связей филиалов и пользователей.
    /// </summary>
    public class UserBranchMapItemDTO
    {
        /// <summary>
        /// Задает или получает код соответствия.
        /// </summary>
        public Guid? UserBranchMapID { get; set; }

        /// <summary>
        /// Задает или получает код связанного филиала.
        /// </summary>
        public Guid? BranchID { get; set; }

        /// <summary>
        /// Задает или получает дату соответствия.
        /// </summary>
        public DateTime? EventDate { get; set; }

        /// <summary>
        /// Задает или получает код пользователя.
        /// </summary>
        public Guid? UserID { get; set; }
    }
}
