using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Remontinka.Server.WebPortal.Models.CancellationDocGridForm
{
    /// <summary>
    /// Модель детализации документов списания.
    /// </summary>
    public class CancellationDocGridDetailModel
    {
        /// <summary>
        /// Задает или получает код документа списания.
        /// </summary>
        public Guid? CancellationDocID { get; set; }
    }
}