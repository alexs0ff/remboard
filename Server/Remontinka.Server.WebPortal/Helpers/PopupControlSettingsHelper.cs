using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using DevExpress.Web.Mvc.UI;
using Romontinka.Server.Core.Security;

namespace Remontinka.Server.WebPortal.Helpers
{
    /// <summary>
    /// Хелпер для работы с popup.
    /// </summary>
    public static class PopupControlSettingsHelper
    {
        /// <summary>
        /// Устанавливает настройки по-умолчанию для popup контролов.
        /// </summary>
        /// <param name="settings">Настройки.</param>
        /// <param name="token">Токен безорпасности.</param>
        public static void InitEditButtonsOnFooter(this PopupControlSettings settings, SecurityToken token,HtmlHelper helper,string clickFunction)
        {
            settings.ShowFooter = true;
            settings.SetFooterTemplateContent(c =>
            {
                //TODO: Перенести в стили
                helper.ViewContext.Writer.Write("<div style=\"overflow: hidden\">");
                helper.ViewContext.Writer.Write("<div style=\"padding: 3px; float: right;\">");
                

                helper.DevExpress().Button(buttonsettings =>
                {
                    buttonsettings.Name = settings.Name + "CancelButton";
                    buttonsettings.Text = "Отменить";
                    buttonsettings.ClientSideEvents.Click = string.Format("function(s, e) {{ {0}.Hide(); }}", settings.Name);
                }).Render();

                helper.ViewContext.Writer.Write("</div><div style=\"padding: 3px; float: right;\">");
                helper.DevExpress().Button(buttonsettings =>
                {
                    buttonsettings.Name = settings.Name + "UpdateButton";
                    buttonsettings.Text = "Сохранить";
                    
                    buttonsettings.ClientSideEvents.Click = clickFunction;
                }).Render();
                helper.ViewContext.Writer.Write("</div></div>");
            });
        }
    }
}