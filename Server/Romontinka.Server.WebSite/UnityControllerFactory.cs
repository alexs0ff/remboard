using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Practices.Unity;
using Romontinka.Server.Core;
using log4net;

namespace Romontinka.Server.WebSite
{
    /// <summary>
    ///   Фабрика контролеров для текущего сайта c dependency injection.
    /// </summary>
    public class UnityControllerFactory : DefaultControllerFactory
    {
        /// <summary>
        ///   Текущий логер.
        /// </summary>
        private static readonly ILog _logger = LogManager.GetLogger(typeof(UnityControllerFactory));

        /// <summary>
        ///   Retrieves the controller instance for the specified request context and controller type.
        /// </summary>
        /// <returns> The controller instance. </returns>
        /// <param name="requestContext"> The context of the HTTP request, which includes the HTTP context and route data. </param>
        /// <param name="controllerType"> The type of the controller. </param>
        /// <exception cref="T:System.Web.HttpException">
        ///   <paramref name="controllerType" />
        ///   is null.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="controllerType" />
        ///   cannot be assigned.</exception>
        /// <exception cref="T:System.InvalidOperationException">An instance of
        ///   <paramref name="controllerType" />
        ///   cannot be created.</exception>
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                _logger.ErrorFormat("Нет возможности создать контроллер из запроса {0}",requestContext.HttpContext.Request.RawUrl);
                return null;
            } //if

            try
            {
                return (IController)RemontinkaServer.Instance.Container.Resolve(controllerType);
            }
            catch (Exception ex)
            {
                _logger.WarnFormat(
                    "Неудачная попытка создать тип контроллера {0} побробности {1}, пользуемся базовым методом",
                    controllerType, ex.Message);
                return base.GetControllerInstance(requestContext, controllerType);
            }
        }
    }
}