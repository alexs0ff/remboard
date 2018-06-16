using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Romontinka.Server.WebSite.Models.DataGrid
{
    /// <summary>
    /// Представляет описатель кнопки с кнопками редактирования.
    /// </summary>
    public class EditButtonGridColumn
    {
        /// <summary>
        /// Задает или получает флаг указывающий, что диалог нужно показывать на весь экран.
        /// </summary>
        public bool FullScreen { get; set; }

        /// <summary>
        /// Задает или получает подсказку к кнопке.
        /// </summary>
        public string ToolTip { get; set; }

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