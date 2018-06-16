using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Models.DataGrid;

namespace Romontinka.Server.WebSite.Models.User
{
    /// <summary>
    /// Модель для управления пользователями.
    /// </summary>
    public class UserViewModel
    {
        /// <summary>
        /// Задает или получает описатель грида для пользователей.
        /// </summary>
        public DataGridDescriptor UsersGrid { get; set; }
    }
}