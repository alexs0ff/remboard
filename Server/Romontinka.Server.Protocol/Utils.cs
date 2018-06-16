using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;

namespace Romontinka.Server.Protocol
{
    /// <summary>
    /// Содержит разные вспомогательные методы.
    /// </summary>
    public static class Utils
    {
        
        /// <summary>
        /// Читает значение элемента в ноде.
        /// </summary>
        /// <param name="node">Нод содержащий элемент.</param>
        /// <param name="xpath">XPath.</param>
        /// <param name="defaultValue">Значение по умолчанию.</param>
        /// <returns>Значение.</returns>
        public static string ReadElementValue(XmlNode node, string xpath, string defaultValue)
        {
            var element = node.SelectSingleNode(xpath);
            if (element != null)
            {
                return element.InnerText;
            } //if

            return defaultValue;
        }

        /// <summary>
        /// Переводит целочисленное значение в строковое представление.
        /// </summary>
        /// <param name="value">Значение.</param>
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
        /// Переводит целочисленное значение в строковое представление.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <returns>Строковое представление.</returns>
        public static string IntToString(long? value)
        {
            if (value == null)
            {
                return string.Empty;
            } //if

            return value.Value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Переводит уникальный идентификатор в строковое представление.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <returns>Строковое представление.</returns>
        public static string GuidToString(Guid? value)
        {
            if (value == null)
            {
                return null;
            } //if

            return value.Value.ToString();
        }


        /// <summary>
        /// Переводит значение времени и даты в строковое представление.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <returns>Строковое представление.</returns>
        public static string DateTimeToString(DateTime? value)
        {
            if (value == null)
            {
                return string.Empty;
            } //if

            return value.Value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Переводит дробное значение в строковое представление.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <returns>Строковое представление.</returns>
        public static string DecimalToString(decimal? value)
        {
            if (value == null)
            {
                return string.Empty;
            } //if

            return value.Value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Переводит булевское значение в строковое представление.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <returns>Строковое представление.</returns>
        public static string BooleanToString(bool? value)
        {
            if (value == null)
            {
                return string.Empty;
            } //if

            return value.Value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Разбирает строку в boolean значение.
        /// </summary>
        /// <param name="boolValue">Строка для разбора.</param>
        /// <returns>Bool значение</returns>
        public static bool ParseBoolean(string boolValue)
        {
            return bool.Parse(boolValue);
        }

        /// <summary>
        /// Разбирает строку в целочисленное значение.
        /// </summary>
        /// <param name="intValue">Строка для разбора.</param>
        /// <returns></returns>
        public static int ParseInt(string intValue)
        {
            return Int32.Parse(intValue, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Разбирает строку в целочисленное значение nullable.
        /// </summary>
        /// <param name="intValue">Строка для разбора.</param>
        /// <returns></returns>
        public static int? ParseNullInt(string intValue)
        {
            if (string.IsNullOrWhiteSpace(intValue))
            {
                return null;
            } //if
            return Int32.Parse(intValue, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Разбирает строку в уникальный идентификатор.
        /// </summary>
        /// <param name="guidValue">Строка для разбора.</param>
        /// <returns></returns>
        public static Guid? ParseGuid(string guidValue)
        {
            if (string.IsNullOrWhiteSpace(guidValue))
            {
                return null;
            }
            return Guid.Parse(guidValue);
        }

        /// <summary>
        /// Разбирает строку в значение byte.
        /// </summary>
        /// <param name="byteValue">Строка для разбора.</param>
        /// <returns></returns>
        public static byte ParseByte(string byteValue)
        {
            return Byte.Parse(byteValue, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Разбирает строку в значение времени и даты.
        /// </summary>
        /// <param name="dateTimeValue">Строка для разбора.</param>
        /// <returns>Разобранное значение.</returns>
        public static DateTime? ParseDateTime(string dateTimeValue)
        {
            if (string.IsNullOrWhiteSpace(dateTimeValue))
            {
                return null;
            } //if

            return DateTime.Parse(dateTimeValue, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Возвращает целочисленное представление строки или null если строку не удалось разобрать.
        /// </summary>
        /// <param name="value">Строка для разбора.</param>
        /// <returns>Целочисленное значение.</returns>
        public static int? GetIntegerOrNull(string value)
        {
            int result;

            if (!int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out result))
            {
                return null;
            } //if

            return result;
        }

        /// <summary>
        /// Возвращает целочисленное представление строки или null если строку не удалось разобрать.
        /// </summary>
        /// <param name="value">Строка для разбора.</param>
        /// <returns>Целочисленное значение.</returns>
        public static long? GetLongOrNull(string value)
        {
            long result;

            if (!long.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out result))
            {
                return null;
            } //if

            return result;
        }

        /// <summary>
        /// Возвращает вещественное представление строки.
        /// </summary>
        /// <param name="value">Строка для разбора.</param>
        /// <returns>Полученная строка.</returns>
        public static decimal? GetDecimalValueOrNull(string value)
        {
            decimal? result;
            decimal res;
            if (!decimal.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out res))
            {
                result = null;
            } //if
            else
            {
                result = res;
            } //else

            return result;
        }

    }
}
