using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevExpress.Utils;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using Romontinka.Server.Core.Security;

namespace Remontinka.Server.WebPortal.Helpers
{
    /// <summary>
    /// Хедпер для настройки колонок грида.
    /// </summary>
    public static class GridViewColumnHelper
    {

        /// <summary>
        /// Устанавливает настройки для колонки с периодом.
        /// </summary>
        /// <param name="column">Колонка.</param>
        /// <param name="token">Токен безопасности.</param>
        public static void SetDatePeriod(this MVCxGridViewColumn column, SecurityToken token)
        {
            column.ColumnType = MVCxGridViewColumnType.DateEdit;
            var dateEditProperties = column.PropertiesEdit as DateEditProperties;
            dateEditProperties.SetDefaultProperties(token);
            column.Settings.AllowHeaderFilter = DefaultBoolean.True;
            column.PropertiesEdit.DisplayFormatString = "d";
            column.SettingsHeaderFilter.Mode = GridHeaderFilterMode.DateRangePicker;
            column.Settings.AllowAutoFilter = DefaultBoolean.False;
        }

        /// <summary>
        /// Устанавливает настройки для колонки с числами.
        /// </summary>
        /// <param name="column">Колонка.</param>
        /// <param name="token">Токен безопасности.</param>
        public static void SetNumeric(this MVCxGridViewColumn column, SecurityToken token)
        {
            column.ColumnType = MVCxGridViewColumnType.SpinEdit;
            column.PropertiesEdit.DisplayFormatString = "#.00";
        }
    }
}