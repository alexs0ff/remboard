using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Remontinka.Server.WebPortal.Models.Common
{
    /// <summary>
    /// Базовая модель для контроллера.
    /// </summary>
    public abstract class GridLookupModelBase
    {
        /// <summary>
        /// Задает или получает имя контроллера.
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// Задает или получает имя действия.
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// Задает или получает данные.
        /// </summary>
        public IQueryable Data { get; set; }

        /// <summary>
        /// Задает или получает имя связанного поля.
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Задает или получает наименование ключевого поля.
        /// </summary>
        public string KeyField { get; set; }

        /// <summary>
        /// Задает или получает значение для биндинга.
        /// </summary>
        public object Value { get; set; }
    }
}