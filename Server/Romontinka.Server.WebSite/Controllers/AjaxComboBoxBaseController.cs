using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Romontinka.Server.Core.Security;
using Romontinka.Server.WebSite.Common;
using log4net;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Базовый контоллер для aJax комбобоксов.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AjaxComboBoxBaseController<T>:BaseController
        where T:struct
    {
        /// <summary>
        /// Текущий логер.
        /// </summary>
        private static ILog _logger = LogManager.GetLogger("AjaxComboBox");

        /// <summary>
        /// Возвращает инициализированный список пунктов.
        /// </summary>
        /// <returns>Список</returns>
        private List<JSelectListItem<T>> GetList()
        {
            return new List<JSelectListItem<T>>();
        }

        /// <summary>
        /// Переопределяется для получения переопределенного списка с пунктами.
        /// </summary>
        /// <param name="token">Текущий токен безопасности. </param>
        /// <param name="list">Список который необходимо инициализировать.</param>
        /// <param name="selectedId"> Выбранный пункт.</param>
        /// <param name="parentValue">Значение родительского контрола.</param>
        public abstract void GetInitializedItems(SecurityToken token,List<JSelectListItem<T>> list, T? selectedId, string parentValue);

        /// <summary>
        /// Производит выборку пунктов для отображения в комбобоксе.
        /// </summary>
        /// <param name="id">Код выбранного пункта.</param>
        /// <param name="parent">Значение родительского контрола.</param>
        /// <returns>Результат.</returns>
        [HttpPost]
        public JsonResult GetItems(T? id, string parent)
        {
            try
            {
                var token = GetToken();
                var items = GetList();
                GetInitializedItems(token, items, id, parent);

                return Json(new JCrudItemsResult<JSelectListItem<T>>
                            {
                                Items = items,
                                ResultState = CrudResultKind.Success
                            });
            }
            catch (Exception ex)
            {

                var innerException = string.Empty;
                if (ex.InnerException != null)
                {
                    innerException = ex.InnerException.Message;
                } //if

                _logger.ErrorFormat(
                    "Во время получения данных для создания AjaxComboBox {0} произошла ошибка {1} {2} {3} {4}",
                    GetType().Name, ex.Message, ex.GetType(), innerException, ex.StackTrace);
                return Json(new JCrudErrorResult(string.Format("Произошла ошибка в AjaxComboBox {0}", ex.Message)));
            } //try

        }

    }
}