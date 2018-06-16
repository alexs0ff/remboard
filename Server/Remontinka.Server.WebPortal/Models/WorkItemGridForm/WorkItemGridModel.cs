using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;

namespace Remontinka.Server.WebPortal.Models.WorkItemGridForm
{
    /// <summary>
    /// Модель грида выполненных работ.
    /// </summary>
    public class WorkItemGridModel: GridModelBase
    {
        /// <summary>
        /// Получает наименование ключевого поля.
        /// </summary>
        public override string KeyFieldName {
            get
            {
                return "WorkItemID";
            } }

        /// <summary>
        /// Задает или получает инжинеров.
        /// </summary>
        public List<SelectListItem<Guid>> Engineers { get; set; }
    }
}