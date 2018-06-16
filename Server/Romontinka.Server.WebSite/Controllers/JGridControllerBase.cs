using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Helpers;
using Romontinka.Server.WebSite.Models;
using log4net;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Базовый контроллер для работы с j-гридом.
    /// </summary>
    /// <typeparam name="TKey">Тип ключа.</typeparam>
    /// <typeparam name="TGridItemModel">Модель данных для пункта грида.</typeparam>
    /// <typeparam name="TCreateModel">Модель данных для формы создания сущности.</typeparam>
    /// <typeparam name="TEditModel">Модель данных для формы редактирования сущности.</typeparam>
    /// <typeparam name="TSearchModel">Модель поиска.</typeparam>
    public class JGridControllerBase<TKey, TGridItemModel, TCreateModel, TEditModel, TSearchModel> : BaseController
        where TKey : struct
        where TSearchModel : JGridSearchBaseModel
        where TCreateModel : JGridDataModelBase<TKey>
        where TEditModel : JGridDataModelBase<TKey>
        where TGridItemModel : JGridItemModel<TKey>
    {

        /// <summary>
        /// Текущий логер.
        /// </summary>
        protected static ILog _logger = LogManager.GetLogger("JGrid");

        /// <summary>
        /// Содержит адаптер для данных грида.
        /// </summary>
        private readonly JGridDataAdapterBase<TKey, TGridItemModel, TCreateModel, TEditModel, TSearchModel> _adapter;

        /// <summary>
        /// Инициализирует новый инстанс для контроллера данных грида.
        /// </summary>
        /// <param name="adapter">Адаптер даных.</param>
        public JGridControllerBase(JGridDataAdapterBase<TKey,TGridItemModel,TCreateModel,TEditModel,TSearchModel> adapter)
        {
            _adapter = adapter;
            ItemsPerPage = ItemsPerPageDefault;
            PaginatorMaxItems = PaginatorMaxItemsDefault;
            EditItemViewName = EditItemViewNameDefault;
            CreateItemViewName = CreateItemViewNameDefault;
        }

        /// <summary>
        /// Содержит количество строк на странице грида по-умолчанию.
        /// </summary>
        private const int ItemsPerPageDefault = 10;

        /// <summary>
        /// Содержит максимальное количество элементов в пагинаторе.
        /// </summary>
        private const int PaginatorMaxItemsDefault = 8;

        /// <summary>
        /// Содержит название представления для отображения формы редактирования сущности.
        /// </summary>
        protected const string EditItemViewNameDefault = "edit";

        /// <summary>
        /// Содержит название представления для отображения формы создания сущности.
        /// </summary>
        protected const string CreateItemViewNameDefault = "create";

        /// <summary>
        /// Задает или получает количество строк на странице грида.
        /// </summary>
        protected int ItemsPerPage { get; set; }

        /// <summary>
        /// Задает или получает максимальное количество страниц в пагинаторе.
        /// </summary>
        protected int PaginatorMaxItems { get; set; }

        /// <summary>
        /// Задает или получает имя представления для редактирования сущности.
        /// </summary>
        protected string EditItemViewName { get; set; }

        /// <summary>
        /// Задает или получает имя представления для создания сущности.
        /// </summary>
        public string CreateItemViewName { get; set; }

        /// <summary>
        /// Вызывается для получения пунктов для грида.
        /// </summary>
        /// <param name="search">Модель поиска.</param>
        /// <returns>Результат.</returns>
        [HttpPost]
        public JsonResult GetItems(TSearchModel search)
        {
            try
            {
                int count;
                var items = _adapter.GetPageableGridItems(GetToken(), search, ItemsPerPage, out count);
                return Json(new JGridDataResult<TGridItemModel>
                            {
                                CurrentPage = search.Page,
                                Items = items,
                                PaginatorMaxItems = PaginatorMaxItems,
                                PerPage = ItemsPerPage,
                                ResultState = CrudResultKind.Success,
                                TotalCount = count
                            });
            }
            catch (Exception ex)
            {
                var innerException = string.Empty;
                if (ex.InnerException!=null)
                {
                    innerException = ex.InnerException.Message;
                } //if

                _logger.ErrorFormat("Во время получения данных для грида {0} произошла ошибка {1} {2} {3} {4}", GetType().Name, ex.Message, ex.GetType(), innerException, ex.StackTrace);
                return Json(new JCrudErrorResult(string.Format("Произошла ошибка в таблице \"{0}\"",ex.Message)));
            } //try
        }

        /// <summary>
        /// Получает запрос на удаление данных.
        /// </summary>
        /// <param name="id">Код сущности для удаления.</param>
        /// <returns>Результат.</returns>
        [HttpPost]
        public JsonResult DeleteItem(TKey id)
        {
            try
            {
                _adapter.DeleteEntity(GetToken(), id);

                return Json(new JCrudResult {ResultState = CrudResultKind.Success});

            }
            catch (Exception ex)
            {
                var innerException = string.Empty;
                if (ex.InnerException != null)
                {
                    innerException = ex.InnerException.Message;
                } //if

                _logger.ErrorFormat("Во время удаления сущности {0} с ключем {1} произошла ошибка {2} {3} {4} {5}",
                                    GetType().Name, id, ex.Message, ex.GetType(), innerException, ex.StackTrace);
                return Json(new JCrudErrorResult(string.Format("Произошла ошибка в таблице \"{0}\"", ex.Message)));
            } //try
        }

        /// <summary>
        /// Получение формы редактирования для определенной сущности по ее коду.
        /// </summary>
        /// <param name="id">Код сущности.</param>
        /// <returns>Результат.</returns>
        [HttpPost]
        public JsonResult GetItemForm(TKey id)
        {
            try
            {
                var model = _adapter.CreateEditedModel(GetToken(), id);

                return
                    Json(new JCrudDataResult
                         {
                             ResultState = CrudResultKind.Success,
                             NeedReloadModel = false,
                             Data = PartialView(EditItemViewName, model).RenderToString()
                         });

            }
            catch (Exception ex)
            {
                var innerException = string.Empty;
                if (ex.InnerException != null)
                {
                    innerException = ex.InnerException.Message;
                } //if

                _logger.ErrorFormat("Во время получения формы для редактирования {0} с ключем {1} произошла ошибка {2} {3} {4} {5}",
                                    GetType().Name, id, ex.Message, ex.GetType(), innerException, ex.StackTrace);
                return Json(new JCrudErrorResult(string.Format("Произошла ошибка в таблице \"{0}\"", ex.Message)));
            } //try
        }

        /// <summary>
        /// Получение формы создания для определенной сущности по ее коду.
        /// </summary>
        /// <param name="searchModel">Модель поиска.</param>
        /// <returns>Результат.</returns>
        [HttpPost]
        public JsonResult GetNewItemForm(TSearchModel searchModel)
        {
            try
            {
                var model = _adapter.CreateNewModel(GetToken(), searchModel);

                return
                    Json(new JCrudDataResult
                    {
                        ResultState = CrudResultKind.Success,
                        NeedReloadModel = false,
                        Data = PartialView(CreateItemViewName, model).RenderToString()
                    });

            }
            catch (Exception ex)
            {
                var innerException = string.Empty;
                if (ex.InnerException != null)
                {
                    innerException = ex.InnerException.Message;
                } //if

                _logger.ErrorFormat("Во время получения формы для создания {0} произошла ошибка {1} {2} {3} {4}",
                                    GetType().Name, ex.Message, ex.GetType(), innerException, ex.StackTrace);
                return Json(new JCrudErrorResult(string.Format("Произошла ошибка в таблице \"{0}\"", ex.Message)));
            } //try
        }

        /// <summary>
        /// Сохранение данных из модели редактирования.
        /// </summary>
        /// <param name="model">Модель редактирования.</param>
        /// <returns>Результат.</returns>
        [HttpPost]
        public JsonResult SaveEditedItem(TEditModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var modelResult = new JGridSaveModelResult();

                    var item = _adapter.SaveEditModel(GetToken(), model, modelResult);

                    if (modelResult.HasModelError)
                    {
                        foreach (var modelError in modelResult.ModelErrors)
                        {
                            ModelState.AddModelError(modelError.FirstItem, modelError.SecondItem);
                        } //foreach

                        return
                            Json(new JCrudDataResult
                                 {
                                     ResultState = CrudResultKind.Success,
                                     NeedReloadModel = true,
                                     Data=PartialView(EditItemViewName, model).RenderToString()
                                 });
                    } //if
                    //Все нормально, возвращаем пункт
                    return
                        Json(new JCrudItemResult<TGridItemModel>
                             {
                                 ResultState = CrudResultKind.Success,
                                 Item = item
                             });
                } //if

                //Есть ошибки модели
                return
                    Json(new JCrudDataResult
                         {
                             ResultState = CrudResultKind.Success,
                             NeedReloadModel = true,
                             Data = PartialView(EditItemViewName, model).RenderToString()
                         });
            }
            catch (Exception ex)
            {
                var innerException = string.Empty;
                if (ex.InnerException != null)
                {
                    innerException = ex.InnerException.Message;
                } //if

                _logger.ErrorFormat("Во время получения формы для редактирования {0} произошла ошибка {1} {2} {3} {4}",
                                    GetType().Name, ex.Message, ex.GetType(), innerException, ex.StackTrace);
                return Json(new JCrudErrorResult(string.Format("Произошла ошибка в таблице \"{0}\"", ex.Message)));
            } //try
        }

        /// <summary>
        /// Сохранение данных из модели создания сущности.
        /// </summary>
        /// <param name="model">Модель создания.</param>
        /// <returns>Результат.</returns>
        [HttpPost]
        public JsonResult SaveCreatedItem(TCreateModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var modelResult = new JGridSaveModelResult();

                    var item = _adapter.SaveCreateModel(GetToken(), model, modelResult);

                    if (modelResult.HasModelError)
                    {
                        foreach (var modelError in modelResult.ModelErrors)
                        {
                            ModelState.AddModelError(modelError.FirstItem, modelError.SecondItem);
                        } //foreach

                        return
                            Json(new JCrudDataResult
                            {
                                ResultState = CrudResultKind.Success,
                                NeedReloadModel = true,
                                Data = PartialView(CreateItemViewName, model).RenderToString()
                            });
                    } //if
                    //Все нормально, возвращаем пункт
                    return
                        Json(new JCrudItemResult<TGridItemModel>
                        {
                            ResultState = CrudResultKind.Success,
                            Item = item
                        });
                } //if

                //Есть ошибки модели
                return
                    Json(new JCrudDataResult
                    {
                        ResultState = CrudResultKind.Success,
                        NeedReloadModel = true,
                        Data = PartialView(CreateItemViewName, model).RenderToString()
                    });
            }
            catch (Exception ex)
            {
                var innerException = string.Empty;
                if (ex.InnerException != null)
                {
                    innerException = ex.InnerException.Message;
                } //if

                _logger.ErrorFormat("Во время получения формы для создания {0} произошла ошибка {1} {2} {3} {4}",
                                    GetType().Name, ex.Message, ex.GetType(), innerException, ex.StackTrace);
                return Json(new JCrudErrorResult(string.Format("Произошла ошибка в таблице \"{0}\"", ex.Message)));
            } //try
        }
    }
}