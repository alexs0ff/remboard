using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Remontinka.Client.Core.Interception
{
    /// <summary>
    ///   The event args of value changed.
    /// </summary>
    public class ValueChangedEventArgs : EventArgs
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:Remontinka.Client.Core.Interception.ValueChangedEventArgs" /> class.
        /// </summary>
        public ValueChangedEventArgs(PropertyInfo propertyInfo, object target, object oldValue, object newValue)
        {
            PropertyInfo = propertyInfo;
            Target = target;
            OldValue = oldValue;
            NewValue = newValue;
        }

        /// <summary>
        ///   The property info of changed property.
        /// </summary>
        public PropertyInfo PropertyInfo { get; private set; }

        /// <summary>
        ///   The changed object.
        /// </summary>
        public Object Target { get; private set; }

        /// <summary>
        ///   The old value.
        /// </summary>
        public Object OldValue { get; private set; }

        /// <summary>
        ///   The new value.
        /// </summary>
        public Object NewValue { get; private set; }
    }
}
