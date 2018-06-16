using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Romontinka.Server.WebSite.Common
{
    /// <summary>
    /// Результат выполнения с данными.
    /// </summary>
    public class JCrudDataResult : JCrudResult
    {
        /// <summary>
        /// Задает или получает флаг указывающий на необходимость перегрузки модели.
        /// </summary>
        public bool NeedReloadModel { get; set; }

        /// <summary>
        /// Задает или получает данные.
        /// </summary>
        public string Data { get; set; }
    }
}