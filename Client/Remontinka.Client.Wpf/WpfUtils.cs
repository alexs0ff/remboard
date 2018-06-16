using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Remontinka.Client.Wpf
{
    /// <summary>
    /// Утилиты для работы с Wpf.
    /// </summary>
    public static class WpfUtils
    {
        /// <summary>
        /// Переводит дату в строковое представление.
        /// </summary>
        /// <param name="eventDate">Дата.</param>
        /// <returns>Строковое представление.</returns>
        public static string DateTimeToString(DateTime eventDate)
        {
            return eventDate.ToShortDateString();
        }

        /// <summary>
        /// Переводит дату в строковое представление со временем.
        /// </summary>
        /// <param name="eventDate">Дата.</param>
        /// <returns>Строковое представление.</returns>
        public static string DateTimeToStringWithTime(DateTime eventDate)
        {
            return eventDate.ToString("dd.MM.yyyy hh:mm:ss");
        }

        /// <summary>
        /// Переводит число в строковое представление.
        /// </summary>
        /// <param name="value">Число.</param>
        /// <returns>Строковое представление.</returns>
        public static string DecimalToString(decimal? value)
        {
            if (value == null)
            {
                return string.Empty;
            } //if
            return value.Value.ToString("0.00", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Переводит строковое представление в число.
        /// </summary>
        /// <param name="value">Число.</param>
        /// <returns>Decimal представление при успешном преобразовании.</returns>
        public static decimal? StringToDecimal(string value)
        {
            if (value == null)
            {
                return null;
            } //if

            decimal dt;

            value = value.Replace(",", ".");

            if (decimal.TryParse(value,NumberStyles.Any,CultureInfo.InvariantCulture,out dt))
            {
                return dt;
            } //if

            return null;
        }

        /// <summary>
        /// Переводит число в строковое представление.
        /// </summary>
        /// <param name="value">Число.</param>
        /// <returns>Строковое представление.</returns>
        public static string IntToString(int? value)
        {
            if (value == null)
            {
                return string.Empty;
            } //if
            return value.Value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Собирает из отдельных ФИО одну строку.
        /// </summary>
        /// <param name="lastName">Фамилия.</param>
        /// <param name="firstName">Имя.</param>
        /// <param name="middleName">Отчетство.</param>
        /// <returns>Собранная строка.</returns>
        public static string GetPersonFullName(string lastName, string firstName, string middleName)
        {
            return string.Format("{0} {1} {2}", lastName, firstName, middleName);
        }
    }
}
