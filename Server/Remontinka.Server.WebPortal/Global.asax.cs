using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DevExpress.Web.Mvc;
using log4net;
using log4net.Config;
using Microsoft.Practices.Unity;
using Remontinka.Server.WebPortal.Services;
using Romontinka.Server.Core;

namespace Remontinka.Server.WebPortal
{
    //Как изменять темы https://www.devexpress.com/Support/Center/Example/Details/E3825
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Текущий логер.
        /// </summary>
        private static ILog _logger;

        protected void Application_Start()
        {
            XmlConfigurator.Configure();
            _logger = LogManager.GetLogger(typeof(MvcApplication));
            SetUpUnityContainer();

            //ControllerBuilder.Current.SetControllerFactory(new UnityControllerFactory());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;
            ModelBinders.Binders.DefaultBinder = new DevExpress.Web.Mvc.DevExpressEditorsBinder();
        }

        /// <summary>
        /// Задает всю конфигурацию типов для ориона.
        /// </summary>
        private static void SetUpUnityContainer()
        {
            var container = new UnityContainer();
            //конфигурируем основной сервер.
            RemontinkaServer.SetConfiguration(new SiteConfiguration(container));
        }

        /// <summary>
        /// Вызывается при упущенном исключении.
        /// </summary>
        void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = (Exception)e.ExceptionObject;
            LogException(ex);
        }

        public static string LogException(Exception ex)
        {
            string result = string.Empty;
            try
            {

                string innerException = string.Empty;
                if (ex.InnerException != null)
                {
                    innerException = ex.InnerException.Message;
                } //if
                result = string.Format("Вызвана неизвестная ошибка в домене приложении {0} {1} {2} {3}", ex.GetType(),
                    ex.Message,
                    innerException, ex.StackTrace);
                _logger.Error(result);
            }
            catch{
                
            }

            return result;
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            Response.Clear();

            HttpException httpException = exception as HttpException;

            string innerException = string.Empty;

            if (exception.InnerException != null)
            {
                innerException = exception.InnerException.Message;
            } //if

            if (httpException != null)
            {
                _logger.ErrorFormat("Вызвана ошибка в http приложении {0} {1} {2}", httpException.Message, httpException.GetHttpCode(), innerException);
                // clear error on server
                Server.ClearError();

            }
            else
            {
                _logger.ErrorFormat("Вызвана неизвестная ошибка в http приложении {0} {1} {2} {3}", exception.GetType(), exception.Message, innerException, exception.StackTrace);
            } //else
        }

        protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            if (HttpContext.Current.User!=null && HttpContext.Current.User.Identity!=null && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                DevExpressHelper.Theme = RemontinkaServer.Instance.GetService<IWebSiteSettingsService>().GetDevexpressTheme(HttpContext.Current.User.Identity.Name);
            }
        }
    }
}
