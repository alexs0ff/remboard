using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Models.UserInterestGridForm
{
    /// <summary>
    /// Модель грида по вознаграждениям для пользователей.
    /// </summary>
    public class UserInterestGridModel : GridModelBase
    {
        /// <summary>
        /// Получает наименование ключевого поля.
        /// </summary>
        public override string KeyFieldName { get { return "UserInterestID"; } }

        /// <summary>
        /// Задает или получает пользователей.
        /// </summary>
        public List<SelectListItem<Guid>> Users { get; set; }

        /// <summary>
        /// Получает типы вознаграждения.
        /// </summary>
        public IEnumerable<InterestKind> InterestKinds { get { return InterestKindSet.Kinds; } }
    }
}