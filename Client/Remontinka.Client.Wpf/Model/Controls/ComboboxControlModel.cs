using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Remontinka.Client.Wpf.Model.Controls
{
    /// <summary>
    /// Модель контрола ComboBox.
    /// </summary>
    public class ComboBoxControlModel : ControlModelBase
    {
        public ComboBoxControlModel()
        {
            ComboboxItems = new ObservableCollection<ComboBoxItem>();
            ShowNullValue = true;
            _selectedIndex = -1;
        }

        /// <summary>
        /// Получает коллекцию пунктов для комбобокса.
        /// </summary>
        public ObservableCollection<ComboBoxItem> ComboboxItems { get; private set; }

        /// <summary>
        /// Содержит Null пункт для comboбокса.
        /// </summary>
        private readonly ComboBoxItem _nullItem = new ComboBoxItem { Title = "Выберите…", Value = null };


        private bool _showNullValue;

        /// <summary>
        /// Задает или получает флаг указывающий на необходимость показа null value.
        /// </summary>
        public bool ShowNullValue
        {
            get { return _showNullValue; }
            set
            {
                if (value == _showNullValue && ComboboxItems.Any(i => ReferenceEquals(i, _nullItem)))
                {
                    return;
                } //if
                _showNullValue = value;

                var list = ComboboxItems.Where(comboboxItem => _nullItem != comboboxItem).ToList();

                if (_showNullValue)
                {

                    list.Add(_nullItem);
                } //if

                ComboboxItems.Clear();

                foreach (var comboboxItem in list)
                {
                    ComboboxItems.Add(comboboxItem);
                } //foreach
                SelectedItem = ComboboxItems.FirstOrDefault();
            }
        }

        private ComboBoxItem _selectedItem;

        /// <summary>
        /// Получает текущий выбранный пункт.
        /// </summary>
        public ComboBoxItem SelectedItem
        {
            get { return _selectedItem; }
            private set
            {
                _selectedItem = value;
                RisePropertyChanged("SelectedItem");
                var selectedIndex = ComboboxItems.IndexOf(value);
                if (selectedIndex >= 0 && selectedIndex != SelectedIndex)
                {
                    SelectedIndex = selectedIndex;
                } //if
            }
        }

        /// <summary>
        /// Производит очистку пунктов в комбобоксе.
        /// </summary>
        public void ClearComboboxItems()
        {
            var list = ComboboxItems.Where(i => !ReferenceEquals(i, _nullItem)).ToList();

            foreach (var comboboxItem in list)
            {
                ComboboxItems.Remove(comboboxItem);
            } //foreach
        }

        /// <summary>
        /// Получает индекс исходя из разных настроек.
        /// </summary>
        public int GetIndex()
        {
            if (RawValue == null || Value == null)
            {
                return _selectedIndex;
            } //if
            var item =
                ComboboxItems.FirstOrDefault(i => StringComparer.OrdinalIgnoreCase.Equals(i.Title, Value) && i.Value.Equals(RawValue));
            if (item == null)
            {
                return _selectedIndex;
            } //if

            return ComboboxItems.IndexOf(item);
        }

        /// <summary>
        /// Задает или получает флаг указывающий, что пользователь должен выбрать не null.
        /// </summary>
        public bool AllowNull { get; set; }

        private int _selectedIndex;

        /// <summary>
        /// Задает или получает индекс выбора.
        /// </summary>
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                if ((_selectedIndex != value || base.RawValue == null) && value <= ComboboxItems.Count && value >= 0)
                {
                    var item = ComboboxItems[value];

                    base.RawValue = item.Value;
                    RisePropertyChanged("Value");

                    base.Value = item.Title;
                    RisePropertyChanged("RawValue");
                    _selectedIndex = value;
                } //if

            }
        }

        /// <summary>
        /// Задает или получает исходное представление типа.
        /// </summary>
        public override object RawValue
        {
            get { return base.RawValue; }
            set
            {
                if (Equals(value, base.RawValue))
                {
                    return;
                } //if

                if (value == null)
                {
                    base.RawValue = null;
                    Value = _nullItem.Title;
                    SelectedItem = ComboboxItems.FirstOrDefault();
                    RisePropertyChanged("RawValue");
                    return;
                } //if

                var item = ComboboxItems.FirstOrDefault(i => Equals(i.Value, value));
                if (item != null)
                {
                    base.RawValue = item.Value;
                    Value = item.Title;
                    RisePropertyChanged("RawValue");
                } //if
                SelectedItem = item;
            }
        }

        /// <summary>
        /// Задает или получает текстовое представление значение контрола.
        /// </summary>        
        public override string Value
        {
            get { return base.Value; }
            set
            {
                if (StringComparer.Ordinal.Equals(value, base.Value))
                {
                    return;
                } //if
                if (string.IsNullOrWhiteSpace(value))
                {
                    base.Value = _nullItem.Title;
                    RawValue = null;
                    SelectedItem = ComboboxItems.FirstOrDefault();
                    RisePropertyChanged("Value");
                    return;
                } //if

                var item = ComboboxItems.FirstOrDefault(i => StringComparer.OrdinalIgnoreCase.Equals(i.Title, value));

                if (item != null)
                {
                    base.Value = item.Title;
                    RawValue = item.Value;
                    RisePropertyChanged("Value");
                } //if

                SelectedItem = item;
            }
        }

        /// <summary>
        /// Вызывается для проверки значения.
        /// </summary>
        public override void ValidateValue()
        {
            if (AllowNull)
            {
                IsAcceptable = true;
            } //if
            else
            {
                IsAcceptable = RawValue != null;
            } //else
        }

        /// <summary>
        /// Происходит когда значение изменяется.
        /// </summary>
        public event EventHandler<EventArgs> SelectedValueChanged;

        /// <summary>
        /// Вызывает событие изменение положения.
        /// </summary>
        public void RiseSelectedValueChanged()
        {
            EventHandler<EventArgs> handler = SelectedValueChanged;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        /// <summary>
        /// Пробует установить значение для параметра.
        /// </summary>
        /// <param name="value">Текстовое значение для установки.</param>
        /// <returns>При успехе возвращает true.</returns>
        public override bool TrySetValue(string value)
        {
            RawValue = value;

            return ComboboxItems.Any(it => StringComparer.OrdinalIgnoreCase.Equals(it.Value, value));
        }
    }

    /// <summary>
    /// Методы расширений для списка с выбором.
    /// </summary>
    public static class ComboBoxExtentions
    {
        /// <summary>
        /// Добавляет коллекцию элементов для выбора.
        /// </summary>
        /// <param name="model">Текущая модель.</param>
        /// <param name="items">Пункты которые требуется добавить.</param>
        public static void AddRange(this ComboBoxControlModel model, ICollection<ComboBoxItem> items)
        {
            foreach (var comboboxItem in items)
            {
                model.ComboboxItems.Add(comboboxItem);
            } //foreach
        }
    }
}
