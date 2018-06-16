using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using log4net;
using Remontinka.Server.WebPortal.Models.Common;

namespace Remontinka.Server.WebPortal.Metadata
{
    /// <summary>
    /// Расширенный атрибут для запроса авторизации.
    /// </summary>
    public class ExtendedAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Текущий логер.
        /// </summary>
        protected static ILog _logger = LogManager.GetLogger(typeof(ExtendedAuthorizeAttribute));

        protected override void HandleUnauthorizedRequest(AuthorizationContext context)
        {
            if (DevExpressHelper.IsCallback || context.HttpContext.Request.IsAjaxRequest())
            {
                _logger.WarnFormat("Контекст авторизации не пройден {0}", context.Controller);

                var urlHelper = new UrlHelper(context.RequestContext);
                //Делаем статус нормальным, чтобы обработать инфраструктурой Jcrud
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Result = new JsonResult
                {
                    Data = new JCrudAuthorizeResult(urlHelper.Action("Login", "Account"))
                };
            }
            else
            {
                base.HandleUnauthorizedRequest(context);
            }
        }
    }
}