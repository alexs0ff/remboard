using System;
using System.Globalization;

namespace Romontinka.Server.WebSite.Helpers
{
    /// <summary>
    /// Утилиты для инфраструктуры.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Создает jSon строковое представления пунктов массиа.
        /// </summary>
        /// <param name="items">Массив пунктов.</param>
        /// <returns>Строковое представление.</returns>
        public static string ArrayToJson(string[] items)
        {
            if (items==null ||items.Length==0)
            {
                return "[]";
            } //if
            return string.Format("[\"{0}\"]", string.Join("\",\"",items));
        }

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
            return eventDate.ToString("dd.MM.yyyy HH:mm:ss",CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Переводит число в строковое представление.
        /// </summary>
        /// <param name="value">Число.</param>
        /// <returns>Строковое представление.</returns>
        public static string DecimalToString(decimal? value)
        {
            if (value==null)
            {
                return string.Empty;
            } //if
            return value.Value.ToString("0.00", CultureInfo.InvariantCulture);
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
            return string.Format("{0} {1} {2}",lastName,firstName,middleName);
        }

        /// <summary>
        /// Переводит строку для MVC контролов дату.
        /// </summary>
        /// <param name="value">Значение для перевода.</param>
        /// <returns>Переведенная строка.</returns>
        public static string MvcControlDateToString(DateTime value)
        {
            if (value == DateTime.MinValue)
            {
                value = DateTime.Today;
            }

            return value.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Получение первого дня месяца.
        /// </summary>
        /// <param name="value">День месяца.</param>
        /// <returns>Первый день месяца.</returns>
        public static DateTime GetFirstDayOfMonth(DateTime value)
        {
            if (value == DateTime.MinValue)
            {
                return DateTime.MinValue;
            } //if

            return new DateTime(value.Year, value.Month, 1);
        }

        /// <summary>
        /// Получение последнего дня месяца.
        /// </summary>
        /// <param name="value">День месяца.</param>
        /// <returns>Последний день месяца.</returns>
        public static DateTime GetLastDayOfMonth(DateTime value)
        {
            if (value == DateTime.MaxValue)
            {
                return DateTime.MaxValue;
            } //if

            return GetFirstDayOfMonth(value).AddMonths(1).AddDays(-1);
        }


    }
}