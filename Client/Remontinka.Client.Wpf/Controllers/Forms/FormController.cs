using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using Remontinka.Client.Wpf.Controllers.Controls;
using Remontinka.Client.Wpf.Controllers.Items;
using Remontinka.Client.Wpf.Controls;
using Remontinka.Client.Wpf.Model.Controls;
using Remontinka.Client.Wpf.Model.Forms;
using DisplayNameAttribute = Remontinka.Client.Wpf.Model.Controls.DisplayNameAttribute;

namespace Remontinka.Client.Wpf.Controllers.Forms
{
    /// <summary>
    /// Базовый класс для форм моделей.
    /// </summary>
    public class FormController<TModel,TKey>
        where TKey:struct
        where TModel : ViewModelBase<TKey>,new()
    {
        /// <summary>
        /// Содержит представления для привязки модели. 
        /// </summary>
        private UserControl _view;

        /// <summary>
        /// Содержит текущие контроллеры управления моделью.
        /// </summary>
        private readonly IList<IUserControlController> _modelControllers =
            new List<IUserControlController>();

        /// <summary>
        /// Хранимые данные модели.
        /// </summary>
        private readonly IDictionary<string,object> _modelData = new Dictionary<string, object>();

        /// <summary>
        /// Содержит идентификатор ключа.
        /// </summary>
        private TKey? _id;

        /// <summary>
        /// Текущий менеджер табуляции.
        /// </summary>
        private TabControlManager _tabControlManager;

        /// <summary>
        /// Установки представления.
        /// </summary>
        /// <param name="control">Контрол с именнованными полями.</param>
        public void SetView(UserControl control)
        {
            if (control==null)
            {
                throw new ArgumentNullException("control");
            }

            _id = null;

            //отписываемся
            foreach (var userControlController in _modelControllers)
            {
                userControlController.GetModel().PropertyChanged -= BindedModelOnPropertyChanged;
                userControlController.GetModel().PressKey -= BindedModelOnPressKey;
            }
            _tabControlManager = new TabControlManager();

            _modelControllers.Clear();
            _modelData.Clear();
            _comboBoxItems.Clear();

            _lastValid = false;

            _view = control;

            //ищем все прорерти
            foreach (var propertyInfo in typeof(TModel).GetProperties())
            {
                ProcessProperty(propertyInfo);
            }

            CheckModelValid(true);
        }

        /// <summary>
        /// Биндит значения определенной модели.
        /// </summary>
        /// <param name="model">Модель содержащая значения для установки значений.</param>
        public void SetModelValues(TModel model)
        {
            _id = model.Id;

            foreach (var userControlController in _modelControllers)
            {
                
                var property = typeof(TModel).GetProperty(userControlController.GetModel().Id);
                if (property!=null)
                {
                    var value = property.GetValue(model, null);
                    userControlController.GetModel().RawValue = value;
                    //для комбобокса производим реинициализацию значений
                    if (userControlController is ComboBoxController)
                    {
                        var combobox = _comboBoxItems[userControlController.GetModel().Id];
                        combobox.ReInitialize(value);

                    }
                }
                
            }

            foreach (var key in _modelData.Keys.ToList())
            {
                var property = typeof (TModel).GetProperty(key);

                if (property!=null)
                {
                    _modelData[key] = property.GetValue(model, null);
                } //if
            } //foreach
        }

        /// <summary>
        /// Получает введеные значения из всех контролов.
        /// </summary>
        /// <returns>Значение модели.</returns>
        public TModel GetModelValues()
        {
            var result = new TModel();
            result.Id = _id;

            foreach (var userControlController in _modelControllers)
            {
                var property = typeof(TModel).GetProperty(userControlController.GetModel().Id);
                if (property != null)
                {
                    property.SetValue(result, userControlController.GetModel().RawValue, null);
                }
            }

            foreach (var key in _modelData.Keys)
            {
                var property = typeof(TModel).GetProperty(key);

                if (property != null)
                {
                    property.SetValue(result, _modelData[key], null);
                } //if
            } //foreach

            return result;
        }

        /// <summary>
        /// Получает текущий менеджер табуляций.
        /// </summary>
        /// <returns>Менеджер табуляций.</returns>
        public TabControlManager GetCurrentTabManager()
        {
            return _tabControlManager;
        }

        /// <summary>
        /// Возвращает модель определенного контроллера.
        /// </summary>
        /// <param name="controlID">Код контрола.</param>
        /// <returns>Модель контроллера, если найдена.</returns>
        public ControlModelBase GetModel(string controlID)
        {
            return
                _modelControllers.Where(i => StringComparer.OrdinalIgnoreCase.Equals(i.GetModel().Id, controlID)).Select
                    (i => i.GetModel()).FirstOrDefault();
        }

        /// <summary>
        /// Возвращает все модели зависимых контроллеров.
        /// </summary>
        /// <returns>Модели контроллера.</returns>
        public IEnumerable<ControlModelBase> GetModels()
        {
            return
                _modelControllers.Select(i => i.GetModel());
        }

        /// <summary>
        /// Обработка свойства.
        /// </summary>
        /// <param name="propertyInfo">Информация по свойству.</param>
        private void ProcessProperty(PropertyInfo propertyInfo)
        {
            var control = _view.FindName(propertyInfo.Name) as IControlDescriptor;
            if (control!=null)
            {
                BindControl(propertyInfo, control);
            }
            else
            {
                var attr = GetAttribute<ModelDataAttribute>(propertyInfo);
                if (attr!=null)
                {
                    _modelData.Add(propertyInfo.Name, null);
                } //if
            } //else
        }

        /// <summary>
        /// Привязывает контрол к соответствующей модели.
        /// </summary>
        /// <param name="propertyInfo">Информация по связанному свойству.</param>
        /// <param name="control">Контрол.</param>
        private void BindControl(PropertyInfo propertyInfo, IControlDescriptor control)
        {
            IUserControlController controller = null;
            switch (control.ControlType)
            {
                case ControlType.CheckBoxControl:
                    controller =BindCheckBox(propertyInfo, control);
                    break;
                case ControlType.ComboBoxControl:
                    controller = BindComboBox(propertyInfo, control);
                    break;
                case ControlType.DateBoxControl:
                    controller = BindDateBox(propertyInfo, control);
                    break;
                case ControlType.MaskedTextBoxControl:
                    controller = BindMaskedTextBox(propertyInfo, control);
                    break;
                case ControlType.MoneyBoxControl:
                    controller = BindMoneyBox(propertyInfo, control);
                    break;
                case ControlType.TextBoxControl:
                    controller = BindTextBox(propertyInfo, control);
                    break;
                case ControlType.TextBlockControl:
                    break;
            }

            if (controller!=null)
            {
                ProcessFocusable(control, propertyInfo);
                _modelControllers.Add(controller);
            }
        }

        /// <summary>
        /// Биндинг текстового бокса.
        /// </summary>
        private IUserControlController BindTextBox(PropertyInfo propertyInfo, IControlDescriptor control)
        {
            var controller = new TextBoxController();

            controller.SetView((TextBoxControl)control);

            ProcessCommon(controller.Model, propertyInfo);

            var attr = GetAttribute<RegexValueAttribute>(propertyInfo);

            if (attr!=null)
            {
                controller.Model.RegexText = attr.Regex;
            }

            return controller;
        }

        /// <summary>
        /// Биндинг decimal бокса.
        /// </summary>
        private IUserControlController BindMoneyBox(PropertyInfo propertyInfo, IControlDescriptor control)
        {
            var controller = new MoneyBoxController();

            controller.SetView((MoneyBoxControl)control);

            ProcessCommon(controller.Model, propertyInfo);

            return controller;
        }

        /// <summary>
        /// Биндинг масочного бокса.
        /// </summary>
        private IUserControlController BindMaskedTextBox(PropertyInfo propertyInfo, IControlDescriptor control)
        {
            var controller = new MaskedTextBoxController();

            controller.SetView((MaskedTextBoxControl)control);

            ProcessCommon(controller.Model, propertyInfo);

            var attr = GetAttribute<TextBoxMaskAttrubute>(propertyInfo);

            if (attr!=null)
            {
                controller.Model.Mask = attr.Mask;
            }
            return controller;
        }

        /// <summary>
        /// Биндинг даты.
        /// </summary>
        private IUserControlController BindDateBox(PropertyInfo propertyInfo, IControlDescriptor control)
        {
            var controller = new DateBoxController();

            controller.SetView((DateBoxControl)control);

            ProcessCommon(controller.Model, propertyInfo);

            var type = propertyInfo.PropertyType;
            controller.Model.AllowNull = type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>);

            return controller;
        }

        /// <summary>
        /// Здесь хранятся инстансы контроллеров конбобокса.
        /// </summary>
        private readonly IDictionary<string,IComboBoxItemController> _comboBoxItems = new Dictionary<string, IComboBoxItemController>(); 

        /// <summary>
        /// Биндинг комбобокса.
        /// </summary>
        private IUserControlController BindComboBox(PropertyInfo propertyInfo, IControlDescriptor control)
        {
            ComboBoxController controller = null;

            var attr = GetAttribute<ComboBoxControlAttribute>(propertyInfo);
            if (attr!=null)
            {
                var instance = Core.ClientCore.Instance.CreateInstance<IComboBoxItemController>(attr.ControllerType);

                if (instance!=null)
                {
                    instance.SetView((ComboBoxControl) control, attr.AllowNull, attr.ShowNullValue);
                    controller = instance.GetController();

                    ProcessCommon(controller.Model, propertyInfo);
                    ProcessFocusable(control, propertyInfo);
                    _comboBoxItems.Add(propertyInfo.Name, instance);
                }
            }

            return controller;
        }

        /// <summary>
        /// Биндинг check бокса.
        /// </summary>
        private IUserControlController BindCheckBox(PropertyInfo propertyInfo, IControlDescriptor control)
        {
            var controller = new CheckBoxController();

            controller.SetView((CheckBoxControl) control);

            ProcessCommon(controller.Model, propertyInfo);
            ProcessFocusable(control, propertyInfo);
            
            return controller;
        }

        /// <summary>
        /// Обработка фокусных контролов.
        /// </summary>
        /// <param name="control">Контрол для обработки.</param>
        /// <param name="propertyInfo">Информация о свойстве.</param>
        private void ProcessFocusable(IControlDescriptor control, PropertyInfo propertyInfo)
        {
            var focusable = control as IFocusable;

            if (focusable!=null)
            {
                var attr = GetAttribute<TabStepAttribute>(propertyInfo);

                if (attr!=null)
                {
                    _tabControlManager.Add(focusable,attr.Step);
                } //if
            } //if
        }

        /// <summary>
        /// Обработка наименования.
        /// </summary>
        private void ProcessCommon(ControlModelBase model, PropertyInfo propertyInfo)
        {
            var name = GetName(propertyInfo);

            if (!string.IsNullOrWhiteSpace(name))
            {
                model.Title = name;
            }

            model.Id = propertyInfo.Name;

            model.PropertyChanged += BindedModelOnPropertyChanged;
            model.PressKey+=BindedModelOnPressKey;
        }

        /// <summary>
        /// Вызывается при нажатии клавиши на контроле.
        /// </summary>
        private void BindedModelOnPressKey(object sender, PressKeyEventArgs pressKeyEventArgs)
        {
            if (pressKeyEventArgs.Key==Key.Enter)
            {
                _tabControlManager.Next();
            } //if
        }

        /// <summary>
        /// Содержит последнее изменение по валидности.
        /// </summary>
        private bool _lastValid;

        /// <summary>
        /// Возвращает признак валидности формы.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return _lastValid;
            }
        }

        /// <summary>
        /// Проверяет валидацию формы.
        /// </summary>
        /// <param name="forceEvent">признак того, что необходимо все-равно вызвать event.</param>
        public void CheckModelValid( bool forceEvent)
        {
            foreach (var userControlController in _modelControllers)
            {
                userControlController.GetModel().ValidateValue();
            } //foreach

            var valid = _modelControllers.All(i =>

                                              i.GetModel().IsAcceptable
                );
            

            if (valid !=_lastValid)
            {
                _lastValid = valid;
                RiseModelIsValidStateChanged(_lastValid);
            } //if
            else
            {
                if (forceEvent)
                {
                    RiseModelIsValidStateChanged(_lastValid);
                } //if
            } //else
        }



        /// <summary>
        /// Вызывается при изменении свойства.
        /// </summary>
        private void BindedModelOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "IsAcceptable")
            {
                CheckModelValid(false);
            } //if
        }

        /// <summary>
        /// Происходит когда изменяется свойство валидности формы.
        /// </summary>
        public event EventHandler<ModelIsValidStateChangedEventArgs> ModelIsValidStateChanged;

        private void RiseModelIsValidStateChanged(bool isValid)
        {
            EventHandler<ModelIsValidStateChangedEventArgs> handler = ModelIsValidStateChanged;
            if (handler != null)
            {
                handler(this, new ModelIsValidStateChangedEventArgs(isValid));
            }
        }

        /// <summary>
        /// Возвращает наименование для контрола.
        /// </summary>
        /// <param name="propertyInfo">Информация о свойстве.</param>
        /// <returns>Имя.</returns>
        private string GetName(PropertyInfo propertyInfo)
        {
            var result = string.Empty;

            var attr = GetAttribute<DisplayNameAttribute>(propertyInfo);

            if (attr!=null)
            {
                result = attr.Title;
            }

            return result;
        }


        /// <summary>
        /// Получает атрибут типа, если есть.
        /// </summary>
        /// <typeparam name="TAttr">Тип искомого атрибута.</typeparam>
        /// <param name="propertyInfo">Информация о свойстве.</param>
        private TAttr GetAttribute<TAttr>(PropertyInfo propertyInfo)
            where TAttr:Attribute
        {
            var attrs = propertyInfo.GetCustomAttributes(typeof (TAttr), false);

            TAttr result = null; 
            if (attrs.Length>0)
            {

                result = attrs[0] as TAttr;
            }

            return result;
        }
    }
}
