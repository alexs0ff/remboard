using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.TransferDocForm
{
    /// <summary>
    /// Модель пункта грида по перемещению между складами.
    /// </summary>
    public class TransferDocGridItemModel : JGridItemModel<Guid>
    {
        /// <summary>
        /// Задает или получает название склада откуда перемещается товар.
        /// </summary>
        public string SenderWarehouseTitle { get; set; }

        /// <summary>
        /// Задает или получает название получаещего склада.
        /// </summary>
        public string RecipientWarehouseTitle { get; set; }

        /// <summary>
        /// Задает или получает ФИО создателя.
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// Задает или получает номер документа.
        /// </summary>
        public string DocNumber { get; set; }

        /// <summary>
        /// Задает или получает дату документа.
        /// </summary>
        public string DocDate { get; set; }

        /// <summary>
        /// Задает или получает описание документа.
        /// </summary>
        public string DocDescription { get; set; }
    }
}