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
    /// Interaction logic for MaskedTextBoxControl.xaml
    /// </summary>
    public partial class MaskedTextBoxControl : UserControl, IFocusable
    {
        public MaskedTextBoxControl()
        {
            InitializeComponent();
        }

        private void TextBoxKeyUp(object sender, KeyEventArgs e)
        {
            UpdateValues();
            var textBoxControlModel = DataContext as MaskedTextBoxControlModel;
            if (textBoxControlModel != null)
            {
                textBoxControlModel.RisePressKey(new PressKeyEventArgs(textBox, e.Key));
            } //if
        }

        private void UpdateValues()
        {
            var textBoxControlModel = DataContext as MaskedTextBoxControlModel;
            if (textBoxControlModel != null)
            {
                textBoxControlModel.RawValue = textBox.MaskedTextProvider.ToString(false, false);
                textBoxControlModel.Value = textBox.MaskedTextProvider.ToString(true, true);
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
        /// Происходит когда происходит изменение текста.
        /// </summary>
        private void TextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBoxControlModel = DataContext as MaskedTextBoxControlModel;
            if (textBoxControlModel != null)
            {
                textBoxControlModel.RawValue = textBox.MaskedTextProvider.ToString(false, false);
                textBoxControlModel.ValidateValue();
            } //if
        }
    }
}
