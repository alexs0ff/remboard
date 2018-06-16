using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Romontinka.Server.WebSite.Common;
using log4net;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Представляет базовый класс для простых лукапов.
    /// </summary>
    /// <typeparam name="T">Тип идентификатора.</typeparam>
    public abstract class SimpleLookupBaseController<T>:BaseController
        where T:struct
    {
        /// <summary>
        /// Текущий логер.
        /// </summary>
        private static ILog _logger = LogManager.GetLogger("SimpleLookup");

        /// <summary>
        /// Переопределяется для получения переопределенного списка с пунктами.
        /// </summary>
        /// <param name="list">Список который необходимо инициализировать.</param>
        /// <param name="query"> Строка поиска.</param>
        /// <param name="parentValue">Значение родительского контрола.</param>
        public abstract void GetInitializedItems(List<LookupItem<T>> list, string query, string parentValue);
        
        /// <summary>
        /// Получает пункт для лукапа по его идентификатору.
        /// </summary>
        /// <param name="itemId">Код пункта.</param>
        /// <returns>Пункт для лукапа.</returns>
        public abstract LookupItem<T> GetInitializedItem(T? itemId);

        /// <summary>
        /// Производит выборку пунктов для отображения в лукапе.
        /// </summary>
        /// <param name="query">Строка поиска.</param>
        /// <param name="parent">Значение родительского контрола.</param>
        /// <returns>Результат.</returns>
        [HttpPost]
        public JsonResult GetItems(string query, string parent)
        {
            try
            {
                var items = new List<LookupItem<T>>();
                GetInitializedItems(items, query, parent);

                return Json(new JCrudItemsResult<LookupItem<T>>
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

         /// <summary>
        /// Производит выборку конкретного пункта для отображения в лукапе.
        /// </summary>
        /// <param name="id">Идентификатор пункта.</param>
        /// <returns>Результат.</returns>
        [HttpPost]
        public JsonResult GetItem(T? id)
         {
             try
             {
                 var item = GetInitializedItem(id);
                 if (item==null)
                 {
                     throw new Exception(string.Format("Нет такого пункта {0}", id));
                 } //if
                 return Json(new JCrudItemResult<LookupItem<T>>
                 {
                     Item = item,
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
                     "Во время получения данных для создания SimpleLookup {0} произошла ошибка {1} {2} {3} {4}",
                     GetType().Name, ex.Message, ex.GetType(), innerException, ex.StackTrace);
                 return Json(new JCrudErrorResult(string.Format("Произошла ошибка в SimpleLookup {0}", ex.Message)));
             } //try
         }
    }
}