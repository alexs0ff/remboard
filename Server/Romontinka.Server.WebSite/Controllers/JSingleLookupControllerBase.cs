using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Romontinka.Server.Core.Security;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Helpers;
using log4net;

namespace Romontinka.Server.WebSite.Controllers
{
    public abstract class JSingleLookupControllerBase<TKey,TItemModel, TSearchModel> : BaseController
        where TKey : struct
        where TSearchModel : JLookupSearchBaseModel
        where TItemModel : JLookupItemBaseModel
    {
        public JSingleLookupControllerBase()
        {
            ItemsPerPage = ItemsPerPageDefault;
            PaginatorMaxItems = PaginatorMaxItemsDefault;
        }

        /// <summary>
        /// Текущий логер.
        /// </summary>
        protected static ILog _logger = LogManager.GetLogger("JLookup");

        /// <summary>
        /// Содержит количество строк на странице грида по-умолчанию.
        /// </summary>
        private const int ItemsPerPageDefault = 10;

        /// <summary>
        /// Содержит максимальное количество элементов в пагинаторе.
        /// </summary>
        private const int PaginatorMaxItemsDefault = 8;

        /// <summary>
        /// Задает или получает количество строк на странице грида.
        /// </summary>
        protected int ItemsPerPage { get; set; }

        /// <summary>
        /// Задает или получает максимальное количество страниц в пагинаторе.
        /// </summary>
        protected int PaginatorMaxItems { get; set; }

        /// <summary>
        /// Содержит наименование представления содержащее пункт выбора.
        /// </summary>
        private const string ItemViewName = "SingleLookupList";

        /// <summary>
        /// Переопределяется для заполнение списка пунктов для отображения лукапа.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="search">Модель поиска. </param>
        /// <param name="list">Список для заполнения.</param>
        /// <param name="itemsPerPage">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        protected abstract void PopulateItems(SecurityToken token,TSearchModel search, List<TItemModel> list ,int itemsPerPage, out int count);

        /// <summary>
        /// Получает строковой контент для отображения в поле ввода реестра.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="id">Код элемента.</param>
        /// <param name="parent">Значение связанного элемента. </param>
        /// <returns>Строковое предстовление.</returns>
        protected abstract string GetItemContent(SecurityToken token, TKey? id, string parent);

        /// <summary>
        /// Получение формы редактирования для определенной сущности по ее коду.
        /// </summary>
        /// <param name="id">Код сущности.</param>
        /// <param name="parent">Значение связанного элемента.</param>
        /// <returns>Результат.</returns>
        [HttpPost]
        public JsonResult GetItem(TKey id, string parent)
        {
            try
            {
                var model = GetItemContent(GetToken(), id, parent);

                return
                    Json(new JLookupItemResult
                    {
                        ResultState = CrudResultKind.Success,
                        Id = string.IsNullOrWhiteSpace(model)?null: string.Format(CultureInfo.InvariantCulture,"{0}", id),
                        Value = model
                    });

            }
            catch (Exception ex)
            {
                var innerException = string.Empty;
                if (ex.InnerException != null)
                {
                    innerException = ex.InnerException.Message;
                } //if

                _logger.ErrorFormat("Во время получения элемента для лукапа {0} с ключем {1} произошла ошибка {2} {3} {4} {5}",
                                    GetType().Name, id, ex.Message, ex.GetType(), innerException, ex.StackTrace);
                return Json(new JCrudErrorResult(string.Format("Произошла ошибка в lookup {0}", ex.Message)));
            } //try
        }

        [ChildActionOnly]
        public ViewResult SearchForm(TSearchModel search)
        {
            return View("searchForm", search);
        }

        /// <summary>
        /// Вызывается для получения пунктов для лукапа.
        /// </summary>
        /// <param name="search">Модель поиска.</param>
        /// <returns>Результат.</returns>
        [HttpPost]
        public JsonResult GetItems(TSearchModel search)
        {
            try
            {
                int count;
                var list = new List<TItemModel>();
                PopulateItems(GetToken(),search, list, ItemsPerPage, out count);

                return Json(new JLookupDataResult
                {
                    CurrentPage = search.Page,
                    ItemsData = PartialView(ItemViewName, list).RenderToString(),
                    PaginatorMaxItems = PaginatorMaxItems,
                    PerPage = ItemsPerPage,
                    ResultState = CrudResultKind.Success,
                    TotalCount = count
                });
            }
            catch (Exception ex)
            {
                var innerException = string.Empty;
                if (ex.InnerException != null)
                {
                    innerException = ex.InnerException.Message;
                } //if

                _logger.ErrorFormat("Во время получения данных для single lookup {0} произошла ошибка {1} {2} {3} {4}", GetType().Name, ex.Message, ex.GetType(), innerException, ex.StackTrace);
                return Json(new JCrudErrorResult(string.Format("Произошла ошибка в single lookup {0}", ex.Message)));
            } //try
        }
    }
}