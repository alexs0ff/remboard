using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Practices.Unity;
using Romontinka.Server.Core;
using Romontinka.Server.WebSite.Metadata;
using log4net;
using log4net.Config;
using log4net.Repository.Hierarchy;

namespace Romontinka.Server.WebSite
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    //Использована следующая палитра цветов http://www.colourlovers.com/palette/46688/fresh_cut_day
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Текущий логер.
        /// </summary>
        private static ILog _logger;

        protected void Application_Start()
        {
            XmlConfigurator.Configure();
            _logger =LogManager.GetLogger(typeof (MvcApplication));
            
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            SetUpUnityContainer();

            ControllerBuilder.Current.SetControllerFactory(new UnityControllerFactory());

            ModelBinders.Binders.Add(typeof(DateTime), new DateTimeModelBinder());
            ModelBinders.Binders.Add(typeof(DateTime?), new DateTimeModelBinder());
            ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());
            ModelBinders.Binders.Add(typeof(decimal?), new DecimalModelBinder());
            //_logger.InfoFormat("Старт приложения.. {0}", HostingEnvironment.ApplicationID);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;
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

        protected void Application_End()
        {

            //HttpRuntime runtime = (HttpRuntime) typeof (System.Web.HttpRuntime).InvokeMember("_theRuntime",

            //                                                                                 BindingFlags.NonPublic

            //                                                                                 | BindingFlags.Static

            //                                                                                 | BindingFlags.GetField,

            //                                                                                 null,

            //                                                                                 null,

            //                                                                                 null);
            //string shutDownMessage = "shutDownMessage";
            //string shutDownStack = "shutDownMessage";
            //if (runtime != null)
            //{



            //    shutDownMessage = (string) runtime.GetType().InvokeMember("_shutDownMessage",

            //                                                                     BindingFlags.NonPublic

            //                                                                     | BindingFlags.Instance

            //                                                                     | BindingFlags.GetField,

            //                                                                     null,

            //                                                                     runtime,

            //                                                                     null);



            //    shutDownStack = (string) runtime.GetType().InvokeMember("_shutDownStack",

            //                                                                   BindingFlags.NonPublic

            //                                                                   | BindingFlags.Instance

            //                                                                   | BindingFlags.GetField,

            //                                                                   null,

            //                                                                   runtime,

            //                                                                   null);
            //}
            //Process proc = Process.GetCurrentProcess();
            //var total = proc.PeakWorkingSet64 / 1024 + "kb";


            //_logger.InfoFormat("Завершение приложения.. {0} {1} {2}", HostingEnvironment.ApplicationID, shutDownMessage,
            //                   shutDownStack);
        }

        /// <summary>
        /// Вызывается при упущенном исключении.
        /// </summary>
        void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string innerException = string.Empty;

            var ex = (Exception) e.ExceptionObject;
            if (ex.InnerException != null)
            {
                innerException = ex.InnerException.Message;
            } //if
            _logger.ErrorFormat("Вызвана неизвестная ошибка в домене приложении {0} {1} {2} {3}", ex.GetType(), ex.Message, innerException, ex.StackTrace);
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

    }
}