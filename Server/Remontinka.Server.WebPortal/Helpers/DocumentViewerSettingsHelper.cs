using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevExpress.Web.Mvc;
using Remontinka.Server.WebPortal.Models.Common;

namespace Remontinka.Server.WebPortal.Helpers
{
    /// <summary>
    /// Хелпер для отображения отчетов.
    /// </summary>
    public static class DocumentViewerSettingsHelper
    {
        /// <summary>
        /// Настроивает репорт вьюер по модели данных отчета.
        /// </summary>
        /// <param name="settings">Настройки.</param>
        /// <param name="model">Модель данных.</param>
        public static void SetModel(this DocumentViewerSettings settings, ReportViewModel model)
        {
            settings.Name = model.DocumentViewerName;
            settings.Report = model.Report;
            settings.CallbackRouteValues = new { Controller = model.ControllerName, Action = model.ActionName };
            settings.ExportRouteValues = new { Controller = model.ControllerName, Action = model.ExportActionName };
            settings.ClientSideEvents.BeginCallback = model.UpdateParametersFunctionName;
            settings.ClientSideEvents.BeforeExportRequest = model.UpdateParametersFunctionName;
            //TODO: убрать после регистрации
            settings.ClientSideEvents.EndCallback = "function(s,e){ $(\"img[alt$= 'Close']\").click();}";
        }
    }
}