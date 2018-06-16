using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Romontinka.Server.WebSite.Common;
using log4net;

namespace Romontinka.Server.WebSite.Metadata
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
            if (context.HttpContext.Request.IsAjaxRequest())
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