using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Remontinka.Server.WebPortal.Models.TransferDocGridForm
{
    /// <summary>
    /// Модель детализации докуумента перемещения.
    /// </summary>
    public class TransferDocGridDetailModel
    {
        /// <summary>
        /// Задает или получает код документа перемещения.
        /// </summary>
        public Guid? TransferDocID { get; set; }
    }
}