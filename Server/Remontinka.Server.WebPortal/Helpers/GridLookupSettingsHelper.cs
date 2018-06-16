using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using Remontinka.Server.WebPortal.Controllers;
using Remontinka.Server.WebPortal.Models.Common;

namespace Remontinka.Server.WebPortal.Helpers
{
    /// <summary>
    /// Хелпер для настроек лукапов для грида.
    /// </summary>
    public static class GridLookupSettingsHelper
    {
        /// <summary>
        /// Настраивает грид согласно модели.
        /// </summary>
        /// <param name="settings">Настройки лукапа.</param>
        /// <param name="model">Модель лукапа.</param>
        public static void SetUp(this GridLookupSettings settings, GridLookupModelBase model)
        {
            settings.KeyFieldName = model.FieldName;
            settings.Name = model.FieldName;
            settings.GridViewProperties.SettingsBehavior.EnableRowHotTrack = true;
            settings.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
            settings.ShowModelErrors = true;
            settings.Width = Unit.Percentage(100);
            settings.GridViewProperties.CallbackRouteValues =
                new {Controller = model.ControllerName, Action = model.ActionName, fieldName = model.FieldName, initionalValue = model.Value};

            settings.GridViewProperties.SettingsPager.Position = PagerPosition.Bottom;
            settings.GridViewProperties.SettingsPager.FirstPageButton.Visible = true;
            settings.GridViewProperties.SettingsPager.LastPageButton.Visible = true;
            settings.GridViewProperties.SettingsPager.PageSizeItemSettings.Visible = true;
            settings.GridViewProperties.SettingsPager.PageSizeItemSettings.Items = new string[] { "5", "10", "15" };
        }

        /// <summary>
        /// Производит биндинг к модели данных.
        /// </summary>
        /// <param name="extension">Лукап грида.</param>
        /// <param name="model">Модель.</param>
        /// <returns></returns>
        public static MvcHtmlString BindToModel(this GridLookupExtension extension, GridLookupModelBase model)
        {
            return
                extension.BindToLINQ(String.Empty, String.Empty, (sender, args) =>
                    {
                        args.QueryableSource = model.Data;
                        args.KeyExpression = model.KeyField;
                    }).Bind(model.Value)
                    .GetHtml();
        }

        /// <summary>
        /// Содержит наименование действия для получения грида.
        /// </summary>
        public const string ActionDefaultName = "GetGridData";

        /// <summary>
        /// Создает грид лукап для определенного контроллера и параметра.
        /// </summary>
        /// <param name="helper">Html хелпер.</param>
        /// <param name="controllerName">Имя контроллера.</param>
        /// <param name="propertyName">Имя параметра.</param>
        /// <param name="value">Значения для биндинга.</param>
        /// <returns>Строка html.</returns>
        public static void RenderGridLookUp(this HtmlHelper helper, string controllerName, string propertyName,object value)
        {
            helper.RenderAction(ActionDefaultName, controllerName,new {fieldName =propertyName, initionalValue = value });
        }
    }
}