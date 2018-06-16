using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;

namespace Remontinka.Server.WebPortal.Models.Common
{
    /// <summary>
    /// Модель для настройки редактирования грида.
    /// </summary>
    public class GridEditSettingModel<TKey,TGridModel, TGridDataModel>
        where TKey:struct 
        where TGridModel : GridModelBase
        where TGridDataModel: GridDataModelBase<TKey>
    {
        /// <summary>
        /// Задает или получает модель данных.
        /// </summary>
        public TGridDataModel Model { get; set; }

        /// <summary>
        /// Задает или получает настройки лайаута.
        /// </summary>
        public FormLayoutSettings<TGridModel> LayoutSettings { get; set; }

        /// <summary>
        /// Задает или получает настройки грида.
        /// </summary>
        public TGridModel GridSettings { get; set; }

        /// <summary>
        /// Задает или получает текущий html хэлпер для записи данных.
        /// </summary>
        public HtmlHelper<TGridModel> Html { get; set; }
    }
}