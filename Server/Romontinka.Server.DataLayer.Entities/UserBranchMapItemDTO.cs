using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// ДТО объект для соответствия пользователей и филиалов.
    /// </summary>
    public class UserBranchMapItemDTO : UserBranchMapItem
    {
        /// <summary>
        /// Задает или получает название филиала.
        /// </summary>
        public string BranchTitle { get; set; }

        /// <summary>
        /// Задает или получает логин пользователя.
        /// </summary>
        public string UserLogin { get; set; }

        /// <summary>
        /// Задает или получает имя пользователя.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Задает или получает фамилию пользователя.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Задает или получает отчество пользвателя.
        /// </summary>
        public string MiddleName { get; set; }
    }
}
