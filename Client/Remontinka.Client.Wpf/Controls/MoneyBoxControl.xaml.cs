using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for MoneyBoxControl.xaml
    /// </summary>
    public partial class MoneyBoxControl : UserControl, IFocusable, IControlDescriptor
    {
        public MoneyBoxControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Вызывается, когда необработанное вложенное событие<see cref="E:System.Windows.Input.Keyboard.PreviewKeyDown"/> встречает на своем пути элемент, производный от этого класса.Реализация этого метода позволяет добавить обработчик класса для данного события.
        /// </summary>
        /// <param name="e">Объект <see cref="T:System.Windows.Input.KeyEventArgs"/>, содержащий данные, относящиеся к событию.</param>
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            ProcessValue(e.Key);
            e.Handled = true;
            base.OnPreviewKeyDown(e);
        }

        private void TextBoxKeyUp(object sender, KeyEventArgs e)
        {
            var moneyControlModel = DataContext as MoneyBoxControlModel;
            if (moneyControlModel != null)
            {
                if (!moneyControlModel.ReadOnly)
                {
                    moneyControlModel.Value = textBox.Text;
                    decimal value;
                    if (decimal.TryParse(textBox.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
                    {
                        moneyControlModel.RawValue = value;
                    }
                } //if
                moneyControlModel.RisePressKey(new PressKeyEventArgs(textBox, e.Key));
            } //if
        }

        /// <summary>
        /// Максимальное количество цифр для текста.
        /// </summary>
        private const int MoneyDigitMax = 10;

        /// <summary>
        /// Предобработка формата значения.
        /// </summary>
        /// <param name="key">Нажатая клавиша.</param>
        private void ProcessValue(Key key)
        {
            string buffer = textBox.Text;

            if (buffer.Length > MoneyDigitMax && key != Key.Back)
            {
                return;
            } //if

            if (buffer.Length == 1)
            {
                buffer = "0.0" + buffer;
            } //if

            if ((key == Key.Back))
            {
                if (textBox.Text.Length > 0 && buffer.Length > 0)
                {
                    buffer = buffer.Remove(buffer.Length - 1);
                }
            }
            if ((key >= Key.D0 && key <= Key.D9) || (key >= Key.NumPad0 && key <= Key.NumPad9))
            {
                buffer = buffer + KeyToString(key);
            }

            if (buffer.Length > 2)
            {
                buffer = buffer.Replace(".", "");
                buffer = buffer.Insert(buffer.Length - 2, ".");
            }

            if (buffer.Length > 4)
            {
                buffer = buffer.TrimStart('0');
            }

            if (buffer.StartsWith("."))
            {
                buffer = buffer.Insert(0, "0");
            }

            textBox.Text = buffer;
            textBox.Select(buffer.Length, 0);
        }

        /// <summary>
        /// Парсит введеную клавишу.
        /// </summary>
        /// <param name="key">Клавиша.</param>
        /// <returns>Строковое значение.</returns>
        private string KeyToString(Key key)
        {
            string result = string.Empty;

            switch (key)
            {
                case Key.D0:
                case Key.NumPad0:
                    result = "0";
                    break;
                case Key.D1:
                case Key.NumPad1:
                    result = "1";
                    break;
                case Key.D2:
                case Key.NumPad2:
                    result = "2";
                    break;
                case Key.D3:
                case Key.NumPad3:
                    result = "3";
                    break;
                case Key.D4:
                case Key.NumPad4:
                    result = "4";
                    break;
                case Key.D5:
                case Key.NumPad5:
                    result = "5";
                    break;
                case Key.D6:
                case Key.NumPad6:
                    result = "6";
                    break;
                case Key.D7:
                case Key.NumPad7:
                    result = "7";
                    break;
                case Key.D8:
                case Key.NumPad8:
                    result = "8";
                    break;
                case Key.D9:
                case Key.NumPad9:
                    result = "9";
                    break;
            }

            return result;
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
        /// Получает заголовок контрола.
        /// </summary>
        public string Title
        {
            get { return titleBox.Text; }
            set { titleBox.Text = value; }
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
            get { return ControlType.MoneyBoxControl; }
        }
    }
}
