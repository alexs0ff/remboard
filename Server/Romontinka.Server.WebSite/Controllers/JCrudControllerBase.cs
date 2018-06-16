using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Romontinka.Server.Core.Security;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Helpers;
using log4net;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Базовый контроллер для JCrud действий.
    /// </summary>
    public abstract class JCrudControllerBase<TKey,TCreateViewModel, TEditViewModel, TCreate,TEdit> : BaseController
        where TCreateViewModel : JCrudModelBase
        where TEditViewModel : JCrudModelBase
        where TCreate: class 
        where TEdit: class
        where TKey: struct
    {
        /// <summary>
        /// Текущий логер.
        /// </summary>
        protected static ILog _logger = LogManager.GetLogger("JCrud");

        /// <summary>
        /// Содержит имя представления для редактирования сущности по умолчанию.
        /// </summary>
        private const string EditItemViewNameDefault = "edit";

        /// <summary>
        /// Содержит имя представления для создания сущности по умолчанию.
        /// </summary>
        private const string CreateItemViewNameDefault = "create";

        /// <summary>
        /// Задает или получает имя представления для редактирования сущности.
        /// </summary>
        protected const string EditItemViewName = EditItemViewNameDefault;

        /// <summary>
        /// Задает или получает имя представления для создания сущности.
        /// </summary>
        protected const string CreateItemViewName = CreateItemViewNameDefault;

        /// <summary>
        /// Инициализирует модель для создания формы новой сущности.
        /// </summary>
        ///<param name="token">Токен безопасности.</param>
        /// <returns>Инициализированная модель.</returns>
        protected abstract TCreateViewModel InitCreateModel(SecurityToken token);

        /// <summary>
        /// Получение формы создания для определенной сущности.
        /// </summary>
        /// <returns>Результат.</returns>
        [HttpPost]
        public JsonResult GetNewItemForm()
        {
            try
            {
                var model = InitCreateModel(GetToken());

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

                _logger.ErrorFormat("Во время получения формы создания {0} произошла ошибка {1} {2} {3} {4}",typeof(TCreateViewModel).Name,
                                    ex.Message, ex.GetType(), innerException, ex.StackTrace);
                return
                    Json(
                        new JCrudErrorResult(string.Format("Произошла ошибка в контроллере {0}",
                                                           ex.Message)));
            } //try
        }

        /// <summary>
        /// Сохраняет модель при сохранении новой сущности.
        /// </summary>
        ///<param name="model">Модель для сохранения сущности.</param>
        ///<param name="token">Токен безопасности.</param>
        /// <returns>Сохраненная сущность.</returns>
        protected abstract TCreate SaveCreateModel(TCreateViewModel model,SecurityToken token);

        /// <summary>
        /// Сохранение данных из модели создания сущности.
        /// </summary>
        /// <param name="model">Модель создания сущности.</param>
        /// <returns>Результат.</returns>
        [HttpPost]
        public virtual JsonResult SaveCreatedItem(TCreateViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var item = SaveCreateModel(model, GetToken());

                    //Все нормально, возвращаем пункт
                    return
                        Json(new JCrudItemResult<TCreate>
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

                _logger.ErrorFormat("Во время сохранения новой сущности {0} произошла ошибка {1} {2} {3} {4}",typeof(TCreate).Name,
                                     ex.Message, ex.GetType(), innerException, ex.StackTrace);
                return Json(new JCrudErrorResult(string.Format("Произошла ошибка {0}", ex.Message)));
            } //try
        }

        /// <summary>
        /// Сохраняет модель при редактировании существующей сущности.
        /// </summary>
        ///<param name="model">Модель для сохранения редактируемой сущности.</param>
        ///<param name="token">Токен безопасности.</param>
        /// <returns>Сохраненная сущность.</returns>
        protected abstract TEdit SaveEditModel(TEditViewModel model, SecurityToken token);

        /// <summary>
        /// Сохранение редактируемой сущности.
        /// </summary>
        /// <param name="model">Модель редактирования сущности.</param>
        /// <returns>Результат.</returns>
        [HttpPost]
        public virtual JsonResult SaveEditItem(TEditViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var item = SaveEditModel(model, GetToken());

                    //Все нормально, возвращаем пункт
                    return
                        Json(new JCrudItemResult<TEdit>
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

                _logger.ErrorFormat("Во время сохранения существующей сущности {0} произошла ошибка {1} {2} {3} {4}",typeof(TEdit).Name,
                                    ex.Message, ex.GetType(), innerException, ex.StackTrace);
                return Json(new JCrudErrorResult(string.Format("Произошла ошибка {0}", ex.Message)));
            } //try
        }

        /// <summary>
        /// Инициализирует модель для редактирования формы существующей сущности.
        /// </summary>
        ///<param name="token">Токен безопасности.</param>
        ///<param name="id">Код сущности.</param>
        ///<returns>Инициализированная модель редактирования.</returns>
        protected abstract TEditViewModel InitEditModel(SecurityToken token,TKey? id);

        /// <summary>
        /// Получение формы редактирования для сущности по ее коду.
        /// </summary>
        /// <param name="id">Код сущности.</param>
        /// <returns>Результат.</returns>
        [HttpPost]
        public JsonResult GetItemForm(TKey? id)
        {
            try
            {
                var model = InitEditModel(GetToken(), id);

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

                _logger.ErrorFormat(
                    "Во время получения формы для редактирования сущности {0} с ключем {1} произошла ошибка {2} {3} {4} {5}",typeof(TEditViewModel).Name,
                    id, ex.Message, ex.GetType(), innerException, ex.StackTrace);
                return Json(new JCrudErrorResult(string.Format("Произошла ошибка {0}", ex.Message)));
            } //try
        }

        /// <summary>
        /// Удаляет сущность.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="id">Код сущности для удаления.</param>
        protected abstract void DeleteEntity(SecurityToken token,TKey? id);

        /// <summary>
        /// Получает запрос на удаление сущности.
        /// </summary>
        /// <param name="id">Код сущности для удаления.</param>
        /// <returns>Результат.</returns>
        [HttpPost]
        public JsonResult DeleteItem(TKey? id)
        {
            try
            {
                var token = GetToken();
                DeleteEntity(token, id);
                return Json(new JCrudResult {ResultState = CrudResultKind.Success});
            }
            catch (Exception ex)
            {
                var innerException = string.Empty;
                if (ex.InnerException != null)
                {
                    innerException = ex.InnerException.Message;
                } //if

                _logger.ErrorFormat("Во время удаления сущности {0} с ключем {1} произошла ошибка {2} {3} {4} {5}",typeof(TEditViewModel).Name,
                                     id, ex.Message, ex.GetType(), innerException, ex.StackTrace);
                return Json(new JCrudErrorResult(string.Format("Произошла ошибка {0}", ex.Message)));
            } //try
        }
    }
}