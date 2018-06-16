using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Remontinka.Client.Core;
using Remontinka.Client.Wpf.Controllers.Controls;
using Remontinka.Client.Wpf.Controls;
using Remontinka.Client.Wpf.Model.Controls;

namespace Remontinka.Client.Wpf.Controllers.Items
{
    /// <summary>
    /// Базовый контролер для управления пунктами выбора комбобокса.
    /// </summary>
    public abstract class ComboBoxItemControllerBase<T> : IComboBoxItemController
        where T : struct
    {
        /// <summary>
        /// Контроллер комбобокса.
        /// </summary>
        private ComboBoxController _controller;

        /// <summary>
        /// Возвращает контроллер комбобокса.
        /// </summary>
        /// <returns>Контроллер.</returns>
        public ComboBoxController GetController()
        {
            return _controller;
        }

        /// <summary>
        /// Устанавливает представление комбобокса.
        /// </summary>
        /// <param name="comboBox">Объект представления.</param>
        /// <param name="allowNull">Допустимость Null значений. </param>
        /// <param name="showNullValue">Показывать ли стандартное Null значение. </param>
        public void SetView(ComboBoxControl comboBox, bool allowNull, bool showNullValue)
        {
            SetView(comboBox, default(T), allowNull, showNullValue);
        }

        /// <summary>
        /// Устанавливает представление комбобокса.
        /// </summary>
        /// <param name="comboBox">Объект представления.</param>
        /// <param name="id">Код выбранного пункта.</param>
        /// <param name="allowNull">Допустимость Null значений. </param>
        /// <param name="showNullValue">Показывать ли стандартное Null значение. </param>
        public void SetView(ComboBoxControl comboBox, T? id, bool allowNull,bool showNullValue)
        {
            
            _controller = new ComboBoxController();
            _controller.SetView(comboBox);
            SetUpModel(_controller.Model);

            _controller.Model.AllowNull = allowNull;
            _controller.Model.ShowNullValue = showNullValue;
            ReInitialize(id);
        }

        /// <summary>
        /// Реинициализация значений на основе выбранного пункта.
        /// </summary>
        public void ReInitialize(object value)
        {
            var id = (T?)value;
            ReInitialize(id);
            _controller.Model.RawValue = value;
        }

        /// <summary>
        /// Реинициализация пунктов комбобокса.
        /// </summary>
        /// <param name="id">Код выбранного пункта.</param>
        public void ReInitialize(T? id)
        {
            var items = GetList();
            GetInitializedItems(ClientCore.Instance.AuthService.SecurityToken, items, id);
            _controller.Model.ComboboxItems.Clear();
            _controller.Model.RawValue = null;
            if (items.Any())
            {
                foreach (var selectListItem in items)
                {
                    _controller.Model.ComboboxItems.Add(new ComboBoxItem { Title = selectListItem.Text, Value = selectListItem.Value });
                }

                _controller.Model.RawValue = id;
            }
        }

        /// <summary>
        /// Устанавливает представление комбобокса.
        /// </summary>
        /// <param name="comboBox">Объект представления.</param>
        /// <param name="id">Код выбранного пункта.</param>
        public void SetView(ComboBoxControl comboBox,T? id)
        {
            SetView(comboBox, id, false, true);
        }

        /// <summary>
        /// Возвращает выбранное значение.
        /// </summary>
        public T? SelectedValue
        {
            get { return (T?)_controller.Model.RawValue; }
        }

        /// <summary>
        /// Устанавливает значение.
        /// </summary>
        /// <param name="id">Код значения.</param>
        public void SetSelectedValue(T? id)
        {
            _controller.Model.RawValue = id;
        }

        /// <summary>
        /// Возвращает инициализированный список пунктов.
        /// </summary>
        /// <returns>Список</returns>
        private List<SelectListItem<T>> GetList()
        {
            return new List<SelectListItem<T>>();
        }

        /// <summary>
        /// Производит установку модели.
        /// </summary>
        /// <param name="model">Настраеваемая модель.</param>
        protected abstract void SetUpModel(ComboBoxControlModel model);

        /// <summary>
        /// Переопределяется для получения переопределенного списка с пунктами.
        /// </summary>
        /// <param name="token">Текущий токен безопасности. </param>
        /// <param name="list">Список который необходимо инициализировать.</param>
        /// <param name="selectedId"> Выбранный пункт.</param>
        protected abstract void GetInitializedItems(SecurityToken token, List<SelectListItem<T>> list, T? selectedId);
    }
}
