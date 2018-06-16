using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Metadata;

namespace Romontinka.Server.WebSite.Models.RepairOrderForm
{
    /// <summary>
    /// Модель для добавления комментариев к заказу.
    /// </summary>
    public class RepairOrderCommentModel
    {
        /// <summary>
        /// Задает или получает код заказа.
        /// </summary>
        public Guid? CommentRepairOrderID { get; set; }

        /// <summary>
        /// Задает или получает текст комментария к заказу.
        /// </summary>
        public string CommentText { get; set; }
    }
}