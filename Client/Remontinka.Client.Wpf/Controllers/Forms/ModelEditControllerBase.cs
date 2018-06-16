using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Remontinka.Client.Core;
using Remontinka.Client.Wpf.Model;
using Remontinka.Client.Wpf.Model.Controls;
using Remontinka.Client.Wpf.Model.Forms;
using Remontinka.Client.Wpf.View;
using Xceed.Wpf.Toolkit;
using log4net;

namespace Remontinka.Client.Wpf.Controllers.Forms
{
    /// <summary>
    /// Базовый класс для контроллеров управления сущностями.
    /// </summary>
    public abstract class ModelEditControllerBase<TEditView,TCreateView,TEditModel,TCreateModel, TKey, TParametersModel>
         where TKey:struct
        where TEditModel : ViewModelBase<TKey>, new()
        where TCreateModel : ViewModelBase<TKey>, new()
        where TEditView: UserControl,new()
        where TCreateView: UserControl,new()
    {
        /// <summary>
        ///   Текущий логер.
        /// </summary>
        private readonly ILog _logger = LogManager.GetLogger(typeof(ModelEditControllerBase<TEditView, TCreateView, TEditModel, TCreateModel, TKey, TParametersModel>));

        /// <summary>
        /// Содержит инстанс окна создания.
        /// </summary>
        private EditModelMasterWindow _createWindow;

        /// <summary>
        /// Модель для окна создания сущности.
        /// </summary>
        private EditModelMasterModel _createModelMasterModel;

        /// <summary>
        /// Содержит инстанс окна редактирования.
        /// </summary>
        private EditModelMasterWindow _editWindow;

        /// <summary>
        /// Модель для окна редактирования сущности.
        /// </summary>
        private EditModelMasterModel _editModelMasterModel;

        /// <summary>
        /// Содержит инстанс управления формой создания модели.
        /// </summary>
        private FormController<TCreateModel, TKey> _createController;

        /// <summary>
        /// Содержит инстанс управления формой редактирования модели.
        /// </summary>
        private FormController<TEditModel, TKey> _editController;

        /// <summary>
        /// Инициализация контейнеров.
        /// </summary>
        public void Initialize()
        {
            _createWindow = new EditModelMasterWindow();
            _createModelMasterModel = ClientCore.Instance.CreateInstance<EditModelMasterModel>();
            _createModelMasterModel.CancelButtonText = "Отмена";
            _createModelMasterModel.CancelButtonEnabled = true;
            _createModelMasterModel.InfoTextIsRed = false;
            _createModelMasterModel.SaveButtonEnabled = false;
            _createModelMasterModel.SaveButtonText = "Создать";
            _createWindow.cancelButton.Click += (sender, args) => _createWindow.Close();
            _createWindow.saveButton.Click += (sender, args) => SaveCreateModel();
            _createWindow.DataContext = _createModelMasterModel;

            var createView = new TCreateView();
            _createController = new FormController<TCreateModel, TKey>();
            
            _createController.SetView(createView);
            _createWindow.contentPlaceholder.Children.Add(createView);
            _createController.ModelIsValidStateChanged += (sender, args) => UpdateCreateViewButtonState(args.IsValid);
            
            _editWindow = new EditModelMasterWindow();
            _editModelMasterModel = ClientCore.Instance.CreateInstance<EditModelMasterModel>();
            _editModelMasterModel.CancelButtonEnabled = true;
            _editModelMasterModel.CancelButtonText = "Отмена";
            _editModelMasterModel.SaveButtonText = "Сохранить";
            _editModelMasterModel.SaveButtonEnabled = false;
            _editModelMasterModel.InfoTextIsRed = false;
            _editWindow.cancelButton.Click += (sender, args) => _editWindow.Close();
            _editWindow.saveButton.Click += (sender, args) => SaveEditModel();
            _editWindow.DataContext = _editModelMasterModel;

            var editView = new TEditView();
            _editController = new FormController<TEditModel, TKey>();
            _editController.SetView(editView);
            _editWindow.contentPlaceholder.Children.Add(editView);
            _editController.ModelIsValidStateChanged += (sender, args) => UpdateEditViewButtonState(args.IsValid);
        }

        /// <summary>
        /// Старт окна создания модели.
        /// </summary>
        /// <param name="parameters">Параметры для передачи.</param>
        public void StartCreateModel(TParametersModel parameters)
        {
            var model = CreateNewModel(ClientCore.Instance.AuthService.SecurityToken, parameters);
            _createModelMasterModel.InfoText = string.Empty;
            _createController.SetModelValues(model);
            _createWindow.WindowState = WindowState.Closed;
            _createWindow.IsModal = true;
            _createWindow.WindowStartupLocation = WindowStartupLocation.Center;
            ArmController.Instance.SetChildWindow(_createWindow);
            _createWindow.Show();
            _createController.CheckModelValid(true);
        }

        /// <summary>
        /// Старт окна редактирования модели.
        /// </summary>
        /// <param name="entityId">Код сущности.</param>
        /// <param name="parameters">Параметры.</param>
        public void StartEditModel(TKey entityId, TParametersModel parameters)
        {
            var model = CreateEditedModel(ClientCore.Instance.AuthService.SecurityToken, entityId, parameters);
            _editModelMasterModel.InfoText = string.Empty;
            _editController.SetModelValues(model);
            _editWindow.WindowState = WindowState.Closed;
            _editWindow.IsModal = true;
            _editWindow.WindowStartupLocation = WindowStartupLocation.Center;
            ArmController.Instance.SetChildWindow(_editWindow);
            _editWindow.Show();
            _editController.CheckModelValid(true);
        }

        /// <summary>
        /// Происходит после сохранения новой модели.
        /// </summary>
        public event EventHandler<ModelSavedEventArgs<TCreateModel, TKey>> CreateModelSaved;

        /// <summary>
        /// Вызывает событие успешности сохранения новой модели.
        /// </summary>
        /// <param name="model">Сохраненная модель.</param>
        private void RiseCreateModelSaved(TCreateModel model)
        {
            EventHandler<ModelSavedEventArgs<TCreateModel, TKey>> handler = CreateModelSaved;
            if (handler != null)
            {
                handler(this, new ModelSavedEventArgs<TCreateModel, TKey>(model));
            }
        }

        /// <summary>
        /// Происходит после сохранения редактируемой модели.
        /// </summary>
        public event EventHandler<ModelSavedEventArgs<TEditModel, TKey>> EditModelSaved;

        /// <summary>
        /// Вызывает событие успешности сохранения редактируемой модели.
        /// </summary>
        /// <param name="model">Сохраненная модель.</param>
        private void RiseEditModelSaved(TEditModel model)
        {
            EventHandler<ModelSavedEventArgs<TEditModel, TKey>> handler = EditModelSaved;
            if (handler != null)
            {
                handler(this, new ModelSavedEventArgs<TEditModel, TKey>(model));
            }
        }

        /// <summary>
        /// Обновление состояний кнопок для формы редактирования.
        /// </summary>
        private void UpdateEditViewButtonState(bool modelIsValid)
        {
            _editModelMasterModel.SaveButtonEnabled = modelIsValid;
        }

        /// <summary>
        /// Обновление состояний кнопок для формы создания.
        /// </summary>
        private void UpdateCreateViewButtonState(bool modelIsValid)
        {
            _createModelMasterModel.SaveButtonEnabled = modelIsValid;
        }

        /// <summary>
        /// Сохраняет новую модель.
        /// </summary>
        private void SaveCreateModel()
        {
            _logger.InfoFormat("Страт сохранения новой сущности типа {0}",typeof(TCreateModel));
            if (_createController.IsValid)
            {
                try
                {
                    var model = _createController.GetModelValues();
                    var modelResult = new SaveModelResult();

                    SaveCreateModel(ClientCore.Instance.AuthService.SecurityToken, model, modelResult);

                    if (modelResult.HasModelError)
                    {
                        _createModelMasterModel.InfoTextIsRed = true;
                        var builder = new StringBuilder();

                        foreach (var modelError in modelResult.ModelErrors)
                        {
                            builder.AppendFormat("{0}: {1}{2}", modelError.FirstItem, modelError.SecondItem,
                                                 Environment.NewLine);
                        } //foreach

                        _createModelMasterModel.InfoText = builder.ToString();

                    } //if
                    else
                    {
                        _createModelMasterModel.InfoText = "Успешно сохранено";
                        _createModelMasterModel.InfoTextIsRed = false;
                        _logger.InfoFormat("Успех сохранения новой сущности типа {0} с ID={1}", typeof(TCreateModel), model.Id);
                        RiseCreateModelSaved(model);
                        _createWindow.Close();
                    } //else
                }
                catch (Exception ex)
                {
                    var innerException = string.Empty;
                    if (ex.InnerException != null)
                    {
                        innerException = ex.InnerException.Message;
                    } //if

                    _logger.ErrorFormat("Во время сохранение новой сущности {0} произошла ошибка {1} {2} {3} {4}",
                                        typeof(TCreateModel), ex.Message, ex.GetType(), innerException, ex.StackTrace);
                } //try
            } //if
            else
            {
                _logger.InfoFormat("Модель сущности не валидна {0}", typeof(TCreateModel));
            } //else
        }

        /// <summary>
        /// Сохраняет новую модель.
        /// </summary>
        private void SaveEditModel()
        {
            _logger.InfoFormat("Страт сохранения редактируемой сущности типа {0}", typeof(TEditModel));
            if (_editController.IsValid)
            {
                try
                {
                    var model = _editController.GetModelValues();
                    var modelResult = new SaveModelResult();

                    SaveEditModel(ClientCore.Instance.AuthService.SecurityToken, model, modelResult);

                    if (modelResult.HasModelError)
                    {
                        _editModelMasterModel.InfoTextIsRed = true;
                        var builder = new StringBuilder();

                        foreach (var modelError in modelResult.ModelErrors)
                        {
                            builder.AppendFormat("{0}: {1}{2}", modelError.FirstItem, modelError.SecondItem,
                                                 Environment.NewLine);
                        } //foreach

                        _editModelMasterModel.InfoText = builder.ToString();

                    } //if
                    else
                    {
                        _editModelMasterModel.InfoText = "Успешно сохранено";
                        _editModelMasterModel.InfoTextIsRed = false;
                        _logger.InfoFormat("Успех сохранения редактируемой сущности типа {0} с ID={1}", typeof(TCreateModel), model.Id);
                        RiseEditModelSaved(model);
                        _editWindow.Close();
                    } //else
                }
                catch (Exception ex)
                {
                    var innerException = string.Empty;
                    if (ex.InnerException != null)
                    {
                        innerException = ex.InnerException.Message;
                    } //if

                    _logger.ErrorFormat("Во время сохранение новой сущности {0} произошла ошибка {1} {2} {3} {4}",
                                        typeof(TCreateModel), ex.Message, ex.GetType(), innerException, ex.StackTrace);
                } //try
            } //if
            else
            {
                _logger.InfoFormat("Модель сущности не валидна {0}", typeof(TEditModel));
            } //else
        }

        /// <summary>
        /// Возвращает модель контрола контроллера формы создания модели.
        /// </summary>
        /// <param name="controlID">Код контрола.</param>
        /// <returns>Модель контрола, если найдена.</returns>
        public ControlModelBase GetCreateFormControlModel(string controlID)
        {
            return
                _createController.GetModel(controlID);
        }

        /// <summary>
        /// Возвращает все модели контролов контроллера создания модели.
        /// </summary>
        /// <returns>Модели контролов.</returns>
        public IEnumerable<ControlModelBase> GetCreateFormControlModels()
        {
            return
                _createController.GetModels();
        }

        /// <summary>
        /// Возвращает модель контрола контроллера формы редактирования модели.
        /// </summary>
        /// <param name="controlID">Код контрола.</param>
        /// <returns>Модель контрола, если найдена.</returns>
        public ControlModelBase GetEditFormControlModel(string controlID)
        {
            return
                _editController.GetModel(controlID);
        }

        /// <summary>
        /// Возвращает все модели контролов контроллера редактирования модели.
        /// </summary>
        /// <returns>Модели контролов.</returns>
        public IEnumerable<ControlModelBase> GetEditFormControlModels()
        {
            return
                _editController.GetModels();
        }

        /// <summary>
        /// Создает новую модель создания сущности.
        /// </summary>
        /// <param name="parameters">Модель параметров.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель.</returns>
        public abstract TCreateModel CreateNewModel(SecurityToken token, TParametersModel parameters);

        /// <summary>
        /// Создает модель редактирования из сущности.
        /// </summary>
        /// <param name="entityId">Код сущности.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="parameters">Модель параметров.</param>
        /// <returns>Созданная модель редактирования.</returns>
        public abstract TEditModel CreateEditedModel(SecurityToken token, TKey entityId, TParametersModel parameters);

        /// <summary>
        /// Удаляет из хранилища сущность.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код сущности.</param>
        public abstract void DeleteEntity(SecurityToken token, TKey entityId);

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public abstract void SaveCreateModel(SecurityToken token, TCreateModel model,SaveModelResult result);

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public abstract void SaveEditModel(SecurityToken token, TEditModel model, SaveModelResult result);

        /// <summary>
        /// Вызывает исключение при не нахождении определенного элемента.
        /// </summary>
        /// <param name="instance">Элемент который необходимо проверить.</param>
        /// <param name="id">Код элемента.</param>
        /// <param name="name">Наименование элемента.</param>
        protected static void RiseExceptionIfNotFound(object instance, TKey? id, string name)
        {
            if (instance == null)
            {
                //TODO определить специальное исключение с перехватом.
                throw new Exception(string.Format("Для сущности {0} не найден id {1}", name, id));
            }
        }
    }
}
