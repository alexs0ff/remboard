using System;
using System.Globalization;

namespace Remontinka.Client.Wpf.Model.Controls
{
    /// <summary>
    /// Модель для выбора даты.
    /// </summary>
    public class DateControlModel : ControlModelBase
    {
        /// <summary>
        /// Задает или получает минимальную возможную дату.
        /// </summary>
        public DateTime? MinDate { get; set; }

        /// <summary>
        /// Задает или получает максимальновозможную дату.
        /// </summary>
        public DateTime? MaxDate { get; set; }

        /// <summary>
        /// Задает или получает признак допустимости Null значения.
        /// </summary>
        public bool AllowNull { get; set; }

        /// <summary>
        /// Задает или получает текстовое представление значение контрола.
        /// </summary>        
        public override string Value
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(base.Value))
                {
                    return base.Value.Replace("/", ".").Replace("-", ".");
                } //if

                return
                    base.Value;
            }
            set
            {
                if (value == null)
                {
                    if (base.Value!=null)
                    {
                        base.Value = null;
                        RisePropertyChanged("Value");    
                    } //if
                } //if
                else
                if (!StringComparer.Ordinal.Equals(base.Value, value))
                {
                    DateTime res;
                    if (DateTime.TryParseExact(value, "dd.MM.yyyy", CultureInfo.InvariantCulture,
                                               DateTimeStyles.None, out res))
                    {
                        RawValue = res;
                    } //if
                    base.Value = value;
                    RisePropertyChanged("Value");
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
                if (value == null)
                {
                    if (base.RawValue!=null)
                    {
                        Value = null;

                        base.RawValue = null;
                        RisePropertyChanged("RawValue");    
                    } //if
                    ValidateValue();
                } //if
                else 
                if (value is DateTime)
                {
                    var dt = (DateTime)value;
                    var rv = RawValue as DateTime?;

                    if (dt != rv)
                    {
                        base.RawValue = value;
                        Value = dt.ToString("dd.MM.yyyy");
                        RisePropertyChanged("RawValue");
                    } //if
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
            try
            {
                DateTime res;
                if (DateTime.TryParseExact(value, "dd.MM.yyyy", CultureInfo.InvariantCulture,
                                           DateTimeStyles.None, out res))
                {
                    RawValue = res;
                } //if
                else
                {
                    throw new FormatException();
                } //else
            }
            catch (Exception)
            {
                return false;
            } //try

            return true;
        }

        /// <summary>
        /// Производит валидацию значения.
        /// </summary>
        public override void ValidateValue()
        {
            if (RawValue == null)
            {
                if (!AllowNull)
                {
                    IsAcceptable = false;
                    return;
                } //if
            } //if

            if (MinDate == null && MaxDate == null)
            {
                IsAcceptable = true;
                return;
            } //if

            var curDate = (DateTime)RawValue;

            if (MinDate != null && MaxDate != null)
            {
                IsAcceptable = curDate >= MinDate && curDate <= MaxDate;
                return;
            } //if

            if (MinDate != null)
            {
                IsAcceptable = curDate >= MinDate;
                return;
            } //if

            if (MaxDate != null)
            {
                IsAcceptable = curDate <= MaxDate;
            } //if
        }
    }
}
