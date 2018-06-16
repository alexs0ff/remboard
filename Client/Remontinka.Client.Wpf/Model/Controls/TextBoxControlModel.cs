using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Remontinka.Client.Wpf.Model.Controls
{
    /// <summary>
    /// Модель поля ввода текста.
    /// </summary>
    public class TextBoxControlModel : ControlModelBase
    {
        /// <summary>
        /// Производит валидацию значения.
        /// </summary>
        public override void ValidateValue()
        {
            if (!string.IsNullOrWhiteSpace(RegexText))
            {
                var value = Value ?? string.Empty;

                var match = Regex.Match(value, RegexText);
                IsAcceptable = match.Success;
            } //if
            else
            {
                IsAcceptable = true;
            } //else
        }

        /// <summary>
        /// Задает или получает текст Regex.
        /// </summary>
        public string RegexText { get; set; }

        /// <summary>
        /// Задает или получает текстовое представление значение контрола.
        /// </summary>        
        public override string Value
        {
            get { return base.Value; }
            set
            {
                if (!StringComparer.Ordinal.Equals(base.Value, value))
                {
                    base.Value = value;
                    RawValue = value;
                    RisePropertyChanged("Value");
                } //if
            }
        }

        /// <summary>
        /// Задает или получает исходное представление типа.
        /// </summary>
        public override object RawValue
        {
            get
            {
                return base.RawValue;
            }
            set
            {
                var str = (string)value;
                var rv = (string)RawValue;

                if (!StringComparer.Ordinal.Equals(str, rv))
                {
                    base.RawValue = value;
                    Value = (string)value;
                    RisePropertyChanged("RawValue");
                } //if

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
            Value = value;
            return true;
        }
    }
}
