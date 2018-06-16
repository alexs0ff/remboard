using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Remontinka.Client.DataLayer.Entities
{
    public static class FormatUtils
    {
        /// <summary>
        /// Формат перевода строки в 
        /// </summary>
        private const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// Переводит дату в строковое предстовление.
        /// </summary>
        /// <param name="value">Значение даты.</param>
        /// <returns>Строковое представление.</returns>
        public static string DateTimeToString(DateTime value)
        {
            return value.ToString(DateTimeFormat, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Переводит дату в строковое предстовление.
        /// </summary>
        /// <param name="value">Значение даты.</param>
        /// <returns>Строковое представление.</returns>
        public static string DateTimeToString(DateTime? value)
        {
            if (!value.HasValue)
            {
                return null;
            } //if
            return value.Value.ToString(DateTimeFormat, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Переводит строковое представление в дату.
        /// </summary>
        /// <param name="value">Строковое предствление</param>
        /// <returns>Дата или null</returns>
        public static DateTime? StringToNullDateTime(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            } //if

            DateTime dt;

            if (DateTime.TryParseExact(value,DateTimeFormat,CultureInfo.InvariantCulture,DateTimeStyles.AllowWhiteSpaces, out dt))
            {
                return dt;
            } //if
            return null;
        }

        /// <summary>
        /// Переводит строковое представление в дату.
        /// </summary>
        /// <param name="value">Строковое предствление</param>
        /// <returns>Дата или MinDate</returns>
        public static DateTime StringToDateTime(string value)
        {
            return StringToNullDateTime(value) ?? DateTime.MinValue;
        }

        /// <summary>
        /// Переводит Guid в строковое предстовление.
        /// </summary>
        /// <param name="value">Значение Guid.</param>
        /// <returns>Строковое представление.</returns>
        public static string GuidToString(Guid value)
        {
            return value.ToString();
        }

        /// <summary>
        /// Переводит Guid в строковое предстовление.
        /// </summary>
        /// <param name="value">Значение Guid.</param>
        /// <returns>Строковое представление.</returns>
        public static string GuidToString(Guid? value)
        {
            if (!value.HasValue)
            {
                return null;
            } //if
            return value.ToString();
        }

        /// <summary>
        /// Переводит строковое представление в Guid.
        /// </summary>
        /// <param name="value">Строковое предствление</param>
        /// <returns>Guid или null</returns>
        public static Guid? StringToNullGuid(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            } //if

            Guid dt;

            if (Guid.TryParse(value, out dt))
            {
                return dt;
            } //if
            return null;
        }

        /// <summary>
        /// Переводит строковое представление в Guid.
        /// </summary>
        /// <param name="value">Строковое предствление</param>
        /// <returns>Дата или Empty</returns>
        public static Guid StringToGuid(string value)
        {
            return StringToNullGuid(value) ?? Guid.Empty;
        }

        /// <summary>
        /// Переводит long в bool.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <returns>Булевское значение.</returns>
        public static bool LongToBool(long value)
        {
            return value != 0;
        }

        /// <summary>
        /// Переводит bool в long.
        /// </summary>
        /// <param name="value">Булевское значение.</param>
        /// <returns>Значение.</returns>
        public static long BoolToLong(bool value)
        {
            return value?1:0;
        }

        /// <summary>
        /// Перемещает значение из полей представления Guid.
        /// </summary>
        /// <param name="guidValue">Guid значение.</param>
        /// <param name="stringValue">Строковое представление.</param>
        /// <param name="newValue">Значение для изменения. </param>
        public static void ExchangeFields(ref Guid? guidValue, ref string stringValue,string newValue)
        {
            if (stringValue==newValue)
            {
                return;
            } //if

            stringValue = newValue;
            guidValue = StringToNullGuid(stringValue);
            
        }

        /// <summary>
        /// Перемещает значение из полей представления Guid.
        /// </summary>
        /// <param name="guidValue">Guid значение.</param>
        /// <param name="stringValue">Строковое представление.</param>
        /// <param name="newValue">Значение для изменения.</param>
        public static void ExchangeFields(ref Guid? guidValue, ref string stringValue, Guid? newValue)
        {
            if (guidValue==newValue)
            {
                return;
            } //if
            guidValue = newValue; 
            stringValue = GuidToString(guidValue);
        }

        /// <summary>
        /// Перемещает значение из полей представления DateTime.
        /// </summary>
        /// <param name="dateTimeValue">DateTime значение.</param>
        /// <param name="stringValue">Строковое представление.</param>
        /// <param name="newValue">Новое значение. </param>
        public static void ExchangeFields(ref DateTime? dateTimeValue, ref string stringValue,string newValue)
        {
            if (newValue==stringValue)
            {
                return;
            } //if
            stringValue = newValue;
            dateTimeValue = StringToNullDateTime(stringValue);
        }

        /// <summary>
        /// Перемещает значение из полей представления DateTime.
        /// </summary>
        /// <param name="dateTimeValue">DateTime значение.</param>
        /// <param name="stringValue">Строковое представление.</param>
        /// <param name="newValue">Новое значение. </param>
        public static void ExchangeFields(ref DateTime? dateTimeValue, ref string stringValue, DateTime? newValue)
        {
            if (newValue==dateTimeValue)
            {
                return;
            } //if
            dateTimeValue = newValue;
            stringValue = DateTimeToString(dateTimeValue);
        }

        /// <summary>
        /// Перемещает значение из полей представления DateTime.
        /// </summary>
        /// <param name="dateTimeValue">DateTime значение.</param>
        /// <param name="stringValue">Строковое представление.</param>
        /// <param name="newValue">Новое значение. </param>
        public static void ExchangeFields(ref DateTime dateTimeValue, ref string stringValue, string newValue)
        {
            if (newValue == stringValue)
            {
                return;
            } //if

            stringValue = newValue;

            dateTimeValue = StringToDateTime(stringValue);
        }

        /// <summary>
        /// Перемещает значение из полей представления DateTime.
        /// </summary>
        /// <param name="dateTimeValue">DateTime значение.</param>
        /// <param name="stringValue">Строковое представление.</param>
        /// <param name="newValue">Новое значение. </param>
        public static void ExchangeFields(ref DateTime dateTimeValue, ref string stringValue, DateTime newValue)
        {
            if (newValue == dateTimeValue)
            {
                return;
            } //if

            dateTimeValue = newValue;

            stringValue = DateTimeToString(dateTimeValue);
        }
        
        /// <summary>
        /// Перемещает значение из полей представления bool.
        /// </summary>
        /// <param name="boolValue">Long значение.</param>
        /// <param name="longValue">Строковое представление.</param>
        /// <param name="newValue">Новое значение. </param>
        public static void ExchangeFields(ref bool boolValue, ref long longValue, long newValue)
        {
            if (newValue == longValue)
            {
                return;
            } //if

            longValue = newValue;

            boolValue = LongToBool(longValue);
        }

        /// <summary>
        /// Перемещает значение из полей представления bool.
        /// </summary>
        /// <param name="boolValue">Boolean значение.</param>
        /// <param name="longValue">Строковое представление.</param>
        /// <param name="newValue">Новое значение. </param>
        public static void ExchangeFields(ref bool boolValue, ref long longValue, bool newValue)
        {
            if (newValue == boolValue)
            {
                return;
            } //if

            boolValue = newValue;

            longValue = BoolToLong(boolValue);
        }
    }
}
