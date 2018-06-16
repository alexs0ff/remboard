using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Romontinka.Server.WebSite.Metadata
{
    /// <summary>
    /// Биндер для представления даты.
    /// </summary>
    public class DateTimeModelBinder : IModelBinder
    {
        /// <summary>
        /// Binds the model to a value by using the specified controller context and binding context.
        /// </summary>
        /// <returns>
        /// The bound value.
        /// </returns>
        /// <param name="controllerContext">The controller context.</param><param name="bindingContext">The binding context.</param>
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var val = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (string.IsNullOrWhiteSpace(val.AttemptedValue))
            {
                return null;
            }

            DateTime dt;
            var modValue = val.AttemptedValue.Replace("-", ".").Replace("/", ".").Replace("\\", ".");
            bool success = DateTime.TryParse(modValue, CultureInfo.GetCultureInfo("ru-RU"),
                                             DateTimeStyles.None, out dt);

            if (success)
            {
                return dt;

            }

            return DateTime.Today;
        }
    }
}