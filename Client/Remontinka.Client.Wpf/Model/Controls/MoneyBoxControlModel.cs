using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Remontinka.Client.Wpf.Model.Controls
{
    /// <summary>
    /// Модель для формы ввода чисел.
    /// </summary>
    public class MoneyBoxControlModel : ControlModelBase
    {
        public MoneyBoxControlModel()
        {
            Value = "0.00";
            RawValue = decimal.Zero;
        }

        /// <summary>
        /// Вызывается для проверки значения.
        /// </summary>
        public override void ValidateValue()
        {
            if ((MaxValue == MinValue && MaxValue == decimal.Zero))
            {
                IsAcceptable = true;
                return;
            } //if

            var value = decimal.Zero;

            if (RawValue != null)
            {
                value = (decimal)RawValue;
            } //if

            if (MaxValue == decimal.Zero && MinValue >= decimal.Zero)
            {
                IsAcceptable = value >= MinValue;
                return;
            } //if

            IsAcceptable = value <= MaxValue && value >= MinValue;

        }

        /// <summary>
        /// Задает или получает исходное представление типа.
        /// </summary>
        public override object RawValue
        {
            get
            {
                if (base.RawValue == null)
                {
                    return decimal.Zero;
                } //if
                return
                base.RawValue;
            }
            set
            {
                if (value is decimal)
                {
                    var val = (decimal)value;
                    var rw = decimal.Zero;
                    if (base.RawValue != null)
                    {
                        rw = (decimal)base.RawValue;
                    } //if

                    if (val != rw)
                    {
                        Value = (val).ToString("0.00", CultureInfo.InvariantCulture);
                        base.RawValue = value;
                        RisePropertyChanged("RawValue");
                    } //if

                } //if
            }
        }

        /// <summary>
        /// Задает или получает текстовое представление значение контрола.
        /// </summary>        
        public override string Value
        {
            get
            {
                return base.Value;
            }
            set
            {
                var val = value;
                var rw = base.Value;
                decimal dv;
                if (!StringComparer.Ordinal.Equals(val, rw) && decimal.TryParse(val, NumberStyles.Any, CultureInfo.InvariantCulture, out dv))
                {
                    base.Value = value;
                    RawValue = dv;
                    RisePropertyChanged("Value");
                } //if
            }
        }

        /// <summary>
        /// Задает или получает минимальное значение.
        /// </summary>
        public virtual decimal MinValue { get; set; }

        /// <summary>
        /// Задает или получает максимальное значение.
        /// </summary>
        public virtual decimal MaxValue { get; set; }
    }
}
