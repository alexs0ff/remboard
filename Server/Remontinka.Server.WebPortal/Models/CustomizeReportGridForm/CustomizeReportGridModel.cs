using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Models.CustomizeReportGridForm
{
    /// <summary>
    /// Модель для грида с документами.
    /// </summary>
    public class CustomizeReportGridModel : GridModelBase
    {
        /// <summary>
        /// Получает наименование ключевого поля.
        /// </summary>
        public override string KeyFieldName { get { return "CustomReportID"; } }

        /// <summary>
        /// Получает список типов документов.
        /// </summary>
        public ICollection<DocumentKind> DocumentKinds { get; set; }
    }
}