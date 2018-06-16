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
    /// Базовый клаcc для контроллеров списка check боксев.
    /// </summary>
    /// <typeparam name="T">Тип идентификатора.</typeparam>
    public abstract class CheckBoxListBaseController<T> : BaseController
        where T:struct 
    {
        /// <summary>
        /// Текущий логер.
        /// </summary>
        private static ILog _logger = LogManager.GetLogger("CheckBoxList");

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
        /// <param name="token">Токен безопасности.</param>
        /// <param name="list">Список который необходимо инициализировать.</param>
        /// <param name="selectedIds"> Выбранныйые пункты.</param>
        /// <param name="parentValue">Значение родительского контрола.</param>
        public abstract void GetInitializedItems(SecurityToken token,List<JSelectListItem<T>> list, T[] selectedIds, string parentValue);

        /// <summary>
        /// Производит выборку пунктов для отображения в check листах.
        /// </summary>
        /// <param name="ids">Коды выбранных пунктов.</param>
        /// <param name="parent">Значение родительского контрола.</param>
        /// <returns>Результат.</returns>
        [HttpPost]
        public JsonResult GetItems(T[] ids, string parent)
        {
            try
            {
                var items = GetList();
                if (ids==null)
                {
                    ids = new T[0];
                }
                var token = GetToken();
                GetInitializedItems(token, items, ids, parent);

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
                    "Во время получения данных для создания CheckBoxList {0} произошла ошибка {1} {2} {3} {4}",
                    GetType().Name, ex.Message, ex.GetType(), innerException, ex.StackTrace);
                return Json(new JCrudErrorResult(string.Format("Произошла ошибка в CheckBoxList {0}", ex.Message)));
            } //try

        }

    }
}