using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevExpress.Web.Mvc;
using Romontinka.Server.Core.Security;

namespace Remontinka.Server.WebPortal.Models
{
    /// <summary>
    /// Базовая модель для гридов.
    /// </summary>
    public abstract class GridModelBase
    {
        /// <summary>
        /// Задает или получает наименование грида.
        /// </summary>
        public string GridName { get; set; }

        /// <summary>
        /// Задает или получает имя контроллера.
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// Задает или получает имя объекта для редактирования.
        /// </summary>
        public string GridObjectName { get; set; }

        /// <summary>
        /// Задает или получает модель данных.
        /// </summary>
        public IQueryable Model { get; set; }

        /// <summary>
        /// Получает наименование ключевого поля.
        /// </summary>
        public abstract string KeyFieldName { get; }

        /// <summary>
        /// Задает или получает токен безопасности.
        /// </summary>
        public SecurityToken Token { get; set; }

        /// <summary>
        /// Задает или получает код родительской записи.
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// Задает или получает свойство сигнализации о процессе редактирования.
        /// Null - Не радактирование и не добавление.
        /// False - добавление.
        /// </summary>
        public bool? IsEditing { get; set; }
    }
}