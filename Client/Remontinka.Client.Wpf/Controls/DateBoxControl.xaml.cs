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
using Remontinka.Client.Wpf.Controllers.Controls;
using Remontinka.Client.Wpf.Model.Controls;

namespace Remontinka.Client.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for DateControl.xaml
    /// </summary>
    public partial class DateBoxControl : UserControl, IFocusable, IControlDescriptor
    {
        public DateBoxControl()
        {
            InitializeComponent();
        }

        private void SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var periodBoxModel = DataContext as DateControlModel;
            if (periodBoxModel != null)
            {
                periodBoxModel.ValidateValue();
            } //if
        }

        private void BoxKeyUp(object sender, KeyEventArgs e)
        {
            var periodBoxModel = DataContext as DateControlModel;
            if (periodBoxModel != null)
            {
                periodBoxModel.RisePressKey(new PressKeyEventArgs(dateTimeBox, e.Key));
            } //if
        }

        /// <summary>
        /// Установка фокуса на управляющий контрол.
        /// </summary>
        public void SetFocus()
        {
            if (!IsLoaded)
            {
                Loaded += (sender, args) => dateTimeBox.Focus();
            } //if
            else
            {
                dateTimeBox.Focus();
            } //else
        }

        /// <summary>
        /// Возвращает признак наличия фокуса в контроле.
        /// </summary>
        /// <returns>True - если есть фокус.</returns>
        public bool HasFocus()
        {
            return dateTimeBox.IsFocused;
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
            get { return ControlType.DateBoxControl; }
        }
    }
}
