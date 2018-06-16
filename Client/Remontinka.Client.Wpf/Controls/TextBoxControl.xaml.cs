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
    /// Interaction logic for TextBoxControl.xaml
    /// </summary>
    public partial class TextBoxControl : UserControl, IFocusable, IControlDescriptor
    {
        public TextBoxControl()
        {
            InitializeComponent();
        }

        private void TextBoxKeyUp(object sender, KeyEventArgs e)
        {
            var textBoxControlModel = DataContext as TextBoxControlModel;
            if (textBoxControlModel != null)
            {
                textBoxControlModel.RawValue = textBoxControlModel.Value = textBox.Text;
                textBoxControlModel.RisePressKey(new PressKeyEventArgs(textBox, e.Key));
            } //if
        }

        /// <summary>
        /// Установка фокуса на управляющий контрол.
        /// </summary>
        public void SetFocus()
        {
            if (!IsLoaded)
            {
                Loaded += (sender, args) => textBox.Focus();
            } //if
            else
            {
                textBox.Focus();
            } //else
        }

        /// <summary>
        /// Возвращает признак наличия фокуса в контроле.
        /// </summary>
        /// <returns>True - если есть фокус.</returns>
        public bool HasFocus()
        {
            return textBox.IsFocused;
        }

        /// <summary>
        /// Получает тип контрола.
        /// </summary>
        public ControlType ControlType
        {
            get { return ControlType.TextBoxControl;}
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
        /// Получает имя контрола.
        /// </summary>
        public string ControlName {
            get { return Name; }
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
        /// Задает или получает признак много строчности.
        /// </summary>
        public bool IsMultiline
        {
            get { return textBox.AcceptsReturn; }
            set
            {
                if (value)
                {
                    textBox.TextWrapping = TextWrapping.Wrap;
                    textBox.AcceptsReturn = true;
                    textBox.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
                } //if
                else
                {
                    textBox.AcceptsReturn = false;
                    textBox.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
                } //else
            }
        }
    }
}
