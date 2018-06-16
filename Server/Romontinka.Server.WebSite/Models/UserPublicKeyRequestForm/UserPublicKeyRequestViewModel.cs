using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Models.DataGrid;

namespace Romontinka.Server.WebSite.Models.UserPublicKeyRequestForm
{
    /// <summary>
    /// Модель для отображения списка пользовательских ключей.
    /// </summary>
    public class UserPublicKeyRequestViewModel
    {
        /// <summary>
        /// Задает или получает описатель грида.
        /// </summary>
        public DataGridDescriptor UserPublicKeyRequestsGrid { get; set; }
    }
}