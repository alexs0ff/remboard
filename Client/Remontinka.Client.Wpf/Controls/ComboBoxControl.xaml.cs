using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Remontinka.Client.Wpf.Model.Controls;

namespace Remontinka.Client.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for ComboboxControl.xaml
    /// </summary>
    public partial class ComboBoxControl : UserControl, IFocusable, IControlDescriptor
    {
        public ComboBoxControl()
        {
            InitializeComponent();
            Loaded += (sender, args) =>
            {
                //HACK почему-то не выбирается при биндинге
                var comboboxControlModel = DataContext as ComboBoxControlModel;
                if (comboboxControlModel != null)
                {
                    if (comboboxControlModel.RawValue == null)
                    {
                        comboBox.SelectedValue =
                            comboboxControlModel.ComboboxItems.Select(i => i.Value).FirstOrDefault();
                    } //if

                    comboboxControlModel.PropertyChanged += ComboboxControlModelPropertyChanged;
                }


                if (comboboxControlModel != null && comboboxControlModel.RawValue != null)
                {
                    comboBox.SelectedIndex =
                         comboboxControlModel.GetIndex();
                    comboboxControlModel.ValidateValue();
                } //if

            };
        }

        void ComboboxControlModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedItem")
            {
                var comboboxControlModel = DataContext as ComboBoxControlModel;
                if (comboboxControlModel != null)
                {
                    comboBox.SelectedIndex =
                                       comboboxControlModel.GetIndex();
                    comboboxControlModel.ValidateValue();
                } //if
            } //if
        }

        private void ComboboxKeyUp(object sender, KeyEventArgs e)
        {
            var comboboxControlModel = DataContext as ComboBoxControlModel;
            if (comboboxControlModel != null)
            {
                comboboxControlModel.RisePressKey(new PressKeyEventArgs(comboBox, e.Key));
            } //if
        }

        private void ComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboboxControlModel = DataContext as ComboBoxControlModel;
            if (comboboxControlModel != null)
            {
                comboboxControlModel.SelectedIndex = comboBox.SelectedIndex;
                comboboxControlModel.ValidateValue();
                comboboxControlModel.RiseSelectedValueChanged();
            }
        }

        /// <summary>
        /// Установка фокуса на управляющий контрол.
        /// </summary>
        public void SetFocus()
        {
            if (!IsLoaded)
            {
                Loaded += (sender, args) => comboBox.Focus();
            } //if
            else
            {
                comboBox.Focus();
            } //else
        }

        /// <summary>
        /// Возвращает признак наличия фокуса в контроле.
        /// </summary>
        /// <returns>True - если есть фокус.</returns>
        public bool HasFocus()
        {
            return comboBox.IsFocused;
        }

        /// <summary>
        /// Задает или получает заголовок контрола.
        /// </summary>
        public string Title
        {
            get { return titleBox.Text; }
            set { titleBox.Text = value; }
        }

        /// <summary>
        /// Задает или получает ширину заголовка контрола.
        /// </summary>
        public GridLength TitleWidth
        {
            get { return leftCell.Width; }
            set { leftCell.Width = value; }
        }

        /// <summary>
        /// Получает имя контрола.
        /// </summary>
        public string ControlName
        {
            get { return Name; }
        }

        /// <summary>
        /// Получает тип контрола.
        /// </summary>
        public ControlType ControlType
        {
            get { return ControlType.ComboBoxControl; }
        }
    }
}
