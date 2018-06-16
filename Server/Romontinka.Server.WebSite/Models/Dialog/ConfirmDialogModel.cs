using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Romontinka.Server.WebSite.Models.Dialog
{
    /// <summary>
    /// Модель для диалога подтверждения.
    /// </summary>
    public class ConfirmDialogModel
    {
        /// <summary>
        /// Задает или получает код диалога.
        /// </summary>
        public string DialogId { get; set; }

        /// <summary>
        /// Задает или получает заголовок диалога.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает данные для контента.
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Задает или получает код элемента с данными.
        /// </summary>
        public string DataElementId { get; set; }

        /// <summary>
        /// Задает или получает имя функции которая вызывается в случае успеха.
        /// </summary>
        public string SuccessFunctionName { get; set; }

        /// <summary>
        /// Задает или получает текст кнопки "OK".
        /// </summary>
        public string OkButtonText { get; set; }

        /// <summary>
        /// Задает или получает текст кнопки "Отмена".
        /// </summary>
        public string CancelButtonText { get; set; }
    }
}