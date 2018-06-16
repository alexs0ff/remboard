using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Models.OrderTimelineGridForm
{
    /// <summary>
    /// Модель грида для истории.
    /// </summary>
    public class OrderTimelineGridModel : GridModelBase
    {
        /// <summary>
        /// Получает наименование ключевого поля.
        /// </summary>
        public override string KeyFieldName { get { return "OrderTimelineID"; } }

        /// <summary>
        /// Получает список типов истории заказов.
        /// </summary>
        public ICollection<TimelineKind> TimelineKinds { get { return TimelineKindSet.Kinds; } }
    }
}