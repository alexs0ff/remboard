using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Romontinka.Server.WebSite.Models.Dialog;

namespace Romontinka.Server.WebSite.Helpers
{
    /// <summary>
    /// Хелперы для создания диалогов.
    /// </summary>
    public static class DialogHelpers
    {
        /// <summary>
        /// Создает диалог опроса пользователя с кнопками "да" или "нет".
        /// </summary>
        public static MvcHtmlString ConfirmDialog(this HtmlHelper html,string dialogId,string title,string okButtonText,string cancelButtonText,string data,string dataElementId,string successFunctionName)
        {
            return html.Partial("ConfirmDialog",
                                new ConfirmDialogModel
                                    {
                                        Data = data,
                                        DataElementId = dataElementId,
                                        DialogId = dialogId,
                                        SuccessFunctionName = successFunctionName,
                                        Title = title,
                                        CancelButtonText = cancelButtonText,
                                        OkButtonText = okButtonText
                                    });
        }

        private const int DefaultWindowHeight = 300;

        private const int DefaultWindowWidth = 400;

        /// <summary>
        /// Создает диалог опроса пользователя с кнопками "да" или "нет".
        /// </summary>
        public static MvcHtmlString EditModelDialog(this HtmlHelper html, string dialogId, string title, string okButtonText, string cancelButtonText, string formId, string successFunctionName, bool fullScreen, int height, int width, bool noDialogTitle)
        {
            if(height<=0)
            {
                height = DefaultWindowHeight;
            }

            if (width<=0)
            {
                width = DefaultWindowWidth;
            }

            return html.Partial("EditModelDialog",
                                new EditDialogModel
                                {
                                    FormId = formId,
                                    DialogId = dialogId,
                                    SuccessFunctionName = successFunctionName,
                                    Title = title,
                                    CancelButtonText = cancelButtonText,
                                    OkButtonText = okButtonText,
                                    FullScreen = fullScreen,
                                    Height = height,
                                    Width = width,
                                    NoDialogTitle = noDialogTitle
                                });
        }
    }
}