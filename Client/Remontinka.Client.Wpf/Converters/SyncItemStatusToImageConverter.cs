using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Remontinka.Client.Core.Models;

namespace Remontinka.Client.Wpf.Converters
{
    /// <summary>
    /// Конвертер текущего статуса к картинке.
    /// </summary>
    public class SyncItemStatusToImageConverter : IValueConverter
    {
        /// <summary>
        /// Url картинки для обрабатываемых пунктов.
        /// </summary>
        public const string PreparingImageUri = "/Remboard;component/Images/repeat.png";

        /// <summary>
        /// Url картинки для обрабатываемых пунктов.
        /// </summary>
        public const string ProcessImageUri = "/Remboard;component/Images/arrow.png";

        /// <summary>
        /// Url картинки для успешных пунктов.
        /// </summary>
        public const string SuccessImageUri = "/Remboard;component/Images/checkmark.png";

        /// <summary>
        /// Url картинки для неудачных пунктов.
        /// </summary>
        public const string FailedImageUri = "/Remboard;component/Images/yellow_ball.png";

        /// <summary>
        ///   Modifies the source data before passing it to the target for display in the UI.
        /// </summary>
        /// <returns>
        ///   The value to be passed to the target dependency property.
        /// </returns>
        /// <param name = "value">The source data being passed to the target.</param>
        /// <param name = "targetType">The <see cref = "T:System.Type" /> of data expected by the target dependency property.</param>
        /// <param name = "parameter">An optional parameter to be used in the converter logic.</param>
        /// <param name = "culture">The culture of the conversion.</param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BitmapImage result = null;

            if (value is SyncItemStatus)
            {
                var keyType = (SyncItemStatus)value;

                switch (keyType)
                {
                    case SyncItemStatus.Preparing:
                        result =
                            new BitmapImage(new Uri(PreparingImageUri,
                                                    UriKind.Relative));
                        break;
                    case SyncItemStatus.Processing:
                        result =
                            new BitmapImage(new Uri(ProcessImageUri,
                                                    UriKind.Relative));
                        break;
                    case SyncItemStatus.Failed:
                        result =
                            new BitmapImage(new Uri(FailedImageUri,
                                                    UriKind.Relative));
                        break;
                    case SyncItemStatus.Success:
                        result =
                            new BitmapImage(new Uri(SuccessImageUri,
                                                    UriKind.Relative));
                        break;
                } //switch
            } //if

            return result;
        }

        /// <summary>
        ///   Modifies the target data before passing it to the source object.  This method is called only in <see
        ///    cref = "F:System.Windows.Data.BindingMode.TwoWay" /> bindings.
        /// </summary>
        /// <returns>
        ///   The value to be passed to the source object.
        /// </returns>
        /// <param name = "value">The target data being passed to the source.</param>
        /// <param name = "targetType">The <see cref = "T:System.Type" /> of data expected by the source object.</param>
        /// <param name = "parameter">An optional parameter to be used in the converter logic.</param>
        /// <param name = "culture">The culture of the conversion.</param>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
