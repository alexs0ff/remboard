using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Romontinka.Server.WebSite.Models.DataGrid
{
    /// <summary>
    /// Представляет колонку с контролом удаления данных
    /// </summary>
    public class DeleteButtonGridColumn
    {
        /// <summary>
        /// Задает или получает текст вопроса.
        /// </summary>
        public string QuestionText { get; set; }

        /// <summary>
        /// Задает или получает код данных, которые будут присоеденены к вопросу в виде "[данные]".
        /// </summary>
        public string DataId { get; set; }

        /// <summary>
        /// Задает или получает подсказку к кнопке.
        /// </summary>
        public string ToolTip { get; set; }
    }
}