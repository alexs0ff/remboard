using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;
using Remontinka.Server.WebPortal.Helpers;
using Remontinka.Server.WebPortal.Models.Common;
using Romontinka.Server.Core.Security;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// Базовый контроллер для лоокпа гридов.
    /// </summary>
    public abstract class GridLookupControllerBase<TModelBase> : BaseController
        where TModelBase : GridLookupModelBase, new()
    {
        /// <summary>
        /// Текущий логер.
        /// </summary>
        protected static ILog _logger = LogManager.GetLogger("GridLookupController");

        /// <summary>
        /// Возвращает имя контроллера.
        /// </summary>
        /// <returns>Имя контроллера.</returns>
        protected virtual string GetControllerName()
        {
            return string.Empty;
        }
       

        /// <summary>
        /// Инициализирует модель дефолтными значениями.
        /// </summary>
        private TModelBase CreateModel()
        {
            var model = new TModelBase();

            model.ControllerName = GetControllerName();
            model.ActionName = GridLookupSettingsHelper.ActionDefaultName;
            var token = GetToken();
            IntitializeModel(token, model);
            model.Data = GetData(token);

            return model;
        }

        /// <summary>
        /// Содержит наименование представления с гридом.
        /// </summary>
        private const string GridViewName = "lookup";

        /// <summary>
        /// Переопределяется, чтобы инициализировать модель.
        /// </summary>
        /// <param name="model">Модель для инициализации</param>
        /// <param name="token">Токен безопасности.</param>
        protected virtual void IntitializeModel(SecurityToken token, TModelBase model)
        {
            
        }

        /// <summary>
        /// Вохвращает данные для грида.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns></returns>
        protected abstract IQueryable GetData(SecurityToken token);

        [ValidateInput(false)]
        public ActionResult GetGridData(string fieldName, object initionalValue)
        {
            TModelBase model = null;
            try
            {
                model = CreateModel();
                if (!string.IsNullOrWhiteSpace(fieldName))
                {
                    model.FieldName = fieldName;
                }
                model.Value = initionalValue;

                //var param = Request.Params[model.FieldName];

                //if (param != null)
                //{
                //    param = param.Trim('"');
                //    if (string.IsNullOrWhiteSpace(param))
                //    {
                //        model.Value = null;
                //    }

                //    else
                //    {
                //        model.Value = param;
                //    }
                //}
            }
            catch (Exception ex)
            {
                var innerException = string.Empty;
                if (ex.InnerException != null)
                {
                    innerException = ex.InnerException.Message;
                } //if

                _logger.ErrorFormat("Во время получения данных для лукапа {0} произошла ошибка {1} {2} {3} {4}",
                    GetType().Name, ex.Message, ex.GetType(), innerException, ex.StackTrace);
            }
            return PartialView(GridViewName, model);
        }
    }
}