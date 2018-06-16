using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Romontinka.Server.WebSite.Models.Dialog
{
    /// <summary>
    /// Модель для диалогов редактирования.
    /// </summary>
    public class EditDialogModel
    {
        /// <summary>
        /// Задает или получает код диалога.
        /// </summary>
        public string DialogId { get; set; }

        /// <summary>
        /// Задает или получает ID формы.
        /// </summary>
        public string FormId { get; set; }

        /// <summary>
        /// Задает или получает заголовок диалога.
        /// </summary>
        public string Title { get; set; }

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

        /// <summary>
        /// Задает или получает флаг указывающий на то, что необходимо развернуть окно на весь экран.
        /// </summary>
        public bool FullScreen { get; set; }

        /// <summary>
        /// Задает или получает высоту диалога в пикселях.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Задает или получает ширину диалога в пикселях.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Задает или получает флаг указывающий, что у диалога не нужно отображать заголовок.
        /// </summary>
        public bool NoDialogTitle { get; set; }
    }
}