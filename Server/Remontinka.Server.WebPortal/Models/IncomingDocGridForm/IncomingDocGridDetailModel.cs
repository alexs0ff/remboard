using System;

namespace Remontinka.Server.WebPortal.Models.IncomingDocGridForm
{
    /// <summary>
    /// Модель детализации приходных документов.
    /// </summary>
    public class IncomingDocGridDetailModel
    {
        /// <summary>
        /// Задает или получает код приходной накладной.
        /// </summary>
        public Guid? IncomingDocID { get; set; }
    }
}