using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Remontinka.Client.Core.Interception;

namespace Remontinka.Client.Wpf.Model.Controls
{
    /// <summary>
    /// Модель контрола по текст боксу с маской.
    /// </summary>
    public class MaskedTextBoxControlModel : ControlModelBase
    {
        /// <summary>
        /// Производит валидацию значения.
        /// </summary>
        public override void ValidateValue()
        {
            if (!string.IsNullOrWhiteSpace(RegexText))
            {
                var strval = RawValue as string;

                if (!string.IsNullOrWhiteSpace(strval))
                {
                    var match = Regex.Match(strval, RegexText);
                    IsAcceptable = match.Success;
                } //if
                else
                {
                    IsAcceptable = false;
                }
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
        /// Задает или получает маску для текста.
        /// </summary>
        [NotifyPropertyChanged]
        public virtual string Mask { get; set; }

        /// <summary>
        /// Задает или получает исходное представление типа.
        /// </summary>
        public override object RawValue
        {
            get { return base.RawValue; }
            set
            {
                var str = base.RawValue as string;
                var rv = value as string;

                if (!StringComparer.Ordinal.Equals(str, rv) && (str != null || rv != null))
                {
                    base.RawValue = value;
                    RisePropertyChanged("RawValue");
                } //if
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
                if (!StringComparer.Ordinal.Equals(value, base.Value))
                {
                    base.Value = value;
                    RisePropertyChanged("Value");
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
            Value = value;
            return true;
        }
    }
}
