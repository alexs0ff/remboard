using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Romontinka.Server.WebSite.Common
{
    /// <summary>
    /// Результат обработки документа.
    /// </summary>
    public class JCrudBooleanDataResult : JCrudResult
    {
        /// <summary>
        /// Задавет или получает булевское значение результата.
        /// </summary>
        public bool Data { get; set; }
    }
}