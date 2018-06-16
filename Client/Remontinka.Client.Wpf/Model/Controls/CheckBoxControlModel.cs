using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remontinka.Client.Wpf.Model.Controls
{
    /// <summary>
    /// Модель для чек бокса.
    /// </summary>
    public class CheckBoxControlModel : ControlModelBase
    {
        /// <summary>
        /// Вызывается при изменении состояния checkboxа.
        /// </summary>
        public event EventHandler CheckBoxCheckedChanged;

        /// <summary>
        /// Вызывает события изменения состояния checkbox.
        /// </summary>
        private void RiseCheckBoxCheckedChanged()
        {
            EventHandler handler = CheckBoxCheckedChanged;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        /// <summary>
        /// Возвращает или задает значение флажка.
        /// </summary>
        public bool Checked
        {
            get { return (bool)RawValue; }
            set { RawValue = value; }
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
                    return false;
                } //if
                return base.RawValue;
            }
            set
            {
                if (!(value is bool))
                {
                    return;
                } //if

                var bv = (bool)value;

                var brv = (bool)RawValue;

                if (brv != bv)
                {
                    base.RawValue = value;
                    RisePropertyChanged("RawValue");
                    RiseCheckBoxCheckedChanged();
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
                if (!StringComparer.Ordinal.Equals(base.Value, value))
                {
                    bool bs;
                    if (!bool.TryParse(value, out bs))
                    {
                        return;
                    } //if

                    base.Value = value;
                    RawValue = bs;
                    RisePropertyChanged("Value");
                } //if
            }
        }

        /// <summary>
        /// Вызывается для проверки значения.
        /// </summary>
        public override void ValidateValue()
        {
            IsAcceptable = true;
        }
    }
}
