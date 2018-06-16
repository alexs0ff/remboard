using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace Remontinka.Client.Wpf.Converters
{
    /// <summary>
    /// Конвертер значения boolean в соответствующий цвет текста.
    /// </summary>
    public class BoolToRedColorTextConverter : IValueConverter
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
            if (value == null)
            {
                return Brushes.Black;
            } //if

            var val = (bool)value;
            return val ? Brushes.Red : Brushes.Black;
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
            return value;
        }
    }
}
