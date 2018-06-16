using DevExpress.Data;
using DevExpress.Data.Filtering;
using DevExpress.Data.Linq;
using DevExpress.Data.Linq.Helpers;
using DevExpress.Web.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.Web.Data;
using log4net;
using Remontinka.Server.WebPortal.Helpers;
using Remontinka.Server.WebPortal.Models;
using Remontinka.Server.WebPortal.Models.Common;
using Remontinka.Server.WebPortal.Models.RepairOrderGridForm;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// Базовый контроллер для гридов devexpress.
    /// </summary>
    [Authorize]
    public abstract class GridControllerBase<TKey, TGridModel, TCreateModel, TEditModel> : BaseController,
            ICrudController
        where TKey : struct
        where TGridModel : GridModelBase
        where TCreateModel : GridDataModelBase<TKey>
        where TEditModel : GridDataModelBase<TKey>
    {
        /// <summary>
        /// Текущий логер.
        /// </summary>
        protected static ILog _logger = LogManager.GetLogger("GridController");

        /// <summary>
        /// Содержит адаптер данных.
        /// </summary> 
        private readonly DataAdapterBase<TKey, TGridModel, TCreateModel, TEditModel> _dataAdapter;

        protected GridControllerBase(DataAdapterBase<TKey, TGridModel, TCreateModel, TEditModel> dataAdapter)
        {
            _dataAdapter = dataAdapter;
        }

        /// <summary>
        /// Метод указывающий необходимо ли использовать лайаут по-умолчанию.
        /// </summary>
        /// <returns>Признак использования лайаута по-умолчанию.</returns>
        public override bool UseDefaultLayout()
        {
            return false;
        }

        /// <summary>
        /// Получает название контроллера.
        /// </summary>
        /// <returns>Название контроллера.</returns>
        protected abstract string GetControllerName();

        /// <summary>
        /// Получает название грида.
        /// </summary>
        /// <returns>Название грида.</returns>
        protected abstract string GetGridName();

        /// <summary>
        /// Содержит наименование представления с гридом.
        /// </summary>
        private const string GridViewName = "grid";

        [ValidateInput(false)]
        public ActionResult GetGrid(string parentId = null)
        {
            TGridModel model = null;
            try
            {
                model = _dataAdapter.CreateGridModel(GetToken());
                model.ControllerName = GetControllerName();
                model.GridName = GetGridName();
                model.Model = GetModel(parentId);
                model.GridObjectName = GetGridObjectName();
                model.Token = GetToken();
                model.ParentId = parentId;
                //var command = Request.Params["command"];
            }
            catch (Exception ex)
            {
                var innerException = string.Empty;
                if (ex.InnerException != null)
                {
                    innerException = ex.InnerException.Message;
                } //if

                _logger.ErrorFormat("Во время получения данных для грида {0} произошла ошибка {1} {2} {3} {4}",
                    GetType().Name, ex.Message, ex.GetType(), innerException, ex.StackTrace);
            }
            return PartialView(GridViewName, model);
        }

        /// <summary>
        /// Получает название объекта для передачи данных.
        /// </summary>
        /// <returns></returns>
        public string GetGridObjectName()
        {
            return GetGridName() + "_" + "object";
        }

        private IQueryable GetModel(string parentId)
        {
            return _dataAdapter.GedData(GetToken(), parentId);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditFormTemplateAddNewPartial(TCreateModel objectToCreate, string parentId = null)
        {
            _dataAdapter.ProcessCreateUnboundItems(objectToCreate, parentId, ModelState);
            if (ModelState.IsValid)
            {
                try
                {
                    //d.InsertProduct(objectToCreate);
                    var modelResult = new GridSaveModelResult();
                    _dataAdapter.SaveCreateModel(GetToken(), objectToCreate, modelResult);
                    CheckModelError(modelResult, objectToCreate);

                }
                catch (Exception ex)
                {
                    ViewData["EditError"] = ex.Message;
                    var innerException = string.Empty;
                    if (ex.InnerException != null)
                    {
                        innerException = ex.InnerException.Message;
                    } //if

                    _logger.ErrorFormat("Во время сохранения пункта {0} произошла ошибка {1} {2} {3} {4}",
                        GetType().Name, ex.Message, ex.GetType(), innerException, ex.StackTrace);
                    ViewData[GetGridObjectName()] = objectToCreate;
                }
            }
            else
            {
                ViewData["EditError"] = "Пожалуйста, исправьте все ошибки.";
                ViewData[GetGridObjectName()] = objectToCreate;
            }
            return GetGrid(parentId);
        }

        private void CheckModelError(GridSaveModelResult modelResult, object editedObject)
        {
            if (modelResult.HasModelError)
            {
                var builder = new StringBuilder();
                foreach (var modelError in modelResult.ModelErrors)
                {
                    ModelState.AddModelError(modelError.FirstItem, modelError.SecondItem);
                    builder.AppendFormat("{0};", modelError.SecondItem);
                } //foreach
                ViewData["EditError"] = builder.ToString();
                ViewData[GetGridObjectName()] = editedObject;
            } //if
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditFormTemplateUpdatePartial(TEditModel objectToEdit, string parentId = null)
        {
            _dataAdapter.ProcessEditUnboundItems(objectToEdit, parentId, ModelState);
            if (ModelState.IsValid)
            {
                try
                {
                    var modelResult = new GridSaveModelResult();
                    _dataAdapter.SaveEditModel(GetToken(), objectToEdit, modelResult);
                    CheckModelError(modelResult, objectToEdit);
                }
                catch (Exception ex)
                {
                    ViewData["EditError"] = ex.Message;
                    var innerException = string.Empty;
                    if (ex.InnerException != null)
                    {
                        innerException = ex.InnerException.Message;
                    } //if

                    _logger.ErrorFormat("Во время редактирования пункта {0} произошла ошибка {1} {2} {3} {4}",
                        GetType().Name, ex.Message, ex.GetType(), innerException, ex.StackTrace);
                }
            }
            else
            {
                ViewData["EditError"] = "Пожалуйста, исправьте все ошибки.";
                ViewData[GetGridObjectName()] = objectToEdit;
            }

            return GetGrid(parentId);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditFormTemplateDeletePartial(string parentId = null)
        {
            TKey? entityId = null;

            var gridModel = _dataAdapter.CreateGridModel(GetToken());

            var param = Request.Params[gridModel.KeyFieldName];

            if (param != null)
            {
                param = param.Trim('"');
                var value = TypeDescriptor.GetConverter(typeof(TKey)).ConvertFromInvariantString(param);
                if (value != null)
                {
                    entityId = (TKey) value;
                }
            }

            if (entityId.HasValue)
            {
                try
                {
                    _dataAdapter.DeleteEntity(GetToken(), entityId);
                }
                catch (Exception ex)
                {
                    var innerException = string.Empty;
                    if (ex.InnerException != null)
                    {
                        innerException = ex.InnerException.Message;
                    } //if

                    _logger.ErrorFormat("Во время удаления сущности {0} с ключем {1} произошла ошибка {2} {3} {4} {5}",
                        GetType().Name, entityId, ex.Message, ex.GetType(), innerException, ex.StackTrace);
                    ViewData["EditError"] = ex.Message;
                }
            }
            return GetGrid(parentId);
        }

        /// <summary>
        /// Создает модель данных для передачи в представление создания сущности.
        /// </summary>
        /// <param name="formLayoutSettings">Настройки лайаута devexpress.</param>
        /// <param name="dependedModel">Зависимая модель.</param>
        /// <param name="gridModel">Модель грида.</param> 
        /// <returns>Модель.</returns>
        object ICrudController.CreateNewEditSettingsModel(object formLayoutSettings, object dependedModel,
            object gridModel, object html)
        {
            var model = new GridEditSettingModel<TKey, TGridModel, TCreateModel>();
            model.LayoutSettings = (FormLayoutSettings<TGridModel>) formLayoutSettings;
            model.GridSettings = (TGridModel) gridModel;
            if (dependedModel == null)
            {
                model.Model = (TCreateModel) ((ICrudController) this).CreateNewModel(dependedModel, gridModel);
            }
            else
            {
                model.Model = (TCreateModel) dependedModel;
            }
            model.Html = (HtmlHelper<TGridModel>) html;
            return model;
        }

        /// <summary>
        /// Создает новую модель.
        /// </summary>
        object ICrudController.CreateNewModel(object dependedModel, object gridModel)
        {
            var grid = (TGridModel) gridModel;
            var model = _dataAdapter.CreateNewModel(GetToken(), new GridCreateParameters
            {
                ParentId = grid.ParentId
            });

            if (dependedModel!=null)
            {
                PropetiesCopier.CopyPropertiesTo(dependedModel, model);
            }

            return model;
        }


        /// <summary>
        /// Создает модель данных для передачи в представление обновления сущности.
        /// </summary>
        /// <param name="formLayoutSettings">Настройки лайаута devexpress.</param>
        /// <param name="dependedModel">Зависимая модель.</param>
        /// <param name="gridModel">Модель грида.</param>
        /// <returns>Модель.</returns>
        object ICrudController.CreateUpdateEditSettingsModel(object formLayoutSettings, object dependedModel,
            object gridModel, object html)
        {
            var model = new GridEditSettingModel<TKey, TGridModel, TEditModel>();
            model.LayoutSettings = (FormLayoutSettings<TGridModel>) formLayoutSettings;
            model.GridSettings = (TGridModel) gridModel;
            model.Model = (TEditModel) dependedModel;
            model.Html = (HtmlHelper<TGridModel>) html;
            return model;
        }

        /// <summary>
        /// Подготовливает модель грида для упаковки в состояние представления.
        /// </summary>
        /// <param name="dependedModel">Зависимая модель.</param>
        /// <param name="gridModel">Модель грида.</param>
        /// <returns>Подготовленая модель.</returns>
        object ICrudController.PrepareUpdateEditModel(object dependedModel, object gridModel)
        {
            var gm = (TGridModel) gridModel;
            
            if (dependedModel != null)
            {
                if (typeof(TEditModel) == dependedModel.GetType())
                {
                    return (TEditModel) dependedModel;
                }
                WebDataRow row = (WebDataRow) dependedModel;

                var id = row[gm.KeyFieldName] as TKey?;
                return _dataAdapter.CreateEditModel(GetToken(), id, gm, new GridCreateParameters
                {
                    ParentId = gm.ParentId
                });
            }
            return _dataAdapter.CreateEditModel(GetToken(), null, gm, new GridCreateParameters
            {
                ParentId = gm.ParentId
            });
        }

        /// <summary>
        /// Возвращает название представления для создания сущности.
        /// </summary>
        /// <returns>Название представления.</returns>
        string ICrudController.GetCreateViewName()
        {
            return DefaultCreateViewName;
        }

        /// <summary>
        /// Возвращает название представления для редактирования сущности.
        /// </summary>
        /// <returns>Название представления.</returns>
        string ICrudController.GetUpdateViewName()
        {
            if (typeof(TCreateModel) == typeof(TEditModel))
            {
                return DefaultCreateViewName;
            }
            return DefaultUpdateViewName;
        }

        /// <summary>
        /// Содержит дефолтное значение представления для создания новой сущности.
        /// </summary>
        private const string DefaultCreateViewName = "create";

        /// <summary>
        /// Содержит дефолтное значение названия представления для редактирования сущности.
        /// </summary>
        private const string DefaultUpdateViewName = "edit";

        #region Filters

        /// <summary>
        /// Callback Для комбобокса с фильтрами.
        /// </summary>
        /// <returns>Результат.</returns>
        public ActionResult GetFilters()
        {
            var model = new FiltersComboBoxModel();
            model.ControllerName = GetControllerName();
            model.GridName = GetGridName();
            model.Token = GetToken();
            model.UserGridFilters = new List<UserGridFilter>();

            try
            {
                InitializeCustomFilters(model.Token, model.UserGridFilters);
                var savedFilters = RemontinkaServer.Instance.EntitiesFacade.GetUserGridFilters(model.Token,
                    GetGridName());
                foreach (var userGridFilter in savedFilters)
                {
                    model.UserGridFilters.Add(userGridFilter);
                }
            }
            catch (Exception ex)
            {
                var innerException = string.Empty;
                if (ex.InnerException != null)
                {
                    innerException = ex.InnerException.Message;
                } //if
                _logger.ErrorFormat("Во время получения фильтров для грида {0} {1} произошла ошибка {2} {3} {4} {5}",
                    GetType().Name, model.GridName, ex.Message, ex.GetType(), innerException, ex.StackTrace);
            }
            return PartialView("filtersComboBox", model);
        }

        /// <summary>
        /// Получает фильтр по определенному Id.
        /// </summary>
        /// <returns>Результат.</returns>
        [HttpPost]
        public ActionResult GetFilter(Guid? userFilterId)
        {
            try
            {
                var token = GetToken();
                var filter = RemontinkaServer.Instance.EntitiesFacade.GetUserGridFilter(token, userFilterId);
                if (filter == null)
                {
                    var list = new List<UserGridFilter>();
                    InitializeCustomFilters(token, list);

                    filter = list.FirstOrDefault(i => i.UserGridFilterID == userFilterId);
                }

                if (filter == null)
                {
                    _logger.ErrorFormat("При получении фильтра по ID {0} для грида {1} пользователем {2},фильтр не найден",userFilterId,GetGridName(),token.LoginName);
                    filter = new UserGridFilter();
                    filter.FilterData = string.Empty;
                }

                return JCrudData(filter.FilterData);
            }
            catch (Exception ex)
            {
                var innerException = string.Empty;
                if (ex.InnerException != null)
                {
                    innerException = ex.InnerException.Message;
                } //if
                _logger.ErrorFormat("Во время получения фильтра для грида {0} {1} произошла ошибка {2} {3} {4} {5}",
                    GetType().Name, userFilterId, ex.Message, ex.GetType(), innerException, ex.StackTrace);
                return JCrudError("Ошибка получения данных по фильтру");
            }
        }

        /// <summary>
        /// Возвращает Popup для присвоение имени фильтра.
        /// </summary>
        /// <returns>Результат.</returns>
        public ActionResult GetFilterNamePopup()
        {
            var model = new FilterNamePopupModel();
            model.PopupName = GetGridName() + "FilterNamePopup";
            model.Token = GetToken();
            model.ControllerName = GetControllerName();
            model.GridName = GetGridName();

            return PartialView("filterNamePopup", model);
        }

        /// <summary>
        /// Инициализация фильтров пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="filters">Фильтры.</param>
        protected virtual void InitializeCustomFilters(SecurityToken token, IList<UserGridFilter> filters)
        {
            
        }

        /// <summary>
        /// Сохраняет текущий фильтр.
        /// </summary>
        /// <param name="filterData">Данные фильтра.</param>
        /// <param name="filterName">Имя фильтра.</param>
        /// <returns>Значение</returns>
        [HttpPost]
        public ActionResult SaveCurrentFilter(string filterData, string filterName)
        {
            try
            {
                var token = GetToken();
                _logger.InfoFormat("Поступил запрос на сохранение фильтра для грида {0} {1} от пользователя {2}",GetGridName(),filterName,token.LoginName);
                if (string.IsNullOrWhiteSpace(filterName))
                {

                    _logger.ErrorFormat("При сохранении фильтра произошла ошибка: не задано имя фильтра {0},{1} ",token.LoginName,filterData);
                    return JCrudError("Не задано имя фильтра");
                }

                if (string.IsNullOrWhiteSpace(filterData))
                {

                    _logger.ErrorFormat("При сохранении фильтра произошла ошибка: фильтр пустой {0},{1} ", token.LoginName, filterName);
                    return JCrudError("Фильтр пустой");
                }

                var filter = new UserGridFilter();
                filter.UserID = token.User.UserID;
                filter.Title = filterName;
                filter.FilterData = filterData;
                filter.GridName = GetGridName();
                RemontinkaServer.Instance.EntitiesFacade.SaveUserGridFilter(token, filter);
                return JCrud(new JCrudResult {ResultState = CrudResultKind.Success});
            }
            catch (Exception ex)
            {

                var innerException = string.Empty;
                if (ex.InnerException != null)
                {
                    innerException = ex.InnerException.Message;
                } //if

                _logger.ErrorFormat("Во время сохранения фильтра для грида {0} {1} произошла ошибка {2} {3} {4} {5}",filterData,GetGridName(), ex.Message, ex.GetType(), innerException, ex.StackTrace);

                return JCrudError(ex.Message);
            }
        }

        #endregion Filters
    }
}