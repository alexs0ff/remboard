using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Models.DataGrid;

namespace Romontinka.Server.WebSite.Models.UserPublicKeyForm
{
    /// <summary>
    /// Модель для отображения списка ключей.
    /// </summary>
    public class UserPublicKeyViewModel
    {
        /// <summary>
        /// Задает или получает описатель грида.
        /// </summary>
        public DataGridDescriptor UserPublicKeysGrid { get; set; }
    }
}