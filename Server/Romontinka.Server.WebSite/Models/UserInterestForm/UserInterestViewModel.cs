using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Models.DataGrid;

namespace Romontinka.Server.WebSite.Models.UserInterestForm
{
    /// <summary>
    /// Модель отображения списка вознаграждений.
    /// </summary>
    public class UserInterestViewModel
    {
        /// <summary>
        /// Задает или получает описатель грида.
        /// </summary>
        public DataGridDescriptor UserInterestsGrid { get; set; }
    }
}