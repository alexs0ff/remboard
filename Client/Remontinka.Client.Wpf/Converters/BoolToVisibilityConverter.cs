using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Remontinka.Client.Wpf.Converters
{
    /// <summary>
    /// Конвертер с Bool значения для настройки отображения контрола.
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Преобразует значение. 
        /// </summary>
        /// <returns>
        /// Преобразованное значение.Если метод возвращает null, используется действительное значение null.
        /// </returns>
        /// <param name="value">Значение, произведенное исходной привязкой.</param><param name="targetType">Тип свойства цели связывания.</param><param name="parameter">Параметр используемого преобразователя.</param><param name="culture">Язык и региональные параметры, используемые в преобразователе.</param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Visibility.Visible : Visibility.Hidden;
        }

        /// <summary>
        /// Преобразует значение. 
        /// </summary>
        /// <returns>
        /// Преобразованное значение.Если метод возвращает null, используется действительное значение null.
        /// </returns>
        /// <param name="value">Значение, произведенное целью привязки.</param><param name="targetType">Тип, к которому выполняется преобразование.</param><param name="parameter">Используемый параметр преобразователя.</param><param name="culture">Язык и региональные параметры, используемые в преобразователе.</param>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
